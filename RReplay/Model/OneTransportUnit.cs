using Microsoft.Practices.ServiceLocation;

namespace RReplay.Model
{
    public class OneTransportUnit: Unit
    {
        private ushort transportID;

        // Extented transport property from the XML
        TUniteAuSol transportUnitInfo;


        public OneTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID )
            :base(coalition, veterancy, unitID)
        {
            TransportID = transportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            TransportUnitInfo = repository.GetUnit(coalition, transportID);
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

        public new bool HaveTransport
        {
            get
            {
                return true;
            }
        }

        public TUniteAuSol TransportUnitInfo
        {
            get
            {
                return transportUnitInfo;
            }

            private set
            {
                transportUnitInfo = value;
            }
        }
    }
}
