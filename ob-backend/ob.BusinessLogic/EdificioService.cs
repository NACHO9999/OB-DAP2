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
        if (EdificioExists(edificio.Nombre, edificio.Direccion))
        {
            throw new Exception("El edificio ya existe");
        }
        
        foreach (var depto in edificio.Deptos)
        {
            _deptoService.CrearDepto(depto);
        }
        _repository.Insert(edificio);
        _repository.Save();
    }
    public void CrearEdificioConDatos(string nombre, string direccion, string ubicacion, string constructora, decimal gastos, List<Depto> deptos)
    {
        Constructora empresaConstructora = _constructoraService.GetConstructoraByNombre(constructora);

        if (empresaConstructora == null)
        {
            // La empresa constructora no existe, crear una nueva
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
        _repository.Save();
    }
    public void BorrarEdificio(Edificio edificio)
    {
        _repository.Delete(edificio);
        _repository.Save();
    }

    public Edificio GetEdificioByNombre(string nombre)
    {
        return _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower(), new List<string> { "Deptos" });
    }
    public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion)
    {
        return _repository.Get(e => e.Nombre.ToLower() == nombre.ToLower() && e.Direccion.ToLower() == direccion.ToLower(), new List<string> { "Deptos" });
    }
    public bool EdificioExists(string nombre, string direccion)
    {
        return GetEdificioByNombreYDireccion(nombre, direccion) != null;
    }
}