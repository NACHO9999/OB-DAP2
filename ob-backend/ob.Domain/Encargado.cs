namespace ob.Domain
{
    public class Encargado : Usuario
    {
        public List<Edificio>? Edificios { get; set; }

        public Encargado(string nombre, string apellido, string email, string contrasena) : base(nombre, apellido, email, contrasena)
        {
            Edificios = new List<Edificio>();
        }
    }
}
