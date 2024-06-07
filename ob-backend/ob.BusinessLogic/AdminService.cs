using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
using Enums;


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

    public IEnumerable<Administrador> GetAllAdmins()
    {
        return _repository.GetAll<Usuario>()
                         .OfType<Administrador>();
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
            throw new KeyNotFoundException("No se encontr� el administrador.");
        }
    }
    public void Invitar(string email, string nombre, DateTime fecha, RolInvitaciion rol)
    {
        var invitacion = new Invitacion(email, nombre, fecha, rol);
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
