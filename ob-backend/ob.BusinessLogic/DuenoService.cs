using ob.Domain;
using ob.IDataAccess;
using ob.IBusinessLogic;
using ob.Exceptions.BusinessLogicExceptions;
namespace ob.BusinessLogic;
public class DuenoService : IDuenoService
{
    private readonly IGenericRepository<Dueno> _repository;
    public DuenoService(IGenericRepository<Dueno> duenoRepository)
    {
        _repository = duenoRepository;
    }
    public void CrearDueno(Dueno dueno)
    {
        if (DuenoExists(dueno.Email))
        {
            throw new AlreadyExistsException("El dueño ya existe");
        }
        _repository.Insert(dueno);
        _repository.Save();
    }
    public void BorrarDueno(Dueno dueno)
    {
        _repository.Delete(dueno);
        _repository.Save();
    }
    public Dueno GetDuenoByEmail(string email)
    {

        if (!DuenoExists(email))
        {
            throw new KeyNotFoundException("No se encontró el dueño.");
        }
        return _repository.Get(d => d.Email.ToLower() == email.ToLower());


    }
    public bool DuenoExists(string email)
    {
        return _repository.Get(d => d.Email.ToLower() == email.ToLower()) != null;
    }

}