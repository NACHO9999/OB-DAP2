namespace ob.Domain
{
    public class AdminConstructora : Usuario
    {
        public Constructora? Constructora { get; set; }
        public AdminConstructora(string nombre, string apellido, string email, string contrasena)
        : base(nombre, apellido, email, contrasena)
        {

        }


    }
}
