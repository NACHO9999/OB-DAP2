using ob.Domain;
using Enums;
using ob.IDataAccess;
using ob.IBusinessLogic;
using ob.Exceptions.BusinessLogicExceptions;
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
    public Solicitud GetSolicitudById(Guid id)
    {
        if (SolicitudExists(id))
        {
            return _repository.Get(s => s.Id == id);
        }
        else
        {
            throw new KeyNotFoundException("No se encontró la solicitud.");
        }
    }
    private bool SolicitudExists(Guid id)
    {
        return _repository.Get(s => s.Id == id) != null;
    }
    public List<Solicitud> GetSolicitudesByEdificio(Edificio edificio)
    {
        List<Solicitud> solicitudesInEdificio = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "Depto" }))
        {
            if (edificio.Deptos.Contains(solicitud.Depto))
            {
                solicitudesInEdificio.Add(solicitud);
            }

        }

        return solicitudesInEdificio;
    }
    public IEnumerable<Solicitud> GetSolicitudes()
    {
        return _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "Categoria", "PerMan", "Depto" });
    }
    public List<Solicitud> GetSolicitudesByCategoria(Categoria categoria)
    {
        List<Solicitud> solicitudesByCategoria = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "Categoria" }))
        {
            if (solicitud.Categoria == categoria)
            {
                solicitudesByCategoria.Add(solicitud);
            }
        }
        return solicitudesByCategoria;
    }
    public List<Solicitud> GetSolicitudesByMantenimiento(Mantenimiento mant)
    {
        List<Solicitud> solicitudesByMantenimiento = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "PerMan" }))
        {
            if (solicitud.PerMan == mant)
            {
                solicitudesByMantenimiento.Add(solicitud);
            }
        }
        return solicitudesByMantenimiento;
    }
}