using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class DeptoRepository : GenericRepository<Depto>
    {
        public DeptoRepository(DbContext context)
        {
            Context = context;
        }
    }
}