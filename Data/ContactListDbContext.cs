using ContactListApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactListApi.Data
{
    public class ContactListDbContext : DbContext
    {
        public ContactListDbContext(DbContextOptions<ContactListDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contato>()
                .ToTable("contato", "public") 
                .Property(c => c.Id)
                .HasColumnName("id"); 

            modelBuilder.Entity<Contato>()
                .Property(c => c.PessoaId)
                .HasColumnName("pessoa_id");

            modelBuilder.Entity<Contato>()
                .Property(c => c.Tipo)
                .HasColumnName("tipo");

            modelBuilder.Entity<Contato>()
                .Property(c => c.Valor)
                .HasColumnName("valor");

            modelBuilder.Entity<Pessoa>()
                .ToTable("pessoa", "public")
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Pessoa>()
                .Property(p => p.Nome)
                .HasColumnName("nome");
        }




        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Contato> Contatos { get; set; }
    }
}
