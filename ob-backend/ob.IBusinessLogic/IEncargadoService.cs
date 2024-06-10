using ob.Domain;

namespace ob.IBusinessLogic;

public interface IEncargadoService
{
    void CrearEncargado(Encargado encargado);
    Encargado GetEncargadoByEmail(string email);
    IEnumerable<Encargado> GetAllEncargados();
    void CrearMantenimiento(Mantenimiento mantenimiento);
    void CrearSolicitud(Solicitud solicitud, string email);
    void AsignarSolicitud(Guid solicitudId, string email, string emailEncargado);
    int[] GetSolicitudByEdificio(string nombre, string direccion, string email);
    int[] GetSolicitudByMantenimiento(string email, string emailEncargado);
    TimeSpan? TiempoPromedioAtencion(string email);
    Dueno GetDueno(string email);
    void AsignarDueno(int numero, string edNombre, string edDireccion, Dueno dueno, string email);
    List<Mantenimiento> GetAllMantenimiento();
    void DesasignarDueno(int numero, string edNombre, string edDireccion, string email);
    List<Solicitud> GetSolicitudesSinMantenimiento(string email);
    List<Solicitud> GetAllEncargadoSolicitudes(string email);

}