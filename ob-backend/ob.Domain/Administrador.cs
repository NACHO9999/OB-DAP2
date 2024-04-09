namespace ob.Domain
{
    public class Administrador : Usuario
    {
        public Administrador(string nombre, string apellido, string email, string contrasena)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasena = contrasena;
        }

        public Administrador() { }
    }
}
