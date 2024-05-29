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

}