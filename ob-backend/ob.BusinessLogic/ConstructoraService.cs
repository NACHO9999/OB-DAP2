using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
namespace ob.BusinessLogic;
public class ConstructoraService : IConstructoraService
{
    private readonly IGenericRepository<Constructora> _repository;
    public ConstructoraService(IGenericRepository<Constructora> constructoraRepository)
    {
        _repository = constructoraRepository;
    }
    public void CrearConstructora(string nombre)
    {
        if (ConstructoraExists(nombre))
        {
            throw new AlreadyExistsException("La constructora ya existe");
        }
        var constructora = new Constructora(nombre);
        _repository.Insert(constructora);
        _repository.Save();
    }
    public Constructora GetConstructoraByNombre(string nombre)
    {

        var constructora = _repository.Get(c => c.Nombre == nombre);
        if (constructora == null)
        {
            throw new KeyNotFoundException("No hay constructora con este nombre");
        }
        return constructora;
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
    var existingConstructora = _repository.Get(c => c.Id == constructora.Id);
    if (existingConstructora == null)
    {
        throw new KeyNotFoundException("Constructora no encontrada");
    }

    // Update properties of the existing constructora with new values
    existingConstructora.Nombre = constructora.Nombre;
    // Add other properties that need to be updated

    // Save the changes to the repository
    _repository.Update(existingConstructora);
    _repository.Save();
}
}