namespace ob.Domain
{
    public class Encargado : Usuario
    {
        public List<Edificio> Edificios { get; set; }

        public Encargado(string nombre,  string email, string contrasena) : base(nombre, "null",  email, contrasena)
        {
            Edificios = new List<Edificio>();
        }
    }
}
