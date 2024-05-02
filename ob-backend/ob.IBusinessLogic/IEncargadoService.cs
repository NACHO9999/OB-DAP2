using ob.Domain;

namespace ob.IBusinessLogic;

public interface IEncargadoService
{
    void CrearEncargado(Encargado encargado);
    IEnumerable<Encargado> GetAllEncargados();
    Encargado GetEncargadoByEmail(string email);
    void CrearEdificio(Encargado encargado, Edificio edificio);
    void EditarEdificio(Edificio edificio);
    void BorrarEdificio(string nombre);
    Edificio GetEdificioByNombre(Encargado encargado, string nombre);
    void CrearMantenimiento(Mantenimiento mantenimiento);
    void CrearSolicitud(Solicitud solicitud);
    void AsignarSolicitud(Guid solicitudId, string email);
    int [] GetSolicitudByEdificio(string nombre, string direccion);
    int [] GetSolicitudByMantenimiento(string email);
    TimeSpan? TiempoPromedioAtencion(string  email);
}