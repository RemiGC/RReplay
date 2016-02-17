using Microsoft.Practices.ServiceLocation;

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
        private uint instanceID;
        private int classNumber;

        public Unit(CoalitionEnum coalition, byte veterancy, ushort unitID )
        {
            Coalition = coalition;
            Veterancy = veterancy;
            UnitID = unitID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            var unitInfo = repository.GetUnit(coalition, unitID);

            if ( unitInfo != null )
            {
                ClassNameDebug = unitInfo.ClassNameForDebug;
                Alias = unitInfo.AliasName;
                Category = unitInfo.Category;
                InstanceID = unitInfo.InstanceID;
                ClassNumber = unitInfo.Class;
            }
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

        public uint InstanceID
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
