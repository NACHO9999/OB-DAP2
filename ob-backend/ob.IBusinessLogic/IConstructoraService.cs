using ob.Domain;
namespace ob.IBusinessLogic;
public interface IConstructoraService
{
    void CrearConstructora(string nombre);
    Constructora GetConstructoraByNombre(string nombre);
    IEnumerable<Constructora> GetAllConstructoras();
    public void EditarConstructora(Constructora constructora);

    public bool ConstructoraExists(string nombre);


}