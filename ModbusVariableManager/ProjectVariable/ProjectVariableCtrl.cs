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
        private AcfDevTypeModel mAcfDevTypeModel = new AcfDevTypeModel();
        private IsProjectVariableExistFunc IsProjectVariableExist = null;
        private CreateProjectVariableFunc CreateProjectVariable = null;
        private ChangeProjectVariableValueFunc ChangeProjectVariableValue = null;
        private GetProjectVariableListFunc GetProjectVariableList = null;

        public ProjectVariableModel VariableModel { get { return mVariableModel; } }
        public AcfDevTypeModel AcfDevTypeModel { get { return mAcfDevTypeModel; } }


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

        public bool CheckVariableExist(string VariableName)
        {
            if (CheckFunctionExist() == false) { return false; }

            List<VariableInfo> var_list = new List<VariableInfo>();

            if (GetProjectVarList(ref var_list) == false) { return false; }
            return var_list.Exists(p => p.Name == VariableName);
        }

        private bool GetProjectVarList(ref List<VariableInfo> RtnVarList)
        {
            RtnVarList = new List<VariableInfo>();
            if (GetProjectVariableList(ref RtnVarList) != 0)
            {
                return false;
            }
            return true;
        }

        public bool UpdateDataFromProjectVariable()
        {
            if (CheckFunctionExist() == false) { return false; }

            List<VariableInfo> rtn_var_list = new List<VariableInfo>();
            if (false == GetProjectVarList(ref rtn_var_list))
            {
                return false;
            }

            if (rtn_var_list.Count == 0) { return false; }

            foreach (string key_str in VariableModel.VarTable.Keys)
            {
                if (rtn_var_list.Exists(p => p.Name == "var_" + key_str) == false)
                {
                    continue;
                }

                VariableInfo var_info = rtn_var_list.First(p => p.Name == "var_" + key_str);
                VariableModel.VarTable[key_str].VarValue = var_info.value;
            }
            return true;
        }

        public void UpdateProjectVariableFromData(ref bool RtnState)
        {
            if (CheckFunctionExist() == false) { RtnState = false; return; }

            List<string[]> value_list = new List<string[]>();

            if (mVariableModel.VarTable.Values.Count == 0) { RtnState = false; return; }

            foreach (VariableData var in mVariableModel.VarTable.Values)
            {
                if (false == this.IsProjectVariableExist(var.VarName))
                {
                    this.CreateProjectVariable(var.VarName, var.VarType, var.VarValue);
                }
                value_list.Add(new string[] { "var_" + var.VarName, var.VarValue });
                RtnState = true;
            }
            this.ChangeProjectVariableValue(value_list);

            RtnState = true;
        }
    }
}
