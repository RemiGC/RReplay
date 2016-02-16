using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay.Model
{
    public class OneTransportUnit: Unit
    {
        private ushort transportID;

        // Extented transport property from the XML
        private string transportClassNameDebug;
        private string transportAlias;
        private int transportCategory;
        private int transportInstanceID;
        private int transportClassNumber;
        private string transportImagePath;

        public OneTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID )
            :base(coalition, veterancy, unitID)
        {
            TransportID = transportID;

            IUnitInfoRepository repository = ServiceLocator.Current.GetInstance<IUnitInfoRepository>();

            UnitInfo unitInfo = repository.GetUnit(coalition, transportID);

            transportClassNameDebug = unitInfo.classNameDebug;
            transportAlias = unitInfo.alias;
            transportCategory = unitInfo.category;
            transportInstanceID = unitInfo.instanceID;
            transportClassNumber = unitInfo.classNumber;
            transportImagePath = unitInfo.imagePath;
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

        public int TransportInstanceID
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

        public string TransportImagePath
        {
            get
            {
                return transportImagePath;
            }

            private set
            {
                transportImagePath = value;
            }
        }
    }
}
