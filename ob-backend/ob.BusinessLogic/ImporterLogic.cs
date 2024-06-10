using System.Reflection;
using ob.Reflection.ImportData;
using ob.Reflection;
using ob.IBusinessLogic;
using ob.Exceptions;
using ob.Domain;

namespace ob.BusinessLogic;

public class ImporterLogic : IImporterLogic
{
  public List<IBuildingImporter> GetAllImporters()
  {
    return GetImporterImplementations();
  }

  public List<EdificioData> ImportEdificios(string importerName, string path)
  {
    List<IBuildingImporter> importers = GetImporterImplementations();
    IBuildingImporter? desiredImplementation = null;

    foreach (IBuildingImporter importer in importers)
    {
      if (importer.Name.ToLower() == importerName.ToLower())
      {
        desiredImplementation = importer;
        break;
      }
    }

    if (desiredImplementation == null)
      throw new KeyNotFoundException("No se pudo encontrar el importador solicitado");

    List<EdificioData> importedEdificio = desiredImplementation.ImportarEdificios(path);
    return importedEdificio;
  }

  private List<IBuildingImporter> GetImporterImplementations()
  {
    List<IBuildingImporter> availableImporters = new List<IBuildingImporter>();
    string currentDirectory = Directory.GetCurrentDirectory();
    Console.WriteLine("Current Directory Importerlogic: " + currentDirectory);
    string importersPath = "../Importers";
    string[] filePaths = Directory.GetFiles(importersPath);
    

    foreach (string filePath in filePaths)
    {
      if (filePath.EndsWith(".dll"))
      {
        FileInfo fileInfo = new FileInfo(filePath);
        Assembly assembly = Assembly.LoadFile(fileInfo.FullName);

        foreach (Type type in assembly.GetTypes())
        {
          if (typeof(IBuildingImporter).IsAssignableFrom(type) && !type.IsInterface)
          {
            IBuildingImporter importer = (IBuildingImporter)Activator.CreateInstance(type);
            if (importer != null)
              availableImporters.Add(importer);
          }
        }
      }
    }

    return availableImporters;
  }
}