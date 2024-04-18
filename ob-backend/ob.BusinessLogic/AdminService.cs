using ob.Domain;
using ob.IBusinessLogic;

namespace ob.BusinessLogic;

public class AdminService : IAdminService
{
    public void CrearAdmin(Administrador admin){}
    public void BorrarAdmin(string email){}
    public void EditarAdmin(Administrador admin){}
    public Administrador GetAdminByEmail(string email){
        return null;
    }
    public void InvitarEncargado(Encargado encargado){}
    public void EliminarInvitacion(string email){}
    public void AltaCategoria(Categoria categoria){}
}
