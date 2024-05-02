using ob.Domain;
namespace ob.IBusinessLogic;

public interface ISolicitudService
{
    void CrearSolicitud(Solicitud solicitud);
    void EditarSolicitud(Solicitud solicitud);
    List<Solicitud> GetSolicitudesByEdificio(Edificio edificio);
    List<Solicitud> GetSolicitudesByMantenimiento(Mantenimiento mant);
    List<Solicitud> GetSolicitudesByCategoria(Categoria categoria);
    IEnumerable<Solicitud> GetSolicitudes ();
    Solicitud GetSolicitudById(Guid id);



}