using ob.Domain;
using Enums;
using ob.IDataAccess;
using ob.IBusinessLogic;
namespace ob.BusinessLogic;

public class SolicitudService : ISolicitudService
{
    private readonly IGenericRepository<Solicitud> _repository;
    public SolicitudService(IGenericRepository<Solicitud> solicitudRepository)
    {
        _repository = solicitudRepository;
    }
    public void CrearSolicitud(Solicitud solicitud)
    {
        _repository.Insert(solicitud);
        _repository.Save();
    }
    public void EditarSolicitud(Solicitud solicitud)
    {
        _repository.Update(solicitud);
        _repository.Save();
    }
    public List<Solicitud> GetSolicitudesByEdificio(Edificio edificio)
    {
        List<Solicitud> solicitudesInEdificio = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>()) // Assuming GetAllSolicitudes() returns all solicitudes
        {
            if (edificio.Deptos.Contains(solicitud.Depto))
            {
                solicitudesInEdificio.Add(solicitud);
            }

        }

        return solicitudesInEdificio;
    }
    public List<Solicitud> GetSolicitudesByMantenimiento(Mantenimiento mant)
    {
        List<Solicitud> solicitudesByMantenimiento = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>()) // Assuming GetAllSolicitudes() returns all solicitudes
        {
            if (solicitud.PerMan == mant)
            {
                solicitudesByMantenimiento.Add(solicitud);
            }
        }
        return solicitudesByMantenimiento;
    }
}