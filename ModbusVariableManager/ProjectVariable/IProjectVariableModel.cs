using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMcraft;

namespace VariableManager
{
    public interface IProjectVariableModel
    {
        Dictionary<string, VariableData> VarTable { get; }
        void UpdateDictData(string Key, string Value);
        void AddProjectVariable(string VarName, string VarValue, VariableType VarType = VariableType.String);
    }
}
