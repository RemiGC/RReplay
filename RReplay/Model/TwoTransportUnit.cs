using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay.Model
{
    public class TwoTransportUnit : OneTransportUnit
    {
        private ushort landingCraftID;

        // Extented transport property from the XML
        private string secondTransportClassNameDebug;
        private string secondTransportAlias;
        private int secondTransportCategory;
        private int secondTtransportInstanceID;
        private int secondTransportClassNumber;

        public TwoTransportUnit( CoalitionEnum coalition, byte veterancy, ushort unitID, ushort transportID, ushort secondTransportID )
            : base(coalition, veterancy, unitID, transportID)
        {
            LandingCraftID = secondTransportID;
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

        public int SecondTtransportInstanceID
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
    }
}
