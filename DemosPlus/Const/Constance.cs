using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus
{
    public enum City
    {
        None,
        Thetford,
        BridgeWatch,
        Martlock,
        Lymhurst,
        FortSterling,
        Caerleon,
    }

    public enum Item
    {
        None,
        T4_Bag,
        T5_Bag,
        T6_Bag,
        T7_Bag,
        T8_Bag,
        T4_PLANKS,
        T5_PLANKS,
        T6_PLANKS,
        T7_PLANKS,
        T8_PLANKS,
    }

    public enum Quality
    {
        None = 0,
        Normal = 1,
        Good = 2,
        Outstanding = 3,
        Excellent = 4,
        Masterpiece = 5,
    }

    public enum Duration
    { 
        OneDay,
        SevenDays,
        TwoWeeks,
        ThreeWeeks,
        OneMonth,
    }

    public enum Tax
    { 
        Tax_6_26,
        Tax_8_25,
    }

}
