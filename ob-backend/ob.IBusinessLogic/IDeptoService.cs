using ob.Domain;
namespace ob.IBusinessLogic;
public interface IDeptoService
{
    void CrearDepto(Depto depto);
    bool ExisteDepto(Depto depto);
    void BorrarDepto(Depto depto);
    void EditarDepto(Depto depto);
    Depto GetDepto(int numero, string edificioNombre, string edificioDireccion);
    IEnumerable<Depto> GetDeptosPorEdificio(Edificio edificio);

}