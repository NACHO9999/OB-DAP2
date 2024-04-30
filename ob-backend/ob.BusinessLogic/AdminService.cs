using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;


namespace ob.BusinessLogic;

public class AdminService : IAdminService
{
    private readonly IUsuarioRepository _repository;

    private readonly ICategoriaService _categoriaService;
    private readonly IInvitacionService _invitacionService;
    public AdminService(IUsuarioRepository repository, ICategoriaService categoriaService, IInvitacionService invitacionService)
    {
        _repository = repository;
        _categoriaService = categoriaService;
        _invitacionService = invitacionService;
    }

    
    public void CrearAdmin(Administrador admin)
    {
        if (_repository.EmailExists(admin.Email))
        {
            throw new AlreadyExistsException("El email ya est� en uso.");
        }
        _repository.Insert(admin);
        _repository.Save();
    }
    public Administrador GetAdminByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower());
        if (usuario is Administrador administrador)
        {
            return administrador;
        }
        else
        {
            throw new ResourceNotFoundException("No se encontr� el administrador.");
        }
    }
    public void InvitarEncargado(string email, string nombre, DateTime fecha)
    {
        var invitacion = new Invitacion(email, nombre, fecha);
        _invitacionService.CrearInvitacion(invitacion);
        _repository.Save();
    }
    public void EliminarInvitacion(string email)
    {
        _invitacionService.EliminarInvitacion(email);
        _repository.Save();
    }
    public void AltaCategoria(Categoria categoria)
    {
        _categoriaService.CrearCategoria(categoria);
    }
    
}
