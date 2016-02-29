using Microsoft.Practices.ServiceLocation;

namespace RReplay.Model
{
    public class TwoTransportUnit : OneTransportUnit
    {
        private ushort landingCraftID;

        // Extented transport property from the XML
        private TUniteAuSol landingCraftUnitInfo;

        public TwoTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID, ushort secondTransportID )
            : base(coalition, veterancy, unitID, transportID)
        {
            LandingCraftID = secondTransportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            LandingCraftUnitInfo = repository.GetUnit(coalition, secondTransportID);
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

        public new bool HaveTransport
        {
            get
            {
                return true;
            }
        }

        public TUniteAuSol LandingCraftUnitInfo
        {
            get
            {
                return landingCraftUnitInfo;
            }

            private set
            {
                landingCraftUnitInfo = value;
            }
        }
    }
}
