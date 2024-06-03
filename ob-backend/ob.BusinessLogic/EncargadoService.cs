using ob.Domain;
using ob.IBusinessLogic;
using Enums;
namespace ob.BusinessLogic;

using ob.Exceptions.BusinessLogicExceptions;
using ob.IDataAccess;

public class EncargadoService : IEncargadoService
{
    private readonly IUsuarioRepository _repository;
    private readonly IMantenimientoService _mantenimientoService;
    private readonly ISolicitudService _solicitudService;
    private readonly IEdificioService _edificioService;
    private readonly IDeptoService _deptoService;
    private readonly IDuenoService _duenoService;
    public EncargadoService(IUsuarioRepository repository, IMantenimientoService mantenimientoService, ISolicitudService solicitudService, IEdificioService edificioService, IDeptoService deptoService, IDuenoService duenoService)
    {
        _repository = repository;
        _mantenimientoService = mantenimientoService;
        _solicitudService = solicitudService;
        _edificioService = edificioService;
        _deptoService = deptoService;
        _duenoService = duenoService;
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
            throw new KeyNotFoundException("No Encargado found with the specified email.");
        }
    }
    public IEnumerable<Encargado> GetAllEncargados()
    {
        return _repository.GetAll<Encargado>(encargado => true, new List<string> { "Edificios.Deptos" });
    }


    public void CrearMantenimiento(Mantenimiento mantenimiento)
    {
        _mantenimientoService.CrearMantenimiento(mantenimiento);
    }
    public void CrearSolicitud(Solicitud solicitud, string email)
    {
        var encargado = GetEncargadoByEmail(email);
        if (!encargado.Edificios.Any(edificio => edificio.Deptos.Contains(solicitud.Depto)))
        {
            throw new InvalidOperationException("The Encargado is not in charge of the building of the request.");
        }
        _solicitudService.CrearSolicitud(solicitud);
    }
    public void AsignarSolicitud(Guid solicitudId, string email, string emailEncargado)
    {

        Encargado encargado = GetEncargadoByEmail(emailEncargado);
        Solicitud solicitud = _solicitudService.GetSolicitudById(solicitudId);
        Mantenimiento perMan = _mantenimientoService.GetMantenimientoByEmail(email);

        if (solicitud == null || perMan == null || encargado == null)
        {
            throw new KeyNotFoundException("Solicitud or Mantenimiento not found.");
        }
        if (!encargado.Edificios.Any(edificio => edificio.Deptos.Contains(solicitud.Depto)))
        {
            throw new InvalidOperationException("The Encargado is not in charge of the building of the request.");
        }
        solicitud.PerMan = perMan;
    }




    public int[] GetSolicitudByEdificio(string nombre, string direccion, string email)
    {
        Encargado encargado = GetEncargadoByEmail(email);
        Edificio edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
        if (!encargado.Edificios.Contains(edificio))
        {
            throw new InvalidOperationException("The Encargado is not in charge of the building.");
        }
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
    public int[] GetSolicitudByMantenimiento(string email, string emailEncargado)
    {
        Mantenimiento mantenimiento = _mantenimientoService.GetMantenimientoByEmail(email);
        Encargado encargado = GetEncargadoByEmail(emailEncargado);
        int[] retorno = new int[3];
        var lista = _solicitudService.GetSolicitudesByMantenimiento(mantenimiento);
        foreach (var solicitud in lista)
        {
            if (encargado.Edificios.Any(edificio => edificio.Deptos.Contains(solicitud.Depto)))
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

        }
        return retorno;
    }
    public TimeSpan? TiempoPromedioAtencion(string email)
    {
        Mantenimiento mantenimiento = _mantenimientoService.GetMantenimientoByEmail(email);
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

    public void AsignarDueno(int numero, string edNombre, string edDireccion, string emailDueno, string email)
    {
        var depto = _deptoService.GetDepto(numero, edNombre, edDireccion);
        var dueno = _duenoService.GetDuenoByEmail(emailDueno);
        var encargado = GetEncargadoByEmail(email);
        if (!encargado.Edificios.Any(edificio => edificio.Deptos.Contains(depto)))
        {
            throw new InvalidOperationException("El encargado no esta a cargo del edificio");
        }
        depto.Dueno = dueno;
        _deptoService.EditarDepto(depto);
    }
    public Dueno GetDueno(string email)
    {
        return _duenoService.GetDuenoByEmail(email);
    }


}
