using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;

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
    }
    public Invitacion GetInvitacionByEmail(string email)
    {
        return _repository.Get(i => i.Email == email);
    }
    public void EliminarInvitacion(string email)
    {
        var invitacion = _repository.Get(i => i.Email == email);
        _repository.Delete(invitacion);
    }
    public Encargado InvitacionAceptada(Invitacion invitacion, string contrasena, string apellido){
        var encargado = new Encargado(invitacion.Nombre, apellido, invitacion.Email, contrasena);        
        _repository.Delete(invitacion);
        return encargado;
    }
    public bool InvitacionExiste(string email)
    {
        return _repository.Get(i => i.Email == email) != null;
    }

    
}