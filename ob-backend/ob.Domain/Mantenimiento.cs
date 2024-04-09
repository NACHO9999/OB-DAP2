namespace ob.Domain
{
    public class Mantenimiento: Usuario
    {
        public Mantenimiento(string nombre, string apellido, string email, string contrasena)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasena = contrasena;
        }

        public Mantenimiento() { }
    }
}