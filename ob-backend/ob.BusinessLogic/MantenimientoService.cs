using ob.Domain;
using Enums;
using ob.IDataAccess;
using ob.IBusinessLogic;
using ob.Exceptions.BusinessLogicExceptions;
namespace ob.BusinessLogic;

public class MantenimientoService : IMantenimientoService
{
    private readonly IUsuarioRepository _repository;
    private readonly ISolicitudService _solicitudService;
    public MantenimientoService(IUsuarioRepository repository, ISolicitudService solicitudService)
    {
        _repository = repository;
        _solicitudService = solicitudService;
    }
    
    public void CrearMantenimiento(Mantenimiento mantenimiento)
    {
        if (_repository.EmailExists(mantenimiento.Email))
        {
            throw new AlreadyExistsException("El mantenimiento ya existe");
        }
        _repository.Insert(mantenimiento);
        _repository.Save();
    }
    public Mantenimiento GetMantenimientoByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower());
        if (usuario is Mantenimiento mantenimiento)
        {
            return mantenimiento;
        }
        else
        {
            throw new ResourceNotFoundException("No Mantenimiento found with the specified email.");
        }
    }
    public void AtenderSolicitud(Solicitud solicitud)
    {
        if (solicitud.Estado == EstadoSolicitud.Abierto)
        {
            solicitud.Estado = EstadoSolicitud.Atendiendo;
            solicitud.FechaInicio = DateTime.Now;
            _solicitudService.EditarSolicitud(solicitud);
        }
        else
        {
            throw new Exception("La solicitud ya fue atendida");
        }
        
    }
    public void CompletarSolicitud(Solicitud solicitud)
    {
        if (solicitud.Estado == EstadoSolicitud.Atendiendo)
        {
            solicitud.Estado = EstadoSolicitud.Cerrado;
            solicitud.FechaFin = DateTime.Now;
            _solicitudService.EditarSolicitud(solicitud);

        }
        else
        {
            throw new Exception("La solicitud no esta en estado de atendiendo");
        }
    }
}