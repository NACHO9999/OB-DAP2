using ob.Domain;

namespace ob.IBusinessLogic;

public interface IEncargadoService
{
    void CrearEncargado(Encargado encargado);
    IEnumerable<Encargado> GetAllEncargados();
    Encargado GetEncargadoByEmail(string email);
    void CrearEdificio(string email, Edificio edificio);
    void EditarEdificio(string email, Edificio edificio);
    void BorrarEdificio(string nombre, string direccion, string email);
    void CrearMantenimiento(Mantenimiento mantenimiento);
    void CrearSolicitud(Solicitud solicitud, string email);
    void AsignarSolicitud(Guid solicitudId, string email, string emailEncargado);
    int [] GetSolicitudByEdificio(string nombre, string direccion);
    int [] GetSolicitudByMantenimiento(string email, string emailEncargado);
    TimeSpan? TiempoPromedioAtencion(string  email);
    Edificio GetEdificioByNombreYDireccion(string nombre, string direccion, string email);
    void CrearDepto(string email, Depto depto);
    Depto GetDepto(int numero, string nombre, string direccion, string email);
    void EditarDepto(string email, Depto depto);

}