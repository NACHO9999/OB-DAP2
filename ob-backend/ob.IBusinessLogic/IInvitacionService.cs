using ob.Domain;

namespace ob.IBusinessLogic;

public interface IInvitacionService
{
    void CrearInvitacion(Invitacion invitacion);
    Invitacion GetInvitacionByEmail(string email);
    void EliminarInvitacion(string email);
    void InvitacionAceptada(string email, string contrasena);
    bool InvitacionExiste(string email);
    List<Invitacion> GetInvitaciones();

}