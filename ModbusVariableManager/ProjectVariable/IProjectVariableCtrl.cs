using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VariableManager.ProjectVariableCtrl;

namespace VariableManager
{
    public interface IProjectVariableCtrl
    {
        ProjectVariableModel VariableModel { get; }
        AcfDevTypeModel AcfDevTypeModel { get; }

        void UpdateFunctionPtr(IsProjectVariableExistFunc IsProjectVariableExist
                             , CreateProjectVariableFunc CreateProjectVariable
                             , ChangeProjectVariableValueFunc ChangeProjectVariableValue
                             , GetProjectVariableListFunc GetProjectVariableList);

        void UpdateFunctionPtr(GetProjectVariableListFunc GetProjectVariableList);

        void UpdateProjectVariableFromData(ref bool RtnState);
        bool CheckVariableExist(string VariableName);
        bool UpdateDataFromProjectVariable();
    }
}
