using Microsoft.Practices.ServiceLocation;

namespace RReplay.Model
{
    public class TwoTransportUnit : OneTransportUnit
    {
        private ushort landingCraftID;

        // Extented transport property from the XML
        private string secondTransportClassNameDebug;
        private string secondTransportAlias;
        private int secondTransportCategory;
        private uint secondTtransportInstanceID;
        private int secondTransportClassNumber;
        private string secondTransportImagePath;

        public TwoTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID, ushort secondTransportID )
            : base(coalition, veterancy, unitID, transportID)
        {
            LandingCraftID = secondTransportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            var unitInfo = repository.GetUnit(coalition, secondTransportID);

            if ( unitInfo != null )
            {
                secondTransportClassNameDebug = unitInfo.ClassNameForDebug;
                secondTransportAlias = unitInfo.AliasName;
                secondTransportCategory = unitInfo.Category;
                secondTtransportInstanceID = unitInfo.InstanceID;
                secondTransportClassNumber = unitInfo.Class;
            }
        }

        public string SecondTransportClassNameDebug
        {
            get
            {
                return secondTransportClassNameDebug;
            }

            private set
            {
                secondTransportClassNameDebug = value;
            }
        }

        public string SecondTransportAlias
        {
            get
            {
                return secondTransportAlias;
            }

            private set
            {
                secondTransportAlias = value;
            }
        }

        public int SecondTransportCategory
        {
            get
            {
                return secondTransportCategory;
            }

            private set
            {
                secondTransportCategory = value;
            }
        }

        public uint SecondTtransportInstanceID
        {
            get
            {
                return secondTtransportInstanceID;
            }

            private set
            {
                secondTtransportInstanceID = value;
            }
        }

        public int SecondTransportClassNumber
        {
            get
            {
                return secondTransportClassNumber;
            }

            private set
            {
                secondTransportClassNumber = value;
            }
        }

        public ushort LandingCraftID
        {
            get
            {
                return landingCraftID;
            }

            private set
            {
                landingCraftID = value;
            }
        }

        public string SecondTransportImagePath
        {
            get
            {
                return secondTransportImagePath;
            }

            private set
            {
                secondTransportImagePath = value;
            }
        }
    }
}
