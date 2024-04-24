using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
namespace ob.BusinessLogic;
public class DeptoService: IDeptoService
{
    private readonly IGenericRepository<Depto> _repository;
    public DeptoService(IGenericRepository<Depto> deptoRepository)
    {
        _repository = deptoRepository;
    }
    public void CrearDepto(Depto depto)
    {
        _repository.Insert(depto);
        _repository.Save();
    }
    public void BorrarDepto(Depto depto)
    {
        _repository.Delete(depto);
        _repository.Save();
    }
}
