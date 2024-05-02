using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class DuenoRepository : GenericRepository<Dueno>
    {
        public DuenoRepository(DbContext context)
        {
            Context = context;
        }
    }
}