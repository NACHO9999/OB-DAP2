using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
namespace ob.BusinessLogic;
public class ConstructoraService: IConstructoraService
{
    private readonly IGenericRepository<Constructora> _repository;
    public ConstructoraService(IGenericRepository<Constructora> constructoraRepository)
    {
        _repository = constructoraRepository;
    }
    public void CrearConstructora(Constructora constructora)
    {
        if (ConstructoraExists(constructora.Nombre))
        {
            throw new AlreadyExistsException("La constructora ya existe");
        }
        _repository.Insert(constructora);
        _repository.Save();
    }
    public Constructora GetConstructoraByNombre(string nombre)
    {
       
        return _repository.Get(c => c.Nombre == nombre);
        
       
    }
    public IEnumerable<Constructora> GetAllConstructoras()
    {
        return _repository.GetAll<Constructora>();
    }
    public bool ConstructoraExists(string nombre)
    {
        return _repository.Get(c => c.Nombre.ToLower() == nombre.ToLower()) != null;
    }
    public void EditarConstructora(Constructora constructora)
    {
        _repository.Update(constructora);
        _repository.Save();
    }
}