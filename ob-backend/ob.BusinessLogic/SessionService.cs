using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
namespace ob.BusinessLogic;


public class SessionService : ISessionService
{
    private IUsuarioRepository _usuarioRepository;
    private IGenericRepository<Session> _sessionRepository;
    private Usuario? _currentUser;
    public SessionService(IUsuarioRepository usuarioRepository, IGenericRepository<Session> sessionRepository)
    {
        _usuarioRepository = usuarioRepository;
        _sessionRepository = sessionRepository;
    }
    public Usuario? GetCurrentUser(Guid? authToken = null)
    {
        if (_currentUser != null)
            return _currentUser;

        if (authToken == null)
            throw new ArgumentException("Cant retrieve user without auth token");

        var session = _sessionRepository.Get(s => s.AuthToken == authToken, new List<string>() { "Usuario" });
     


        if (session != null)
            _currentUser = session.Usuario;

        return _currentUser;
    }

    public Guid Authenticate(string email, string password)
    {
        var user = _usuarioRepository.Get(u => u.Email.ToLower() == email.ToLower() && u.Contrasena == password);

        if (user == null)
            throw new InvalidCredentialException("Invalid credentials");

        var session = new Session() { Usuario = user };
        _sessionRepository.Insert(session);
        _sessionRepository.Save();

        return session.AuthToken;
    }

    public string GetUserRole(string email, string password)
    {
        var user = _usuarioRepository.Get(u => u.Email.ToLower() == email.ToLower() && u.Contrasena == password);

        if (user == null)
            throw new InvalidCredentialException("Invalid credentials");

        if(user is Administrador)
            return "admin";
        else if(user is AdminConstructora)
            return "admin_constructora";
        else if(user is Encargado)
            return "encargado";
        else 
            return "mantenimiento";   
    }
    public void Logout(Guid authToken)
    {
        var session = _sessionRepository.Get(s => s.AuthToken == authToken);
        if (session != null)
        {
            _sessionRepository.Delete(session);
            _sessionRepository.Save();
        }
    }
}