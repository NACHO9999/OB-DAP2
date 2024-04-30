using ob.Domain;
using ob.IBusinessLogic;
using Enums;
namespace ob.BusinessLogic;

using ob.Exceptions.BusinessLogicExceptions;
using ob.IDataAccess;

public class EncargadoService : IEncargadoService
{
    private readonly IUsuarioRepository _repository;
    private readonly IEdificioService _edificioService;
    private readonly IMantenimientoService _mantenimientoService;
    private readonly ISolicitudService _solicitudService;
    public EncargadoService(IUsuarioRepository repository, IEdificioService edificioService, IMantenimientoService mantenimientoService, ISolicitudService solicitudService)
    {
        _repository = repository;
        _edificioService = edificioService;
        _mantenimientoService = mantenimientoService;
        _solicitudService = solicitudService;
    }

    public void CrearEncargado(Encargado encargado)
    {
        if (_repository.EmailExists(encargado.Email))
        {
            throw new AlreadyExistsException("El email ya existe");
        }
        _repository.Insert(encargado);
        _repository.Save();
    }

    public Encargado GetEncargadoByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower(), new List<string> { "Edificios.Deptos" });
        if (usuario is Encargado encargado)
        {
            return encargado;
        }
        else
        {
            throw new ResourceNotFoundException("No Encargado found with the specified email.");
        }
    }
    public IEnumerable<Encargado> GetAllEncargados()
    {
        return _repository.GetAll<Encargado>(encargado => true, new List<string> { "Edificios.Deptos" });
    }
    public void CrearEdificio(Encargado encargado, Edificio edificio)
    {

        _edificioService.CrearEdificio(edificio);
        encargado.Edificios.Add(edificio);
        _repository.Save();
    }
    public void EditarEdificio(Edificio edificio)
    {
        _edificioService.EditarEdificio(edificio);
        _repository.Save();
    }
    public void BorrarEdificio(string nombre)
    {
        var edificio = _edificioService.GetEdificioByNombre(nombre);
        _edificioService.BorrarEdificio(edificio);
        _repository.Save();
    }
    public Edificio GetEdificioByNombre(Encargado encargado, string nombre)
    {

        var edificio = encargado.Edificios.FirstOrDefault(e => e.Nombre.ToLower() == nombre.ToLower());
        if (edificio == null)
        {
            throw new ResourceNotFoundException("No Edificio found with the specified name.");
        }
        return edificio;
    }
    public void CrearMantenimiento(Mantenimiento mantenimiento)
    {
        _mantenimientoService.CrearMantenimiento(mantenimiento);
    }
    public void CrearSolicitud(Solicitud solicitud)
    {
        _solicitudService.CrearSolicitud(solicitud);
    }
    public void AsignarSolicitud(Solicitud solicitud, Mantenimiento mantenimiento)
    {
        solicitud.PerMan = mantenimiento;
        _solicitudService.EditarSolicitud(solicitud);
    }

    public int[] GetSolicitudByEdificio(Edificio edificio)
    {
        int[] array = new int[3];
        var lista = _solicitudService.GetSolicitudesByEdificio(edificio);
        foreach (var solicitud in lista)
        {
            if (solicitud.Estado == EstadoSolicitud.Abierto)
            {
                array[0]++;
            }
            else if (solicitud.Estado == EstadoSolicitud.Atendiendo)
            {
                array[1]++;
            }
            else if (solicitud.Estado == EstadoSolicitud.Cerrado)
            {
                array[2]++;
            }
        }
        return array;
    }
    public int[] GetSolicitudByMantenimiento(Mantenimiento mantenimiento)
    {
        int[] retorno = new int[3];
        var lista = _solicitudService.GetSolicitudesByMantenimiento(mantenimiento);
        foreach (var solicitud in lista)
        {
            if (solicitud.Estado == EstadoSolicitud.Abierto)
            {
                retorno[0]++;
            }
            else if (solicitud.Estado == EstadoSolicitud.Atendiendo)
            {
                retorno[1]++;
            }
            else if (solicitud.Estado == EstadoSolicitud.Cerrado)
            {
                retorno[2]++;
            }

        }
        return retorno;
    }
    public TimeSpan? TiempoPromedioAtencion(Mantenimiento mantenimiento)
    {
        var lista = _solicitudService.GetSolicitudesByMantenimiento(mantenimiento);
        TimeSpan? tiempoTotal = null;
        int cantidad = 0;

        foreach (var solicitud in lista)
        {
            if (solicitud.Estado == EstadoSolicitud.Cerrado)
            {
                var tiempoAtencion = solicitud.FechaFin - solicitud.FechaInicio;
                tiempoTotal ??= TimeSpan.Zero; // Initialize tiempoTotal if null
                tiempoTotal += tiempoAtencion;
                cantidad++;
            }
        }

        if (cantidad == 0)
        {
            return null; // No closed requests, return null
        }

        return TimeSpan.FromTicks(tiempoTotal.Value.Ticks / cantidad); // Calculate average time
    }
}
