using ob.Domain;
using ob.IBusinessLogic;

namespace ob.BusinessLogic;

public class EncargadoService : IEncargadoService
{
    public void CrearEncargado(Encargado encargado) { }
    public void BorrarEncargado(string email) { }
    public void EditarEncargado(Encargado encargado) { }
    public Encargado GetEncargadoByEmail(string email)
    {
        return null;
    }
    public void CrearEdificio(Edificio edificio) { }
    public void EditarEdificio(Edificio edificio) { }
    public void BorrarEdificio(string nombre) { }
    public Edificio GetEdificioByNombre(string nombre)
    {
        return null;
    }
    public void CrearMantenimiento(Mantenimiento mantenimiento) { }
    public void CrearSolicitud(Solicitud solicitud) { }
    public void AsignarSolicitud(Solicitud solicitud, Mantenimiento mantenimiento) { }
    public Solicitud GetSolicitudByCategoria(string? categoria)
    {
        return null;
    }
    public Solicitud GetSolicitudByEdificio(string? edificio)
    {
        return null;
    }
    public Solicitud GetSolicitudByMantenimiento(string? mantenimiento)
    {
        return null;
    }
}
