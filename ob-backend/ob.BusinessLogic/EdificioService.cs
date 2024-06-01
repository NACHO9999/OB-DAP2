using ob.Domain;
using ob.IDataAccess;
using ob.IBusinessLogic;
namespace ob.BusinessLogic;
public class EdificioService : IEdificioService
{
    private readonly IGenericRepository<Edificio> _repository;
    private readonly IConstructoraService _constructoraService;
    private readonly IDeptoService _deptoService;
    public EdificioService(IGenericRepository<Edificio> edificioRepository, IConstructoraService constructoraService, IDeptoService deptoService)
    {
        _repository = edificioRepository;
        _constructoraService = constructoraService;
        _deptoService = deptoService;
    }


    public List<Edificio> GetAllEdificios()
    {
        return _repository.GetAll<Edificio>(e => true, new List<string> { "Deptos" }).ToList();
    }

    public void CrearEdificio(Edificio edificio)
    {

        if (_constructoraService.GetConstructoraByNombre(edificio.EmpresaConstructora.Nombre) == null)
        {
            throw new KeyNotFoundException("No se encontró la empresa constructora.");
        }

        if (EdificioExists(edificio.Nombre, edificio.Direccion))
        {
            throw new Exception("El edificio ya existe");
        }
        _repository.Insert(edificio);
        _repository.Save();

        foreach (var depto in edificio.Deptos)
        {
            var existingDepto = _deptoService.GetDepto(depto.Numero, edificio.Nombre, edificio.Direccion);
            if (existingDepto == null)
            {
                _deptoService.CrearDepto(depto);
            }
            else
            {
                existingDepto.EdificioDireccion = edificio.Direccion;
                existingDepto.EdificioNombre = edificio.Nombre;
                _deptoService.EditarDepto(existingDepto);
            }
        }

    }
    public void CrearEdificioConDatos(string nombre, string direccion, string ubicacion, string constructora, decimal gastos, List<Depto> deptos)
    {
        Constructora empresaConstructora = _constructoraService.GetConstructoraByNombre(constructora);

        if (empresaConstructora == null)
        {

            throw new KeyNotFoundException("No se encontró la empresa constructora.");
        }

        // Crear el edificio y asociarlo con la empresa constructora
        Edificio edificio = new Edificio(nombre, direccion, ubicacion, empresaConstructora, gastos, deptos);
        CrearEdificio(edificio);
    }
    public void EditarEdificio(Edificio edificio)
    {
        var constructora = _constructoraService.GetConstructoraByNombre(edificio.EmpresaConstructora.Nombre);
        var edificioExistente = _repository.Get(e => e.Nombre.ToLower() == edificio.Nombre.ToLower() && e.Direccion.ToLower() == edificio.Direccion.ToLower(), new List<string> { "Deptos" });
        if (edificioExistente == null)
        {
            throw new KeyNotFoundException("No se encontró el edificio.");
        }
        if (constructora == null)
        {
            throw new KeyNotFoundException("No se encontró la empresa constructora.");
        }
        if (edificio.Deptos.Count == 0 && edificioExistente.Deptos.Count != 0)
        {
            edificio.Deptos = edificioExistente.Deptos;
        }
        _repository.Update(edificio);
        _repository.Save();
    }
    public void BorrarEdificio(Edificio edificio)
    {
        _repository.Delete(edificio);
        _repository.Save();
    }

    public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion)
    {
        if (EdificioExists(nombre, direccion))
        {
            var edificio = _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower() && e.Direccion.ToLower() == direccion.ToLower(), new List<string> { "Deptos" });
            return edificio;
        }
        else
        {
            throw new KeyNotFoundException("No se encontró el edificio.");
        }

    }
    public bool EdificioExists(string nombre, string direccion)
    {
        var edificio = _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower() && e.Direccion.ToLower() == direccion.ToLower(), new List<string> { "Deptos" });
        return edificio != null;
    }

    public void AgregarDepto(Edificio edificio, Depto depto)
    {
        if (edificio.Deptos == null)
        {
            edificio.Deptos = new List<Depto>();
        }
        edificio.Deptos.Add(depto);
        depto.EdificioNombre = edificio.Nombre;
        depto.EdificioDireccion = edificio.Direccion;
        _deptoService.CrearDepto(depto);
        EditarEdificio(edificio);
    }
}