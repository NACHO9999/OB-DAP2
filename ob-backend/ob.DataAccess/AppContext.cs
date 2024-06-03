using System;
using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            {
                modelBuilder.Entity<Categoria>()
                .HasKey(c => c.Nombre); // Set Nombre as primary key

                modelBuilder.Entity<Constructora>()
                .HasKey(c => c.Id); // Set Rut as primary key


                modelBuilder.Entity<Depto>()
                .HasKey(d => new { d.Numero, d.EdificioNombre, d.EdificioDireccion });

                modelBuilder.Entity<Edificio>()
                .HasKey(e => new { e.Nombre, e.Direccion }); // Set Nombre and Direccion as primary key

                modelBuilder.Entity<Edificio>()
                .Property(e => e.GastosComunes)
                .HasColumnType("decimal(18,2)");

                modelBuilder.Entity<Dueno>()
                .HasKey(d => d.Email); // Set Email as primary key

                modelBuilder.Entity<Invitacion>()
                .HasKey(i => i.Email); // Set email as primary key

                modelBuilder.Entity<Solicitud>()
                .HasKey(s => s.Id); // Set Id as primary key

                modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Email); // Set Id as primary key

                modelBuilder.Entity<Administrador>()
                .HasBaseType<Usuario>(); // Inherit properties from Usuario

                modelBuilder.Entity<AdminConstructora>()
                .HasBaseType<Usuario>(); // Inherit properties from Usuario

                modelBuilder.Entity<AdminConstructora>()
                .HasOne(a => a.Constructora) // An Admin is related to one Constructora
                .WithMany()
                .IsRequired(false); // A Constructora can have many Admins


                modelBuilder.Entity<Encargado>()
                .HasBaseType<Usuario>(); // Inherit properties from Usuario

                modelBuilder.Entity<Session>()
                    .HasOne(s => s.Usuario) // A Session is related to one Usuario
                    .WithMany(); // A Usuario can have many Sessions


                modelBuilder.Entity<Encargado>()
                    .HasMany(e => e.Edificios) // An Encargado can manage multiple Edificios
                    .WithOne(); // An Edificio is managed by one Encargado

                modelBuilder.Entity<Edificio>()
                    .HasOne(e => e.EmpresaConstructora) // An Edificio belongs to one Constructora
                    .WithMany() // A Constructora can have many Edificios
                    .IsRequired(); // Ensure a relationship

                modelBuilder.Entity<Edificio>()

                    .HasMany(e => e.Deptos)
                    .WithOne()
                    .IsRequired(false);

                modelBuilder.Entity<Depto>()
                    .HasOne(d => d.Dueno) // A Depto belongs to one Dueno
                    .WithMany() // A Dueno can own many Deptos
                    .IsRequired(false); // Allow a Depto to not have a Dueno

                modelBuilder.Entity<Mantenimiento>()
                    .HasBaseType<Usuario>(); // Inherit properties from Usuario

                modelBuilder.Entity<Solicitud>()
                    .HasOne(s => s.PerMan) // A Solicitud is assigned to one Mantenimiento
                    .WithMany() // A Mantenimiento can have many Solicituds
                    .IsRequired(false); // Allow a Solicitud to not have a Mantenimiento

                modelBuilder.Entity<Solicitud>()
                    .HasOne(s => s.Depto) // A Solicitud is related to one Depto
                    .WithMany() // A Depto can have many Solicituds
                    .IsRequired(); // Ensure a relationship

                modelBuilder.Entity<Solicitud>()
                    .HasOne(s => s.Categoria) // A Solicitud is categorized under one Categoria
                    .WithMany() // A Categoria can have many Solicituds
                    .IsRequired(); // Ensure a relationship
            }
        }


    }
}