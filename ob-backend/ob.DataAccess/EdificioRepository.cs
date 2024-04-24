using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class EdificioRepository : GenericRepository<Edificio>
    {
        public EdificioRepository(DbContext context)
        {
            Context = context;
        }
    }
}