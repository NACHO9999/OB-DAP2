using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class DeptosRepository : GenericRepository<Depto>
    {
        public DeptosRepository(DbContext context)
        {
            Context = context;
        }
    }
}