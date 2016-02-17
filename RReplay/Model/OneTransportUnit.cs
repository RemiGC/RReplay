using Microsoft.Practices.ServiceLocation;

namespace RReplay.Model
{
    public class OneTransportUnit: Unit
    {
        private ushort transportID;

        // Extented transport property from the XML
        private string transportClassNameDebug;
        private string transportAlias;
        private int transportCategory;
        private uint transportInstanceID;
        private int transportClassNumber;

        public OneTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID )
            :base(coalition, veterancy, unitID)
        {
            TransportID = transportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            var unitInfo = repository.GetUnit(coalition, transportID);

            if ( unitInfo != null )
            {
                transportClassNameDebug = unitInfo.ClassNameForDebug;
                transportAlias = unitInfo.AliasName;
                transportCategory = unitInfo.Category;
                transportInstanceID = unitInfo.InstanceID;
                transportClassNumber = unitInfo.Class;
            }
        }

        public ushort TransportID
        {
            get
            {
                return transportID;
            }

            private set
            {
                transportID = value;
            }
        }

        public string TransportClassNameDebug
        {
            get
            {
                return transportClassNameDebug;
            }

            private set
            {
                transportClassNameDebug = value;
            }
        }

        public string TransportAlias
        {
            get
            {
                return transportAlias;
            }

            private set
            {
                transportAlias = value;
            }
        }

        public int TransportCategory
        {
            get
            {
                return transportCategory;
            }

            private set
            {
                transportCategory = value;
            }
        }

        public uint TransportInstanceID
        {
            get
            {
                return transportInstanceID;
            }

            private set
            {
                transportInstanceID = value;
            }
        }

        public int TransportClassNumber
        {
            get
            {
                return transportClassNumber;
            }

            private set
            {
                transportClassNumber = value;
            }
        }
    }
}
