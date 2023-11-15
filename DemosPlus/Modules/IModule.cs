using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    public interface IModule
    {
        void OnResolve();

        void OnDestroy();

    }
}
