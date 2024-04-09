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
    }
}
