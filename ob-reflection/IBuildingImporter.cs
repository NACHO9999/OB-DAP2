using ob.Reflection.ImportData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ob.Reflection
{
    public interface IBuildingImporter
    {
        string Name { get; } 
        public List<EdificioData> ImportarEdificios(string path);
    }
}
