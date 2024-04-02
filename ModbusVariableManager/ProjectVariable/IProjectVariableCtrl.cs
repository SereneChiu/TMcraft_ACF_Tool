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

        void UpdateFunctionPtr(IsProjectVariableExistFunc IsProjectVariableExist
                             , CreateProjectVariableFunc CreateProjectVariable
                             , ChangeProjectVariableValueFunc ChangeProjectVariableValue);

        void UpdateProjectVariableValue(ref bool RtnState);
    }
}
