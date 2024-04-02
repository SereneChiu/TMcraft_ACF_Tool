using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrobotics_Controller;

namespace Ferrobotics_Node
{
    public class NodeViewModel
    {
        private SetDataModel mSetDataModel = new SetDataModel();

        public SetDataModel SetDataModel { get { return mSetDataModel; } set { mSetDataModel = value; } }
    }
}
