using System;

namespace ob.Domain
{
    public class Validator
    {
        public static void ValidateInt(int number, int minValue, int maxValue)
        {
            if (number <= minValue || number >= maxValue)
            {
                throw new ArgumentException($"El numero tiene que ser entre {minValue} y {maxValue}.");
            }
        }

        public static void ValidateString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("El texto no puede ser nulo o vacío.");
            }
        }
        public static void ValidateDecimal(decimal number, decimal minValue, decimal maxValue)
        {
            if (number <= minValue || number >= maxValue)
            {
                throw new ArgumentException($"El número tiene que ser entre {minValue} y {maxValue}.");
            }
        }

        public static void ValidateEmail(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new ArgumentException("El email debe contener un '@' y un '.'.");
            }
        }


        public static void ValidateFutureDate(DateTime date)
        {
            if (date < DateTime.Now)
            {
                throw new ArgumentException("La fecha no puede ser en el pasado.");
            }
        }
        public static void ValidatePastDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentException("La fecha no puede ser en el futuro.");
            }
        }
        public static void ValidateStringMaxLength(string str, int max)
        {
            if (str.Length > max)
            {
                throw new ArgumentException($"El texto no puede tener más de {max} caracteres.");
            }
        }
        public static void IsNotNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("El objeto no puede ser nulo.");
            }
        }

    }
}