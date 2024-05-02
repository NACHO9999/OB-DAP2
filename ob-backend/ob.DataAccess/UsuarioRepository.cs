using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DbContext context)
        {
            Context = context;
        }
        public bool EmailExists(string email)
        {
            return Get(u => u.Email == email) != null;
        }


        
    }
}
