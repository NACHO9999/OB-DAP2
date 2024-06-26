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

    public List<Solicitud> GetSolicitudesParaAtender(string email)
    {
        var mantenimiento = GetMantenimientoByEmail(email);
        var lista = _solicitudService.GetSolicitudes().Where(s => s.PerMan != null).ToList();
        return lista.Where(s => s.Estado == EstadoSolicitud.Abierto).ToList();
    }
    public List<Solicitud> GetSolicitudesAtendiendo(string email)
    {
        var mantenimiento = GetMantenimientoByEmail(email);
        var lista = _solicitudService.GetSolicitudes().Where(s => s.PerMan!=null).ToList();
        return lista.Where(s => s.Estado == EstadoSolicitud.Atendiendo && s.PerMan.Email == mantenimiento.Email).ToList();
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
            throw new KeyNotFoundException("No Mantenimiento found with the specified email.");
        }
    }
    public void AtenderSolicitud(Guid solicitudId, string email)
    {
        var mantenimiento = GetMantenimientoByEmail(email);
        var solicitud = _solicitudService.GetSolicitudById(solicitudId);
        if (solicitud.PerMan != mantenimiento)
        {
            throw new InvalidOperationException("El mantenimiento no puede atender la solicitud");
        }
        if (solicitud.Estado == EstadoSolicitud.Abierto)
        {
            solicitud.Estado = EstadoSolicitud.Atendiendo;
            solicitud.FechaInicio = DateTime.Now;
            _solicitudService.EditarSolicitud(solicitud);
        }
        else
        {
            throw new InvalidOperationException("La solicitud ya fue atendida");
        }

    }
    public void CompletarSolicitud(Guid solicitudId, string email)
    {
        var mantenimiento = GetMantenimientoByEmail(email);
        var solicitud = _solicitudService.GetSolicitudById(solicitudId);
        if (solicitud.PerMan == null)
        {
            throw new InvalidOperationException("La solicitud no tiene un mantenimiento asignado");
        }
        if (solicitud.PerMan.Email != mantenimiento.Email)
        {
            throw new InvalidOperationException("El mantenimiento no puede atender la solicitud");
        }
        if (solicitud.Estado == EstadoSolicitud.Atendiendo)
        {
            solicitud.Estado = EstadoSolicitud.Cerrado;
            solicitud.FechaFin = DateTime.Now;
            _solicitudService.EditarSolicitud(solicitud);

        }
        else
        {
            throw new InvalidOperationException("La solicitud no esta en estado de atendiendo");
        }
    }
    public List<Mantenimiento> GetAllMantenimiento()
    {
        return _repository.GetAll<Mantenimiento>().ToList();
    }
}