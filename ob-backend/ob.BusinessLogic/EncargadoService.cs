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

public List<Solicitud> GetAllEncargadoSolicitudes(string email)
{
    Encargado encargado = GetEncargadoByEmail(email);
    if (encargado == null)
    {
        throw new InvalidOperationException("Encargado not found.");
    }

    var encargadoDeptos = encargado.Edificios.SelectMany(e => e.Deptos).ToList();

    return _solicitudService.GetSolicitudes()
        .Where(s => encargadoDeptos.Any(d => 
            d.Numero == s.Depto.Numero && 
            d.EdificioDireccion == s.Depto.EdificioDireccion && 
            d.EdificioNombre == s.Depto.EdificioNombre))
        .ToList();
}
    public Encargado GetEncargadoByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower(), new List<string> { "Edificios.Deptos.Dueno", "Edificios.EmpresaConstructora" });
        if (usuario is Encargado encargado)
        {
            return encargado;
        }
        else
        {
            throw new KeyNotFoundException("No Encargado found with the specified email.");
        }
    }

    public List<Solicitud> GetSolicitudesSinMantenimiento(string email)
    {
        var encargado = GetEncargadoByEmail(email);
        var lista = _solicitudService.GetSolicitudes();
        lista = lista.Where(s => s.PerMan == null).ToList();
        var retorno = new List<Solicitud>();

        foreach (var edificio in encargado.Edificios)
        {
            foreach (var solicitud in lista)
            {
                if (solicitud.Depto.EdificioNombre == edificio.Nombre &&
                    solicitud.Depto.EdificioDireccion == edificio.Direccion)
                {
                    retorno.Add(solicitud);
                }
            }
        }

        return retorno;
    }

    public IEnumerable<Encargado> GetAllEncargados()
    {
        return _repository.GetAll<Encargado>(encargado => true, new List<string> { "Edificios.Deptos.Dueno", "Edificios.EmpresaConstructora", "Edificios" });
    }


    public void CrearMantenimiento(Mantenimiento mantenimiento)
    {
        _mantenimientoService.CrearMantenimiento(mantenimiento);
    }
    public void CrearSolicitud(Solicitud solicitud, string email)
    {
        var encargado = GetEncargadoByEmail(email);

        bool isEncargadoInCharge = encargado.Edificios
            .SelectMany(e => e.Deptos)
            .Any(d => d.Numero == solicitud.Depto.Numero
                   && d.EdificioDireccion == solicitud.Depto.EdificioDireccion
                   && d.EdificioNombre == solicitud.Depto.EdificioNombre);

        if (isEncargadoInCharge)
        {
            _solicitudService.CrearSolicitud(solicitud);
        }
        else
        {
            throw new InvalidOperationException("The Encargado is not in charge of the building of the request.");
        }
    }
    public void AsignarSolicitud(Guid solicitudId, string email, string emailEncargado)
    {
        var encargado = GetEncargadoByEmail(emailEncargado);
        var solicitud = _solicitudService.GetSolicitudById(solicitudId);
        var perMan = _mantenimientoService.GetMantenimientoByEmail(email);

        if (solicitud == null)
        {
            throw new KeyNotFoundException("Solicitud not found.");
        }

        if (perMan == null)
        {
            throw new KeyNotFoundException("Mantenimiento not found.");
        }

        if (encargado == null)
        {
            throw new KeyNotFoundException("Encargado not found.");
        }

        bool isEncargadoInCharge = encargado.Edificios
            .SelectMany(e => e.Deptos)
            .Any(d => d.Numero == solicitud.Depto.Numero
                   && d.EdificioDireccion == solicitud.Depto.EdificioDireccion
                   && d.EdificioNombre == solicitud.Depto.EdificioNombre);

        if (!isEncargadoInCharge)
        {
            throw new InvalidOperationException("The Encargado is not in charge of the building of the request.");
        }

        solicitud.PerMan = perMan;
        _solicitudService.EditarSolicitud(solicitud);  // Assuming you need to save changes
    }



    public int[] GetSolicitudByEdificio(string nombre, string direccion, string email)
    {
        Encargado encargado = GetEncargadoByEmail(email);
        if (encargado == null)
        {
            throw new KeyNotFoundException("Encargado no encontrado.");
        }

        Edificio edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
        if (edificio == null)
        {
            throw new KeyNotFoundException("Edificio no encontrado.");
        }

        if (!encargado.Edificios.Any(e => e.Nombre == edificio.Nombre && e.Direccion == edificio.Direccion))
        {
            throw new InvalidOperationException("El encargado no está a cargo del edificio.");
        }

        int[] array = new int[3];
        var lista = _solicitudService.GetSolicitudesByEdificio(edificio);

        foreach (var solicitud in lista)
        {
            switch (solicitud.Estado)
            {
                case EstadoSolicitud.Abierto:
                    array[0]++;
                    break;
                case EstadoSolicitud.Atendiendo:
                    array[1]++;
                    break;
                case EstadoSolicitud.Cerrado:
                    array[2]++;
                    break;
            }
        }

        return array;
    }

    public int[] GetSolicitudByMantenimiento(string email, string emailEncargado)
    {
        Mantenimiento mantenimiento = _mantenimientoService.GetMantenimientoByEmail(email);
        if (mantenimiento == null)
        {
            throw new KeyNotFoundException("Mantenimiento no encontrado.");
        }

        Encargado encargado = GetEncargadoByEmail(emailEncargado);
        if (encargado == null)
        {
            throw new KeyNotFoundException("Encargado no encontrado.");
        }

        int[] retorno = new int[3];
        var lista = _solicitudService.GetSolicitudesByMantenimiento(mantenimiento);

        foreach (var solicitud in lista)
        {
            if (encargado.Edificios.SelectMany(edificio => edificio.Deptos)
                                   .Any(depto => depto.Numero == solicitud.Depto.Numero &&
                                                 depto.EdificioDireccion == solicitud.Depto.EdificioDireccion &&
                                                 depto.EdificioNombre == solicitud.Depto.EdificioNombre))
            {
                switch (solicitud.Estado)
                {
                    case EstadoSolicitud.Abierto:
                        retorno[0]++;
                        break;
                    case EstadoSolicitud.Atendiendo:
                        retorno[1]++;
                        break;
                    case EstadoSolicitud.Cerrado:
                        retorno[2]++;
                        break;
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

    public void AsignarDueno(int numero, string edNombre, string edDireccion, Dueno dueno, string email)
    {
        var depto = _deptoService.GetDepto(numero, edNombre, edDireccion);
        if (depto == null)
        {
            throw new KeyNotFoundException("Departamento no encontrado.");
        }


        try
        {
            _duenoService.GetDuenoByEmail(dueno.Email);
        }
        catch (Exception)
        {
            _duenoService.CrearDueno(dueno);
        }
        var encargado = GetEncargadoByEmail(email);
        if (encargado == null)
        {
            throw new KeyNotFoundException("Encargado no encontrado.");
        }

        bool isEncargadoInCharge = encargado.Edificios
            .SelectMany(e => e.Deptos)
            .Any(d => d.Numero == depto.Numero
                   && d.EdificioDireccion == depto.EdificioDireccion
                   && d.EdificioNombre == depto.EdificioNombre);

        if (!isEncargadoInCharge)
        {
            throw new InvalidOperationException("El encargado no está a cargo del edificio.");
        }

        depto.Dueno = _duenoService.GetDuenoByEmail(dueno.Email);
        _deptoService.EditarDepto(depto);
    }

    public void DesasignarDueno(int numero, string edNombre, string edDireccion, string email)
    {
        var depto = _deptoService.GetDepto(numero, edNombre, edDireccion);
        if (depto == null)
        {
            throw new KeyNotFoundException("Departamento no encontrado.");
        }

        var encargado = GetEncargadoByEmail(email);
        if (encargado == null)
        {
            throw new KeyNotFoundException("Encargado no encontrado.");
        }

        bool isEncargadoInCharge = encargado.Edificios
            .SelectMany(e => e.Deptos)
            .Any(d => d.Numero == depto.Numero
                   && d.EdificioDireccion == depto.EdificioDireccion
                   && d.EdificioNombre == depto.EdificioNombre);

        if (!isEncargadoInCharge)
        {
            throw new InvalidOperationException("El encargado no está a cargo del edificio.");
        }

        depto.Dueno = null;
        _deptoService.EditarDepto(depto);
    }
    public Dueno GetDueno(string email)
    {
        return _duenoService.GetDuenoByEmail(email);
    }

    public void CrearDueno(Dueno dueno)
    {
        _duenoService.CrearDueno(dueno);
    }

    public List<Mantenimiento> GetAllMantenimiento()
    {
        return _mantenimientoService.GetAllMantenimiento();
    }
}
