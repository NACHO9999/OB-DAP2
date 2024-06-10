using ob.Domain;
using ob.Reflection;
using ob.Reflection.ImportData;


namespace ob.IBusinessLogic;

public interface IImporterLogic
{
    List<IBuildingImporter> GetAllImporters();
    List<EdificioData> ImportEdificios(string importerName, string path);
}