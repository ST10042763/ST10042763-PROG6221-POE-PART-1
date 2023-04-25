using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE {
    internal enum UnitOfMeasurement {
        None = 0,
        Tsp = 1,
        Tbsp = 2,
        G = 3,
        Kg = 4,
        Ml = 5,
        L = 6
    }

    internal class UoMConversions {
        public static string UoMToName(UnitOfMeasurement UoM) {
            switch (UoM) {
                case UnitOfMeasurement.None:
                    return null;
                case UnitOfMeasurement.Tsp:
                    return "tea spoons";
                case UnitOfMeasurement.Tbsp:
                    return "table spoons";
                case UnitOfMeasurement.G:
                    return "grams";
                case UnitOfMeasurement.Kg:
                    return "kilograms";
                case UnitOfMeasurement.Ml:
                    return "millilitres";
                case UnitOfMeasurement.L:
                    return "litres";
            }
            return null;
        }
    }
}