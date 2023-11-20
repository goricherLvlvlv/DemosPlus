using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    public class Calculator : IModule
    {
        public void OnResolve()
        {
        }

        public void OnDestroy()
        {
        }

        public float CalCost()
        {
            return 0f;
        }

    }

    public class QCalculator : Query<Calculator> { }
}
