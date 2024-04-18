using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class UsuarioRepository : GenericRepository<Usuario>
    {
        public UsuarioRepository(DbContext context)
        {
            Context = context;
        }
       

        // Example method to retrieve alldad Encargados
        public IEnumerable<Encargado> GetAllEncargados()
        {
            return GetAll<Encargado>(null, new List<string> { "Edificios" });
        }

        // Example method to retrieve all Mantenimientos
        public IEnumerable<Mantenimiento> GetAllMantenimientos()
        {
            return GetAll<Mantenimiento>();
        }

        // Example method to retrieve an Administrador by email


        // Example method to retrieve an Encargado by email
        public Encargado GetEncargadoByEmail(string email)
        {
            var usuario = Get(u => u.Email == email);
            if (usuario is Encargado encargado)
            {
                return encargado;
            }
            else
            {
                throw new Exception("No hay encargado con este mail.");
            }
        }

        public Mantenimiento GetMantenimientoByEmail(string email)
        {
            var usuario = Get(u => u.Email == email);
            if (usuario is Mantenimiento mantenimiento)
            {
                return mantenimiento;
            }
            else
            {
                throw new Exception("No hay persona de mantenimiento con este mail");
            }
        }
       

        public void AddEncargado(string nombre, string apellido, string email, string contrasena)
        {
            if (!EmailExists(email))
            {
                var encargado = new Encargado(nombre, apellido, email, contrasena);
                Insert(encargado);
                Save();
            }
            else
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        public void AddMantenimiento(string nombre, string apellido, string email, string contrasena)
        {
            if (!EmailExists(email))
            {
                var mantenimiento = new Mantenimiento(nombre, apellido, email, contrasena);
                Insert(mantenimiento);
                Save();
            }
            else
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        

        

    }
}
