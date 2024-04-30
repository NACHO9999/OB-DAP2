using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class CategoriaRepository : GenericRepository<Categoria>
    {
     
        public CategoriaRepository(DbContext context)
        {
            Context = context;
        }
        
    }
}