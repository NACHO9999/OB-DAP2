namespace ob.Domain;

public abstract class Usuario
{
    private string _nombre;
    public string Nombre {
        get { return _nombre; }
        set {
            Validator.ValidateStringMaxLength(value, 100);
            Validator.ValidateString(value);
            _nombre = value;
        }
    }
    private string _apellido;
    public string Apellido {
        get { return _apellido; }
        set {
            Validator.ValidateStringMaxLength(value, 100);
            Validator.ValidateString(value);
            _apellido = value;
        }
    }
    private string _email;
    public string Email {
        get { return _email; }
        set {
            Validator.ValidateEmail(value);
            Validator.ValidateStringMaxLength(value, 320);
            _email = value;
        }
    }
    public string Contrasena { get; set; }
    protected Usuario(string nombre, string apellido, string email, string contrasena)
    {
        _nombre = nombre;
        _apellido = apellido;
        _email = email;
        Contrasena = contrasena;
    }

}
