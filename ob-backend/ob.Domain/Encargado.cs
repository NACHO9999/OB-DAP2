namespace ob.Domain
{
    public class Encargado : Usuario
    {
        public List<Edificio> Edificios { get; set; }

        public Encargado(string nombre, string apellido, string email, string contrasena)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasena = contrasena;
            Edificios = new List<Edificio>();
        }

        //public Solicitud CrearSolicitud(string descripcion, Depto departamento, Categoria categoria)
        //{
            //return new Solicitud(descripcion, departamento, categoria);
        //}

        //public void AsignarSolicitud(Solicitud solicitud, Mantenimiento personal)
        //{
            // Lógica para asignar la solicitud
        //}
    }
}
