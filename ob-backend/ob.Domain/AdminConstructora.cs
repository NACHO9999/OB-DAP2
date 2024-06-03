namespace ob.Domain
{
    public class AdminConstructora : Usuario
    {
        public Constructora? Constructora { get; set; }
        public AdminConstructora(string nombre, string email, string contrasena)
        : base(nombre, "null", email, contrasena)
        {

        }


    }
}
