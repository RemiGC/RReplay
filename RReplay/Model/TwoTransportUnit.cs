using Microsoft.Practices.ServiceLocation;
using RReplay.Converters;
using System.Windows.Data;

namespace RReplay.Model
{
    public class TwoTransportUnit : OneTransportUnit
    {
        private ushort landingCraftID;

        // Extented transport property from the XML
        private SimpleUnit landingCraftUnitInfo;

        public TwoTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID, ushort secondTransportID )
            : base(coalition, veterancy, unitID, transportID)
        {
            LandingCraftID = secondTransportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            LandingCraftUnitInfo = repository.GetUnit(coalition, secondTransportID);
        }

        public new int Factory
        {
            get
            {
                IValueConverter fact = new FactoryNumberToName();
                if ( landingCraftUnitInfo.Factory == (int)fact.ConvertBack("Naval", typeof(int), null, null) )
                {
                    return landingCraftUnitInfo.Factory;
                }
                else
                {
                    return UnitInfo.Factory;
                }
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

        public new bool HaveTransport
        {
            get
            {
                return true;
            }
        }

        public SimpleUnit LandingCraftUnitInfo
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
