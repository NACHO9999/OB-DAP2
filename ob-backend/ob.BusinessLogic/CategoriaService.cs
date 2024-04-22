using ob.Domain;
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
        return _repository.Get(c => c.Nombre == nombre);
    }
    public void CrearCategoria(Categoria categoria)
    {
        if (CategoriaExists(categoria.Nombre))
        {
            throw new Exception("La categoria ya existe");
        }
        _repository.Insert(categoria);
        _repository.Save();
    }
    private bool CategoriaExists(string nombre)
    {
        return _repository.Get(c => c.Nombre == nombre) != null;
    }

}