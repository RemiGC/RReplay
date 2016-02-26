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
        TUniteAuSol unitInfo;

        public Unit(CoalitionEnum coalition, byte veterancy, ushort unitID )
        {
            Coalition = coalition;
            Veterancy = veterancy;
            UnitID = unitID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            UnitInfo = repository.GetUnit(coalition, unitID);
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

        public TUniteAuSol UnitInfo
        {
            get
            {
                return unitInfo;
            }

            private set
            {
                unitInfo = value;
            }
        }
    }
}
