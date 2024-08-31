using ContactListApi.Data;
using ContactListApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly ContactListDbContext _context;

        public PessoasController(ContactListDbContext context)
        {
            _context = context;
        }

        // GET api/pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return await _context.Pessoas.Include(p =>p.Contatos).ToListAsync();
        }

        // GET api/pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas
                                       .Include(p => p.Contatos)  // Inclui os contatos relacionados
                                       .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        // POST api/pessoas
        [HttpPost]
        public async Task<ActionResult<Pessoa>> CreatePessoa(Pessoa pessoa)
        {
            // Validação básica
            if (string.IsNullOrEmpty(pessoa.Nome))
            {
                return BadRequest("O nome da pessoa é obrigatório.");
            }

            var contatos = new List<Contato>();
            var existeContatos = false;

            if (pessoa.Contatos != null)
            {
                contatos = (List<Contato>)pessoa.Contatos;
                pessoa.Contatos = null;
                existeContatos = true;
            }

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            if (existeContatos)
            {
                foreach (var contato in contatos)
                {
                    contato.PessoaId = pessoa.Id;
                    _context.Contatos.Add(contato);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
        }

        // PUT api/pessoas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            _context.Pessoas.Update(pessoa);

            _context.Entry(pessoa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/pessoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            
            if (pessoa == null)
            {
                return NotFound();
            }

            var contatos = await _context.Contatos.Where(p => p.PessoaId == id).ToListAsync();

            if (contatos != null)
            {
                foreach (var contato in contatos)
                {
                    _context.Contatos.Remove(contato);
                }
            }            

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
