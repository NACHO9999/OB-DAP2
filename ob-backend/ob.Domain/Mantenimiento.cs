namespace ob.Domain
{
    public class Mantenimiento : Usuario
    {
        public Mantenimiento(string nombre, string apellido, string email, string contrasena)
            : base(nombre, apellido, email, contrasena)
        {
        }
    }
}