using Microsoft.VisualBasic;
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
        public delegate uint GetProjectVariableListFunc(ref List<VariableInfo> RtnVarList);

        private ProjectVariableModel mVariableModel = new ProjectVariableModel();
        private IsProjectVariableExistFunc IsProjectVariableExist = null;
        private CreateProjectVariableFunc CreateProjectVariable = null;
        private ChangeProjectVariableValueFunc ChangeProjectVariableValue = null;
        private GetProjectVariableListFunc GetProjectVariableList = null;

        public ProjectVariableModel VariableModel { get { return mVariableModel; } }

        public void UpdateFunctionPtr(IsProjectVariableExistFunc IsProjectVariableExist
                                    , CreateProjectVariableFunc CreateProjectVariable
                                    , ChangeProjectVariableValueFunc ChangeProjectVariableValue
                                    , GetProjectVariableListFunc GetProjectVariableList)
        {
            this.IsProjectVariableExist = IsProjectVariableExist;
            this.CreateProjectVariable = CreateProjectVariable;
            this.ChangeProjectVariableValue = ChangeProjectVariableValue;
            this.GetProjectVariableList = GetProjectVariableList;
        }


        private bool CheckFunctionExist()
        {
            if (IsProjectVariableExist == null
                || CreateProjectVariable == null
                || ChangeProjectVariableValue == null
                || GetProjectVariableList == null) { return false; }
            return true;
        }


        public string GetProjectVariableValue(string VarName)
        {
            if (CheckFunctionExist() == false) { return null; }
            List<VariableInfo> rtn_var_list = new List<VariableInfo>();

            if (GetProjectVariableList(ref rtn_var_list) != 0)
            {
                return null;
            }

            VariableInfo var_info = rtn_var_list.First(p => p.Name == VarName);
            if (var_info == null)
            {
                return null;
            }

            return var_info.value;
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
