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

        public Administrador AltaAdministrador(
            string nombre,
            string apellido,
            string email,
            string contrasena
        )
        {
            return new Administrador(nombre, apellido, email, contrasena);
        }

        //public Invitacion CrearInvitacion(string email, string nombre, DateTime fechaLimite)
        //{
            //return new Invitacion(email, nombre, fechaLimite);
        //}

        //public void EliminarInvitacion(Invitacion invitacion) { }

        public Categoria AltaCategoria(string nombre)
        {
            return new Categoria(nombre);
        }

        public void ModificarInvitacion(Invitacion invitacion, DateTime nuevaFechaLimite)
        {
            // Lógica para extender la fecha límite de una invitación existente
            //invitacion.FechaLimite = nuevaFechaLimite;
        }

        // Eliminar usuarios encargados
        public void EliminarEncargado(Encargado encargado)
        {
            // Lógica para eliminar un encargado del sistema
        }
    }
}
