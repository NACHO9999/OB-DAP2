using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;

namespace ob.BusinessLogic;

public class AdminService : IAdminService
{
    private readonly IGenericRepository<Usuario> _repository;
    public AdminService(IGenericRepository<Usuario> repository)
    {
        _repository = repository;
    }
    public IEnumerable<Administrador> GetAllEncargados()
    {
        return _repository.GetAll<Administrador>();
    }
    public void CrearAdmin(Administrador admin)
    {
        if (EmailExists(admin.Email))
        {
            throw new Exception("El email ya existe");
        }
        _repository.Insert(admin);
    }
    public Administrador GetAdminByEmail(string email)
    {
        var usuario = _repository.Get(u => u.Email == email);
            if (usuario is Administrador administrador)
            {
                return administrador;
            }
            else
            {
                throw new Exception("No Administrador found with the specified email.");
            }
    }
    public void InvitarEncargado(string email, string nombre, DateTime fecha) { }
    public void EliminarInvitacion(string email) { }
    public void AltaCategoria(Categoria categoria) { }
    private bool EmailExists(string email)
    {
        return _repository.Get(u => u.Email == email) != null;
    }
}
