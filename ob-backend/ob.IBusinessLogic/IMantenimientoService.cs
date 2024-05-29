using ob.Domain;
namespace ob.IBusinessLogic
{
    public interface IMantenimientoService
    {
        void CrearMantenimiento(Mantenimiento mantenimiento);
        Mantenimiento GetMantenimientoByEmail(string email);
        void AtenderSolicitud(Guid solicitudId, string email); 
        void CompletarSolicitud(Guid solicitudId, string email);
    }
    
}