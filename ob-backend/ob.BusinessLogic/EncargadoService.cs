using ob.Domain;
using ob.IBusinessLogic;

namespace ob.BusinessLogic;

using ob.IDataAccess;

public class EncargadoService : IEncargadoService
{
    private readonly IUsuarioRepository _repository;
    private readonly IEdificioService _edificioService;
    private readonly IMantenimientoService _mantenimientoService;
    public EncargadoService(IUsuarioRepository repository, IEdificioService edificioService, IMantenimientoService mantenimientoService)
    {
        _repository = repository;
        _edificioService = edificioService;
        _mantenimientoService = mantenimientoService;
    }

    public void CrearEncargado(Encargado encargado)
    {
        if (_repository.EmailExists(encargado.Email))
        {
            throw new Exception("El email ya existe");
        }
        _repository.Insert(encargado);
        _repository.Save();
    }


    public Encargado GetEncargadoByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email == email, new List<string> { "Edificios" });
        if (usuario is Encargado encargado)
        {
            return encargado;
        }
        else
        {
            throw new Exception("No Encargado found with the specified email.");
        }
    }
    public IEnumerable<Encargado> GetAllEncargados()
    {
        return _repository.GetAll<Encargado>(encargado => true, new List<string> { "Edificios" });
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

        var edificio = encargado.Edificios.FirstOrDefault(e => e.Nombre == nombre);
        if (edificio == null)
        {
            throw new Exception("No Edificio found with the specified name.");
        }
        return edificio;
    }
    public void CrearMantenimiento(Mantenimiento mantenimiento) 
    {
        _mantenimientoService.CrearMantenimiento(mantenimiento);
        _repository.Save();
    }
    public void CrearSolicitud(Solicitud solicitud) { }
    public void AsignarSolicitud(Solicitud solicitud, Mantenimiento mantenimiento) { }
    public Solicitud GetSolicitudByCategoria(string? categoria)
    {
        return null;
    }
    public Solicitud GetSolicitudByEdificio(string? edificio)
    {
        return null;
    }
    public Solicitud GetSolicitudByMantenimiento(string? mantenimiento)
    {
        return null;
    }
}
