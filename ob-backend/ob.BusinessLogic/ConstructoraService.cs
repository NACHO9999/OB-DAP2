using ob.Domain;
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
        _repository.Insert(constructora);
        _repository.Save();
    }
    public Constructora GetConstructoraByNombre(string nombre)
    {
        return _repository.Get(c => c.Nombre.ToLower() == nombre.ToLower());
    }
}