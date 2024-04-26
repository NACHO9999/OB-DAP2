using ob.Domain;

namespace ob.IBusinessLogic;

public interface IAdminService
{
    void CrearAdmin(Administrador admin);
    Administrador GetAdminByEmail(string email);
    void InvitarEncargado(string email, string nombre, DateTime fechaLimite);
    void EliminarInvitacion(string email);
    void AltaCategoria(Categoria categoria);
}
