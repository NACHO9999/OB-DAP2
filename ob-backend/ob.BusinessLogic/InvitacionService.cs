using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
namespace ob.BusinessLogic;

public class InvitacionService : IInvitacionService
{
    private IGenericRepository<Invitacion> _repository;
    private IEncargadoService _encargadoService;

    public InvitacionService(IGenericRepository<Invitacion> invitacionRepository,IEncargadoService encargadoService)
    {
        _repository = invitacionRepository;
        _encargadoService = encargadoService;
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
        return _repository.Get(i => i.Email.ToLower() == email.ToLower());
    }
    public void EliminarInvitacion(string email)
    {
        if (!InvitacionExiste(email))
        {
            throw new KeyNotFoundException("No se encontró la invitacion.");
        }
        var invitacion = _repository.Get(i => i.Email.ToLower() == email.ToLower());
        _repository.Delete(invitacion);
        _repository.Save();
    }
    public void InvitacionAceptada(Invitacion invitacion, string contrasena){
        if(invitacion.FechaExpiracion < DateTime.Now)
        {
            throw new InvalidOperationException("La invitacion ha expirado.");
        }
        var encargado = new Encargado(invitacion.Nombre, invitacion.Email, contrasena);
        EliminarInvitacion(invitacion.Email);
        _encargadoService.CrearEncargado(encargado);
    }
    public bool InvitacionExiste(string email)
    {
        var invitacion = GetInvitacionByEmail(email);
        return invitacion != null;
    }


}