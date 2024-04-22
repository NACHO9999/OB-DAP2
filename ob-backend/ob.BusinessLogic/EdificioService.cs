using ob.Domain;
using ob.IDataAccess;
using ob.IBusinessLogic;
namespace ob.BusinessLogic;
public class EdificioService : IEdificioService
{
    private readonly IGenericRepository<Edificio> _repository;
    public EdificioService(IGenericRepository<Edificio> edificioRepository)
    {
        _repository = edificioRepository;
    }
    public void CrearEdificio(Edificio edificio)
    {
        if (ExisteEdificio(edificio.Nombre, edificio.Direccion))
        {
            throw new Exception("El edificio ya existe");
        }
        _repository.Insert(edificio);
        _repository.Save();
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
        return _repository.Get(e => e.Nombre == nombre, new List<string> { "Departamentos" });
    }
    public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion)
    {
        return _repository.Get(e => e.Nombre == nombre && e.Direccion == direccion, new List<string> { "Departamentos" });
    }
    public bool ExisteEdificio(string nombre, string direccion)
    {
        return _repository.Get(e => e.Nombre == nombre && e.Direccion == direccion) != null;
    }
}