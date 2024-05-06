using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableManager
{
    public class AcfDevTypeModel
    {
        private Dictionary<string, decimal> mDevPayloadTable_Acfk = new Dictionary<string, decimal>()
        {
            { "ACF-K / Dyn / 56819 00 (1.5 kg)", (decimal)1.5 }
          , { "ACF-K / Dyn / 56830 00 (1.5 kg)", (decimal)1.5 }
          , { "ACF-K / Dyn / 57814 00 (1.7 kg)", (decimal)1.7 }
          , { "ACF-K / Dyn / 15360 00 (1.8 kg)", (decimal)1.8 }
          , { "ACF-K / Dyn / 13214 00 (1.5 kg)", (decimal)1.5 }
          , { "ACF-K / Dyn / 52276 00 (1.4 kg)", (decimal)1.4 }
          , { "ACF-K / Dyn / 52276 01 (1.5 kg)", (decimal)1.5 }
          , { "ACF-K / Dyn / 54771 00 (1.6 kg)", (decimal)1.6 }
          , { "ACF-K / Dyn / 52635 00 (2.5 kg)", (decimal)2.5 }
          , { "ACF-K / Dyn / 52639 00 (2.5 kg)", (decimal)2.5 }
          , { "Others (Payload input directly by user)", (decimal)0.0 }
        };

        private Dictionary<string, decimal> mDevPayloadTable_Acf = new Dictionary<string, decimal>()
        {
            { "Payload input directly by user", (decimal)0.0 }
        };

        private Dictionary<string, decimal> mDevPayloadTable_Aok = new Dictionary<string, decimal>()
        {
            { "Velocity input directly by user", (decimal)0.0 }
        };


        private IList<AcfDevType> mDevEntries = new List<AcfDevType>()
        {
            new AcfDevType("ACF-K")
          , new AcfDevType("ACF / ATK")
          , new AcfDevType("AOK-AAK")
        };

        public Dictionary<AcfDevType, Dictionary<string, decimal>> DevInfoTable 
        { 
            get 
            { 
                return new Dictionary<AcfDevType, Dictionary<string, decimal>>()
                {
                    { mDevEntries[0], mDevPayloadTable_Acfk }
                  , { mDevEntries[1], mDevPayloadTable_Acf }
                  , { mDevEntries[2], mDevPayloadTable_Aok }
                }; 
            } 
        }


        public IList<AcfDevType> DevEntries { get { return mDevEntries; } }

        public IList<AcfDevType> DevSubEntries_Acfk 
        { 
            get 
            {
                IList<AcfDevType> entrys = new List<AcfDevType>();
                if (entrys.Count == 0)
                {
                    foreach (string str in mDevPayloadTable_Acfk.Keys)
                    {
                        entrys.Add(new AcfDevType(str));
                    }
                }
                return entrys;
            } 
        }

        public IList<AcfDevType> DevSubEntries_Acf
        {
            get
            {
                IList<AcfDevType> entrys = new List<AcfDevType>();
                if (entrys.Count == 0)
                {
                    foreach (string str in mDevPayloadTable_Acf.Keys)
                    {
                        entrys.Add(new AcfDevType(str));
                    }
                }
                return entrys;
            }
        }

        public IList<AcfDevType> DevSubEntries_Aok
        {
            get
            {
                IList<AcfDevType> entrys = new List<AcfDevType>();
                if (entrys.Count == 0)
                {
                    foreach (string str in mDevPayloadTable_Aok.Keys)
                    {
                        entrys.Add(new AcfDevType(str));
                    }
                }
                return entrys;
            }
        }
    }
}
