using Microsoft.Practices.ServiceLocation;
using RReplay.Converters;
using System.Windows.Data;

namespace RReplay.Model
{
    public class OneTransportUnit: Unit
    {
        private ushort transportID;

        // Extented transport property from the XML
        SimpleUnit transportUnitInfo;


        public OneTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID, IDeckInfoRepository repository )
            :base(coalition, veterancy, unitID, repository)
        {
            TransportID = transportID;

            TransportUnitInfo = repository.GetUnit(coalition, transportID);
        }

        public new int Factory
        {
            get
            {
                IValueConverter fact = new FactoryNumberToName();
                if ( transportUnitInfo.Factory == (int)fact.ConvertBack("Naval",typeof(int),null,null) )
                {
                    return transportUnitInfo.Factory;
                }
                else
                {
                    return base.Factory;
                }
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

        public new bool HaveTransport
        {
            get
            {
                return true;
            }
        }

        public SimpleUnit TransportUnitInfo
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
