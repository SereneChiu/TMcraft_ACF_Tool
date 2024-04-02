using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMcraft;

namespace VariableManager
{
    public class VariableData
    {
        private string mVarValue = "";

        public string VarName { get; set; }
        public VariableType VarType { get; set; }
        public string VarValue 
        { 
            get
            {
                return mVarValue;
            }
            set
            {
                mVarValue = value;
            }
        }

        public VariableData(string VarName, VariableType VarType, string VarValue)
        {
            this.VarName = VarName;
            this.VarType = VarType;
            this.mVarValue = VarValue;
        }
    }
}
