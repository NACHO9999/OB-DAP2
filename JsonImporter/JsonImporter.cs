using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ob.Reflection;
using ob.Reflection.ImportData;

namespace ob.Reflection.JsonImporter
{
    public class JsonImporter : IBuildingImporter
    {
        public string Name => "JSON";

        public List<EdificioData> ImportarEdificios(string path)
        {
            // Leer el archivo JSON
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine("Current Directory: JsonImporter " + currentDirectory);
            var jsonData = File.ReadAllText(path);

            // Deserializar el JSON en una lista de edificios
            var edificiosData = JsonConvert.DeserializeObject<EdificiosData>(jsonData);

            // Convertir los datos deserializados en instancias de Edificio
            var edificios = new List<EdificioData>();
            Console.WriteLine("EdificiosData: " + edificiosData.Edificios.Count);
            foreach (var edificioData in edificiosData.Edificios)
            {
                var deptos = new List<DeptoData>();
                foreach (var deptoData in edificioData.Departamentos)
                {
                    deptos.Add(new DeptoData
                    {
                        Piso = deptoData.Piso,
                        numero_puerta = deptoData.numero_puerta,
                        Habitaciones = deptoData.Habitaciones,
                        ConTerraza = deptoData.ConTerraza,
                        Baños = deptoData.Baños,
                        PropietarioEmail = deptoData.PropietarioEmail
                    });
                }
                edificios.Add(new EdificioData
                {
                    Nombre = edificioData.Nombre ?? "N/A",
                    Direccion = new DireccionData
                    {
                        calle_principal = edificioData.Direccion?.calle_principal ?? "N/A",
                        numero_puerta = edificioData.Direccion?.numero_puerta ?? 0,
                        calle_secundaria = edificioData.Direccion?.calle_secundaria ?? "N/A"
                    },
                    Encargado = edificioData.Encargado ?? "N/A",
                    Gps = new GpsData
                    {
                        Latitud = edificioData.Gps?.Latitud ?? 0,
                        Longitud = edificioData.Gps?.Longitud ?? 0
                    },
                    gastos_comunes = edificioData.gastos_comunes,
                    Departamentos = deptos
                });
            }
            return edificios;
        }
    }

    // Assuming these classes are within the same namespace
    
}