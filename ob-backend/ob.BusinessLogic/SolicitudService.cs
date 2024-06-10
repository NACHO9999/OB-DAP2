using ob.Domain;
using Enums;
using ob.IDataAccess;
using ob.IBusinessLogic;
using ob.Exceptions.BusinessLogicExceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace ob.BusinessLogic;

public class SolicitudService : ISolicitudService
{
    private readonly IGenericRepository<Solicitud> _repository;
    private readonly IDeptoService _deptoService;
    private readonly ICategoriaService _categoriaService;
    private readonly IGenericRepository<Usuario> _usuarioRepository;
    public SolicitudService(IGenericRepository<Solicitud> solicitudRepository, IDeptoService deptoService, ICategoriaService categoriaService, IGenericRepository<Usuario> usuarioRepository)
    {
        _repository = solicitudRepository;
        _deptoService = deptoService;
        _categoriaService = categoriaService;
        _usuarioRepository = usuarioRepository;
    }
 
    
    public void CrearSolicitud(Solicitud solicitud)
    {
        var depto = _deptoService.GetDepto(solicitud.Depto.Numero, solicitud.Depto.EdificioNombre, solicitud.Depto.EdificioDireccion);
        solicitud.Depto = depto;
        var categoria = _categoriaService.GetCategoriaByNombre(solicitud.Categoria.Nombre);
        solicitud.Categoria = categoria;
        Mantenimiento? perMan = null;
        if (solicitud.PerMan != null)
        {
            perMan = (Mantenimiento)_usuarioRepository.Get(u => u.Email == solicitud.PerMan.Email);
            solicitud.PerMan = perMan;
        }

        _repository.Insert(solicitud);
        _repository.Save();
    }
    public void EditarSolicitud(Solicitud solicitud)
    {
                var depto = _deptoService.GetDepto(solicitud.Depto.Numero, solicitud.Depto.EdificioNombre, solicitud.Depto.EdificioDireccion);
        solicitud.Depto = depto;
        var categoria = _categoriaService.GetCategoriaByNombre(solicitud.Categoria.Nombre);
        solicitud.Categoria = categoria;
        Mantenimiento? perMan = null;
        if (solicitud.PerMan != null)
        {
            perMan = (Mantenimiento)_usuarioRepository.Get(u => u.Email == solicitud.PerMan.Email);
            solicitud.PerMan = perMan;
        }

        _repository.Update(solicitud);
        _repository.Save();
    }
    public Solicitud GetSolicitudById(Guid id)
    {
        if (SolicitudExists(id))
        {
            return _repository.Get(s => s.Id == id, new List<string> { "Depto", "PerMan", "Categoria" });
        }
        else
        {
            throw new KeyNotFoundException("No se encontr� la solicitud.");
        }
    }
    private bool SolicitudExists(Guid id)
    {
        return _repository.Get(s => s.Id == id) != null;
    }
    public List<Solicitud> GetSolicitudesByEdificio(Edificio edificio)
    {
        List<Solicitud> solicitudesInEdificio = new List<Solicitud>();

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "Depto","PerMan", "Categoria" }))
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

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "Categoria", "PerMan", "Depto"}))
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

        foreach (var solicitud in _repository.GetAll<Solicitud>(solicitud => true, new List<string> { "PerMan", "Depto", "Categoria"}))
        {
            if (solicitud.PerMan == mant)
            {
                solicitudesByMantenimiento.Add(solicitud);
            }
        }
        return solicitudesByMantenimiento;
    }
}