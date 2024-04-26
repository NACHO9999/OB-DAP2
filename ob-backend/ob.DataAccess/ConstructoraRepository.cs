using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class ConstructoraRepository : GenericRepository<Constructora>
    {
        public ConstructoraRepository(DbContext context)
        {
            Context = context;
        }
    }
}