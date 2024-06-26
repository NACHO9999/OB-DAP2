using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
namespace ob.BusinessLogic;
public class DeptoService : IDeptoService
{
    private readonly IGenericRepository<Depto> _repository;
    private readonly IDuenoService _duenoService;


    public DeptoService(IGenericRepository<Depto> deptoRepository, IDuenoService duenoService)
    {
        _repository = deptoRepository;
        _duenoService = duenoService;
    }
    public void CrearDepto(Depto depto)
    {
        if (ExisteDepto(depto))
        {
            throw new AlreadyExistsException("El departamento ya existe");
        }
        if (depto.Dueno != null)
        {
            if (!_duenoService.DuenoExists(depto.Dueno.Email))
            {
                _duenoService.CrearDueno(depto.Dueno);
            }
            else
            {
                depto.Dueno = _duenoService.GetDuenoByEmail(depto.Dueno.Email);
            }
        }

        _repository.Insert(depto);


        _repository.Save();
    }
    public IEnumerable<Depto> GetDeptosPorEdificio(Edificio edificio)
    {
        return _repository.GetAll<Depto>(d => d.EdificioNombre == edificio.Nombre && d.EdificioDireccion == edificio.Direccion);
    }
    public bool ExisteDepto(Depto depto)
    {
        return _repository.Get(d => d.Numero == depto.Numero && d.EdificioNombre == depto.EdificioNombre && d.EdificioDireccion == depto.EdificioDireccion) != null;
    }
public void EditarDepto(Depto depto)
{
    var existingDepto = _repository.Get(d => d.Numero == depto.Numero && d.EdificioNombre == depto.EdificioNombre && d.EdificioDireccion == depto.EdificioDireccion);
    if (existingDepto == null)
    {
        throw new KeyNotFoundException("Departamento no encontrado");
    }

    if (depto.Dueno != null)
    {
        if (!_duenoService.DuenoExists(depto.Dueno.Email))
        {
            _duenoService.CrearDueno(depto.Dueno);
        }
        else
        {
            // Ensure the Dueno entity is tracked
            depto.Dueno = _duenoService.GetDuenoByEmail(depto.Dueno.Email);
        }
    }

    existingDepto.CantidadBanos = depto.CantidadBanos;
    existingDepto.CantidadCuartos = depto.CantidadCuartos;
    existingDepto.ConTerraza = depto.ConTerraza;
    existingDepto.Piso = depto.Piso;
    existingDepto.Dueno = depto.Dueno; // Assign updated or existing Dueno

    _repository.Update(existingDepto);
    _repository.Save();
}

    public void BorrarDepto(Depto depto)
    {
        _repository.Delete(depto);
        _repository.Save();
    }
    public Depto GetDepto(int numero, string edificioNombre, string edificioDireccion)
    {
        Depto depto = _repository.Get(d => d.Numero == numero && d.EdificioNombre == edificioNombre && d.EdificioDireccion == edificioDireccion, new List<string> { "Dueno" });
        if (depto == null)
        {
            throw new KeyNotFoundException("Departamento no encontrado");
        }
        return depto;
    }
}
