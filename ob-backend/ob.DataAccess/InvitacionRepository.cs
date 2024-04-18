using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class InvitacionRepository : GenericRepository<Usuario>
    {
        public InvitacionRepository(DbContext context)
        {
            Context = context;
        }
    }
}