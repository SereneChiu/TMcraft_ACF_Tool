using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableManager
{
    public interface IProjectVariableModel
    {
        Dictionary<string, VariableData> VarTable { get; }
        void UpdateDictData(string Key, string Value);
    }
}
