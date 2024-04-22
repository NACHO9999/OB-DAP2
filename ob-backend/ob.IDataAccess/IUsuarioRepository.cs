using ob.Domain;
using ob.IDataAccess;


public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    bool EmailExists(string email);
}