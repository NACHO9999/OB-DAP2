using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ob.Reflection.ImportData
{
    
        public class EdificiosData
        {
            public List<EdificioData> Edificios { get; set; }
        }

        public class EdificioData
        {
            public string Nombre { get; set; }
            public DireccionData Direccion { get; set; }
            public string Encargado { get; set; }
            public GpsData Gps { get; set; }
            public decimal gastos_comunes { get; set; }
            public List<DeptoData> Departamentos { get; set; }
        }

        public class DireccionData
        {
            public string calle_principal { get; set; }
            public int numero_puerta { get; set; }
            public string calle_secundaria { get; set; }
        }

        public class GpsData
        {
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
        }

        public class DeptoData
        {
            public int Piso { get; set; }
            public int numero_puerta { get; set; }
            public int Habitaciones { get; set; }
            public bool ConTerraza { get; set; }
            public int Baños { get; set; }
            public string PropietarioEmail { get; set; }
        }
    }

