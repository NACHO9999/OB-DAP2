
using Microsoft.EntityFrameworkCore;
using ob.Domain;
using System.Linq.Expressions;
namespace ob.DataAccess;
public class SessionManagment : GenericRepository<Session>
{
    public SessionManagment(DbContext context)
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