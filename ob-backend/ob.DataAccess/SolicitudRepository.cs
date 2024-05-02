using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess
{
    public class SolicitudRepository : GenericRepository<Solicitud>
    {
        public SolicitudRepository(DbContext context)
        {
            Context = context;
        }
    }
}