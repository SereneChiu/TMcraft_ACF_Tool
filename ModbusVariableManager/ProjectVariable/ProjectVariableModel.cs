using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TMcraft;

namespace VariableManager
{
    public class ProjectVariableModel : IProjectVariableModel
    {
        public Dictionary<string, VariableData> VarTable { get { return mVarDict; } }


        private static List<string> mVarNameList = new List<string>()
        {
            "ferrobotics_ip"
          , "ferrobotics_port"
          , "ferrobotics_do_type"
          , "ferrobotics_do_channel"
          , "ferrobotics_do_status"
        };

        private Dictionary<string, VariableData> mVarDict = new Dictionary<string, VariableData>();


        public ProjectVariableModel()
        {
            InitDictData();
        }

        private void InitDictData()
        {
            mVarDict.Clear();
            mVarDict = new Dictionary<string, VariableData>()
            {
                { mVarNameList[0], new VariableData(mVarNameList[0], VariableType.String, "192.168.99.1") }
              , { mVarNameList[1], new VariableData(mVarNameList[1], VariableType.Integer, "7070") }
              , { mVarNameList[2], new VariableData(mVarNameList[2], VariableType.String, "") }
              , { mVarNameList[3], new VariableData(mVarNameList[3], VariableType.Integer, "") }
              , { mVarNameList[4], new VariableData(mVarNameList[4], VariableType.Boolean, "") }
            };
        }

        public void AddProjectVariable(string VarName, string VarValue, VariableType VarType = VariableType.String)
        {
            if (mVarDict.ContainsKey(VarName) == true) 
            {
                mVarDict[VarName] = new VariableData(VarName, VarType, VarValue);
                return; 
            }
            mVarDict.Add(VarName, new VariableData(VarName, VarType, VarValue));
        }

        public void UpdateDictData(string Key, string Value)
        {
            if (mVarDict.ContainsKey(Key) == false) { return; }
            mVarDict[Key].VarValue = Value;
        }


    }
}
