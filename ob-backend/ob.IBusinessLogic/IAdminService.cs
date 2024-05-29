using Enums;
using ob.Domain;

namespace ob.IBusinessLogic;

public interface IAdminService
{
    void CrearAdmin(Administrador admin);
    Administrador GetAdminByEmail(string email);
    IEnumerable<Administrador> GetAllAdmins();
    void Invitar(string email, string nombre, DateTime fechaLimite, RolInvitaciion rol);
    void EliminarInvitacion(string email);
    void AltaCategoria(Categoria categoria);
}
