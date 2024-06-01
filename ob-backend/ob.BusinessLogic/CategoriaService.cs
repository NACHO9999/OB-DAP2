using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;


namespace ob.BusinessLogic;

public class CategoriaService : ICategoriaService
{
    private readonly IGenericRepository<Categoria> _repository;

    public CategoriaService(IGenericRepository<Categoria> repository)
    {
        _repository = repository;
    }
    public Categoria GetCategoriaByNombre(string nombre)
    {

        var categoria = _repository.Get(c => c.Nombre.ToLower() == nombre.ToLower());
        if (categoria == null)
        {
            throw new KeyNotFoundException("No hay categoria con este nombre");
        }
        return categoria;

    }
    public void CrearCategoria(Categoria categoria)
    {
        if (CategoriaExists(categoria.Nombre))
        {
            throw new AlreadyExistsException("La categoria ya existe");
        }
        _repository.Insert(categoria);
        _repository.Save();
    }
    private bool CategoriaExists(string nombre)
    {
        return _repository.Get(c => c.Nombre.ToLower() == nombre.ToLower()) != null;
    }
    public IEnumerable<Categoria> GetAllCategorias()
    {
        return _repository.GetAll<Categoria>();
    }

}