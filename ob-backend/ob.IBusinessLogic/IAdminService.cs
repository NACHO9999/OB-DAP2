using ob.Domain;

namespace ob.IBusinessLogic;

public interface IAdminService
{
    void CrearAdmin(Administrador admin);
    void BorrarAdmin(string email);
    void EditarAdmin(Administrador admin);
    Administrador GetAdminByEmail(string email);
    void InvitarEncargado(Encargado encargado);
    void EliminarInvitacion(string email);
    void AltaCategoria(Categoria categoria);
}
