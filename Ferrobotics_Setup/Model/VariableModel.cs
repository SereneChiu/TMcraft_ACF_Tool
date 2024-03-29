using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMcraft;

namespace Ferrobotics_Setup.Model
{
    public class VariableModel
    {
        public string VarName { get; set; }
        public VariableType VarType { get; set; }
        public string VarValue { get; set; }

        public VariableModel(string VarName, VariableType VarType, string VarValue)
        {
            this.VarName = VarName;
            this.VarType = VarType;
            this.VarValue = VarValue;
        }

    }
}
