using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMcraft;

namespace VariableManager
{
    public class ProjectVariableCtrl : IProjectVariableCtrl
    {
        public delegate bool IsProjectVariableExistFunc(string varName);
        public delegate uint CreateProjectVariableFunc(string name, VariableType type, string value);
        public delegate uint ChangeProjectVariableValueFunc(List<string[]> value);

        private ProjectVariableModel mVariableModel = new ProjectVariableModel();
        private IsProjectVariableExistFunc IsProjectVariableExist = null;
        private CreateProjectVariableFunc CreateProjectVariable = null;
        private ChangeProjectVariableValueFunc ChangeProjectVariableValue = null;

        public ProjectVariableModel VariableModel { get { return mVariableModel; } }

        public void UpdateFunctionPtr(IsProjectVariableExistFunc IsProjectVariableExist
                                    , CreateProjectVariableFunc CreateProjectVariable
                                    , ChangeProjectVariableValueFunc ChangeProjectVariableValue)
        {
            this.IsProjectVariableExist = IsProjectVariableExist;
            this.CreateProjectVariable = CreateProjectVariable;
            this.ChangeProjectVariableValue = ChangeProjectVariableValue;
        }


        private bool CheckFunctionExist()
        {
            if (IsProjectVariableExist == null
                || CreateProjectVariable == null
                || ChangeProjectVariableValue == null) { return false; }
            return true;
        }


        public void UpdateProjectVariableValue(ref bool RtnState)
        {
            if (CheckFunctionExist() == false) { RtnState = false; }

            List<string[]> value_list = new List<string[]>();
            
            foreach (VariableData var in mVariableModel.VarTable.Values)
            {
                if (false == this.IsProjectVariableExist(var.VarName))
                {
                    this.CreateProjectVariable(var.VarName, var.VarType, var.VarValue);
                    value_list.Add(new string[] { var.VarName, var.VarValue });
                    RtnState = true;
                }
            }
            if (RtnState == false) { return; }

            this.ChangeProjectVariableValue(value_list);

            RtnState = true;
        }
    }
}
