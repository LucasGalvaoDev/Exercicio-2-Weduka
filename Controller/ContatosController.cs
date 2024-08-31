using ContactListApi.Data;
using ContactListApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactListApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : ControllerBase
    {
        private readonly ContactListDbContext _context;

        public ContatosController(ContactListDbContext context)
        {
            _context = context;
        }

        // GET api/contatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatos()
        {
            var contatos = await _context.Contatos.ToListAsync();
            return Ok(contatos);
        }

        // GET api/contatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contato>> GetContato(int id)
        {
            return await _context.Contatos.FindAsync(id);
        }

        // POST api/contatos
        [HttpPost]
        public async Task<ActionResult<Contato>> CreateContato(Contato contato)
        {
            var pessoa = await _context.Pessoas.FindAsync(contato.PessoaId);
            if (pessoa == null)
            {
                return BadRequest("Pessoa associada ao contato não encontrada.");
            }

            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContato), new { id = contato.Id }, contato);
        }

        // DELETE api/contatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
