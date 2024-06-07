using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
using Enums;
namespace ob.BusinessLogic;

public class InvitacionService : IInvitacionService
{
    private IGenericRepository<Invitacion> _repository;
    private IEncargadoService _encargadoService;
    private IAdminConstructoraService _adminConstructoraService;

    public InvitacionService(IGenericRepository<Invitacion> invitacionRepository, IEncargadoService encargadoService, IAdminConstructoraService adminConstructoraService)
    {
        _repository = invitacionRepository;
        _encargadoService = encargadoService;
        _adminConstructoraService = adminConstructoraService;
    }
    public void CrearInvitacion(Invitacion invitacion)
    {
        if (InvitacionExiste(invitacion.Email))
        {
            throw new AlreadyExistsException("La invitacion ya existe");
        }
        _repository.Insert(invitacion);
        _repository.Save();
    }
    public Invitacion GetInvitacionByEmail(string email)
    {
        var invitacion = _repository.Get(i => i.Email.ToLower() == email.ToLower());
        if (invitacion == null)
        {
            throw new KeyNotFoundException("No se encontr� la invitacion.");
        }
        return invitacion;
    }
    public void EliminarInvitacion(string email)
    {
        if (!InvitacionExiste(email))
        {
            throw new KeyNotFoundException("No se encontr� la invitacion.");
        }
        var invitacion = _repository.Get(i => i.Email.ToLower() == email.ToLower());
        _repository.Delete(invitacion);
        _repository.Save();
    }
    public void InvitacionAceptada(string email, string contrasena)
    {
        var invitacion = GetInvitacionByEmail(email);
        if (invitacion.FechaExpiracion < DateTime.Now)
        {
            throw new InvalidOperationException("La invitacion ha expirado.");
        }
        if (invitacion.Rol == RolInvitaciion.Encargado)
        {
            var encargado = new Encargado(invitacion.Nombre, invitacion.Email, contrasena);
            _encargadoService.CrearEncargado(encargado);
        }
        else
        {
            var adminConst = new AdminConstructora(invitacion.Nombre, invitacion.Email, contrasena);
            _adminConstructoraService.CrearAdminConstructora(adminConst);
        }
        EliminarInvitacion(invitacion.Email);
    }
    public bool InvitacionExiste(string email)
    {
        return _repository.Get(i => i.Email.ToLower() == email.ToLower()) != null;
    }
    public List<Invitacion> GetInvitaciones()
    {
        return _repository.GetAll<Invitacion>().ToList();
    }

}