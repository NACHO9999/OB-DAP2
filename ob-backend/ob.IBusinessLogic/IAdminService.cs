using ob.Domain;

namespace ob.IBusinessLogic;

public interface IAdminService
{
    void CrearAdmin(Administrador admin);
    void BorrarAdmin(int id);
}
