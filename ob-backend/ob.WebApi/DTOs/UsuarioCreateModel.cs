using ob.Domain;
namespace ob.WebApi.DTOs

{

    public class UsuarioCreateModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }

        public Administrador AdminToEntity()
        {
            return new Administrador(this.Nombre, this.Apellido, this.Email, this.Contrasena);

        }
        public Encargado EncargadoToEntity()
        {
            return new Encargado(this.Nombre, this.Email, this.Contrasena);

        }
        public Mantenimiento MantenimientoToEntity()
        {
            return new Mantenimiento(this.Nombre, this.Apellido, this.Email, this.Contrasena);
        }
        public AdminConstructora AdminConstructoraToEntity()
        {
            return new AdminConstructora(this.Nombre, this.Email, this.Contrasena);
        }

    }

}
