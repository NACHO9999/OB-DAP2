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
    void AsignarSolicitud(Solicitud solicitud, Mantenimiento mantenimiento);
    int [] GetSolicitudByEdificio(Edificio edificio);
    int [] GetSolicitudByMantenimiento(Mantenimiento mantenimiento);
    TimeSpan? TiempoPromedioAtencion(Mantenimiento mantenimiento);
}