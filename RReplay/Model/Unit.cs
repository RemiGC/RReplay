using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay.Model
{
    public class Unit
    {
        // Base property from the deck code
        private CoalitionEnum coalition;
        private byte veterancy;
        private ushort unitID;

        // Extented property from the XML
        private string classNameDebug;
        private string alias;
        private int category;
        private int instanceID;
        private int classNumber;


        public Unit(CoalitionEnum coalition, byte veterancy, ushort unitID )
        {
            Coalition = coalition;
            Veterancy = veterancy;
            UnitID = unitID;
            /*ClassNameDebug = className;
            Alias = alias;
            Category = category;
            InstanceID = instanceID;
            ClassNumber = classNumber;*/
        }

        public string ClassNameDebug
        {
            get
            {
                return classNameDebug;
            }
            private set
            {
                classNameDebug = value;
            }
        }

        public CoalitionEnum Coalition
        {
            get
            {
                return coalition;
            }
            private set
            {
                coalition = value;
            }
        }

        public bool IsNATO
        {
            get
            {
                return coalition == CoalitionEnum.NATO;
            }
        }

        public bool IsPACT
        {
            get
            {
                return coalition == CoalitionEnum.PACT;
            }
        }

        public string Alias
        {
            get
            {
                return alias;
            }

            private set
            {
                alias = value;
            }
        }

        public int Category
        {
            get
            {
                return category;
            }

            private set
            {
                category = value;
            }
        }

        public int InstanceID
        {
            get
            {
                return instanceID;
            }

            private set
            {
                instanceID = value;
            }
        }

        public int ClassNumber
        {
            get
            {
                return classNumber;
            }

            private set
            {
                classNumber = value;
            }
        }

        public byte Veterancy
        {
            get
            {
                return veterancy;
            }

            private set
            {
                veterancy = value;
            }
        }

        public ushort UnitID
        {
            get
            {
                return unitID;
            }

            private set
            {
                unitID = value;
            }
        }
    }
}
