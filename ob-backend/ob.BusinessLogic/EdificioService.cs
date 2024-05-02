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


    public void CrearEdificio(Edificio edificio)
    {
     
        if(_constructoraService.GetConstructoraByNombre(edificio.EmpresaConstructora.Nombre) == null)
        {
            _constructoraService.CrearConstructora(edificio.EmpresaConstructora);
        }
        else
        {
            edificio.EmpresaConstructora = _constructoraService.GetConstructoraByNombre(edificio.EmpresaConstructora.Nombre);
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
            
            empresaConstructora = new Constructora(constructora);
            _constructoraService.CrearConstructora(empresaConstructora);
        }

        // Crear el edificio y asociarlo con la empresa constructora
        Edificio edificio = new Edificio(nombre, direccion, ubicacion, empresaConstructora, gastos, deptos);
        CrearEdificio(edificio);
    }
    public void EditarEdificio(Edificio edificio)
    {
        _repository.Update(edificio);
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
        _repository.Save();
    }
    public void BorrarEdificio(Edificio edificio)
    {
        _repository.Delete(edificio);
        _repository.Save();
    }

    public Edificio GetEdificioByNombre(string nombre)
    {
        var retorno = _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower(), new List<string> { "Deptos" });
        if (retorno == null)
        {
            throw new KeyNotFoundException("No se encontró el edificio.");
        }
        return retorno;
    }
    public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion)
    {
        if (EdificioExists(nombre, direccion))
        {
            return _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower() && e.Direccion.ToLower() == direccion.ToLower(), new List<string> { "Deptos" });
        }
        else
        {
            throw new KeyNotFoundException("No se encontró el edificio.");
        }
    }
    public bool EdificioExists(string nombre, string direccion)
    {
        return _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower() && e.Direccion.ToLower() == direccion.ToLower()) != null;
    }
}