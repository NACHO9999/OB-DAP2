using ob.Domain;
namespace ob.IBusinessLogic;

public interface IAdminConstructoraService
{
    void CrearAdminConstructora(AdminConstructora adminConstructora);
    AdminConstructora GetAdminConstructoraByEmail(string email);
    void CrearConstructora(string nombre, string email);
    void CrearEdificio(Edificio edificio, string email);
    void BorrarEdificio(string nombre, string direccion, string email);
    void EditarEdificio(Edificio edificio, string email);
    void CrearDepto(string email, Depto depto);
    Edificio GetEdificioByNombreYDireccion(string nombre, string direccion, string email);
    void EditarDepto(string email, Depto depto);
    void BorrarDepto(string email, int numero, string nombre, string direccion);
    Depto GetDepto(int numero, string nombre, string direccion, string email);
    void EditarConstructora(Constructora constructora, string email);
    void AsignarConstructora(string email, string nombreConstructora);
    public void DesasignarEncargado(string email, string edNombre, string edDireccion);
    void AsignarEncargado(string email, string emailEncargado, string nombreEdificio, string direccionEdificio);
    List<Edificio> GetEdificiosPorAdmin(string email);
    List<Edificio> GetEdificiosSinEncargado(string email);
    List<Edificio> GetEdificiosConEncargado(string email);
    List<Edificio> FiltrarPorNombreDeEncargado(string email, string nombreEncargado);
    bool TieneConstructora(string email);
    void ElegirConstructora(string email, string nombre);
    Constructora GetConstructora(string email);
    List<Constructora> GetConstructoras();
}