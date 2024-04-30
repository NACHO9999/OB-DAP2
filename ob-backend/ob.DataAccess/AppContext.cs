using System;
using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ob.DataAccess
{
    public class AppContext : DbContext
    {
        public AppContext() { }
        public AppContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Usuario>? Usuarios { get; set; }
        public virtual DbSet<Categoria>? Categorias { get; set; }
        public virtual DbSet<Depto>? Deptos { get; set; }
        public virtual DbSet<Dueno>? Duenos { get; set; }
        public virtual DbSet<Edificio>? Edificios { get; set; }
        public virtual DbSet<Invitacion>? Invitaciones { get; set; }
        public virtual DbSet<Solicitud>? Solicitudes { get; set; }
        public virtual DbSet<Constructora>? Constructoras { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        public IConfiguration? Config { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Config ??= new ConfigurationBuilder()
                .AddJsonFile($@"{Directory.GetCurrentDirectory()}/appsettings.json")
                .Build();

            var parsed = bool.TryParse(Config?.GetSection("Env")["Testing"], out bool isTesting);
            isTesting = parsed && isTesting;

            string connectionString = Config?
                .GetConnectionString(isTesting ? "DBTest" : "DB")
                ?? throw new Exception("Connection string not found");

            optionsBuilder.UseSqlServer(connectionString);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) { }


    }
}