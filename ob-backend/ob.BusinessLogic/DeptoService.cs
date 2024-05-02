using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
namespace ob.BusinessLogic;
public class DeptoService : IDeptoService
{
    private readonly IGenericRepository<Depto> _repository;
    private readonly IDuenoService _duenoService;
    private readonly IGenericRepository<Edificio> _edificioRepository;

    public DeptoService(IGenericRepository<Depto> deptoRepository, IDuenoService duenoService, IGenericRepository<Edificio> edificioRepository)
    {
        _repository = deptoRepository;
        _duenoService = duenoService;
        _edificioRepository = edificioRepository;
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
        var edificio = _edificioRepository.Get(e => e.Nombre.ToLower() == depto.EdificioNombre.ToLower() && e.Direccion.ToLower() == depto.EdificioDireccion.ToLower(), new List<string> { "EmpresaConstructora" });
        depto.Edificio = edificio;
        _repository.Insert(depto);
       
        if(!depto.Edificio.Deptos.Contains(depto))
        {
            edificio.Deptos.Add(depto);
            _edificioRepository.Update(edificio);
        }
        _repository.Save();
    }
    public bool ExisteDepto(Depto depto)
    {
        return _repository.Get(d => d.Numero == depto.Numero && d.Edificio.Nombre == depto.Edificio.Nombre && d.Edificio.Direccion == depto.Edificio.Direccion) != null;
    }
    public void EditarDepto(Depto depto)
    {
        _repository.Update(depto);
        if (depto.Dueno != null)
        {
            if (!_duenoService.DuenoExists(depto.Dueno.Email))
            {
                _duenoService.CrearDueno(depto.Dueno);
            }
        }
        _repository.Save();
    }
    public void BorrarDepto(Depto depto)
    {
        _repository.Delete(depto);
        _repository.Save();
    }
    public Depto GetDepto(int numero, string edificioNombre, string edificioDireccion)
    {
        return _repository.Get(d => d.Numero == numero && d.Edificio.Nombre == edificioNombre && d.Edificio.Direccion == edificioDireccion, new List<string> { "Dueno", "Edificio"});
    }
}
