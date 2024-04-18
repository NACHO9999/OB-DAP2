using ob.Domain;

namespace ob.IBusinessLogic;

public interface IInvitacionService
{
    void CrearInvitacion(Invitacion invitacion);
    Invitacion GetInvitacionByEmail(string email);
    void EliminarInvitacion(string email);
    
}