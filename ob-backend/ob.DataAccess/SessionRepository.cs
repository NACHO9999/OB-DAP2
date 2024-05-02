
using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess;
public class SessionRepository : GenericRepository<Session>
{
    public SessionRepository(DbContext context)
    {
        Context = context;
    }

    public override void Insert(Session session)
    {
        if (session.Usuario != null)
        {
            Context.Entry(session.Usuario).State = EntityState.Unchanged;
        }
        Context.Set<Session>().Add(session);
    }
}