using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
namespace ob.BusinessLogic;

public class InvitacionService : IInvitacionService
{
    private IGenericRepository<Invitacion> _repository;

    public InvitacionService(IGenericRepository<Invitacion> invitacionRepository)
    {
        _repository = invitacionRepository;
    }
    public void CrearInvitacion(Invitacion invitacion)
    {
        if (InvitacionExiste(invitacion.Email))
        {
            throw new Exception("La invitacion ya existe");
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
        var invitacion = _repository.Get(i => i.Email.ToLower() == email.ToLower());
        _repository.Delete(invitacion);
        _repository.Save();
    }
    public Encargado InvitacionAceptada(Invitacion invitacion, string contrasena){
        var encargado = new Encargado(invitacion.Nombre,  invitacion.Email, contrasena);        
        EliminarInvitacion(invitacion.Email);
        return encargado;
    }
    public bool InvitacionExiste(string email)
    {
        return GetInvitacionByEmail != null;
    }

    
}