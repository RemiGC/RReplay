using System;
using System.Collections.Generic;


namespace RReplay.Model
{
    enum CoalitionEnum
    {
        NATO = 0x0,
        PACT = 0x1
    }

    public class Deck
    {
        private byte country;
        private byte specialization;
        private byte era;
        private CoalitionEnum coalition;

        public string Country
        {
            get
            {
                if(IsNATO)
                {
                    return NATODictionary[country];
                }
                else
                {
                    return PACTDictionary[country];
                }
            }
        }

        public string Era
        {
            get
            {
                return EraDictionary[era];
            }
        }

        public string Specialization
        {
            get
            {
                return SpecializationDictionary[specialization];
            }
        }

        public string Coalition
        {
            get
            {
                return Enum.GetName(typeof(CoalitionEnum), coalition);
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

        public Deck(string deckCode)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(deckCode);

            byte firstByte = Buffer.GetByte(base64EncodedBytes, 0);
            byte secondByte = Buffer.GetByte(base64EncodedBytes, 1);

            if( firstByte >> 7 == 0)
            {
                coalition = CoalitionEnum.NATO;
            }
            else
            {
                coalition = CoalitionEnum.PACT;
            }

            country = (byte)((firstByte & 0x7F) << 1 | secondByte >> 7);

            specialization = (byte)((secondByte & 0x70) >> 4);
            era = (byte)((secondByte & 0xC) >> 2);

        }

        public static Dictionary<byte, string> SpecializationDictionary = new Dictionary<byte, string>
        {
            { 0x0, "Motorized" },
            { 0x1, "Armored" },
            { 0x2, "Support" },
            { 0x3, "Marines" },
            { 0x4, "Mechanized" },
            { 0x5, "AirBorne" },
            { 0x6, "Navy" },
            { 0x7, "All" }
        };

        public static Dictionary<byte, string> EraDictionary = new Dictionary<byte, string>
        {
            { 0x0, "Before 80" },
            { 0x1, "Before 85" },
            { 0x2, "All" },
        };


        public static Dictionary<byte, string> NATODictionary = new Dictionary<byte, string>
        {
            // NATO Countries
            { 0x09, "US" },
            { 0x19, "UK" },
            { 0x29, "FR" },
            { 0x39 , "RFA" },
            { 0x49 , "CAN" },
            { 0x59 , "DAN" },
            { 0x69 , "SWE" },
            { 0x79 , "NOR" },
            { 0x89 , "ANZ" },
            { 0x99 , "JAP" },
            { 0xA9 , "ROK" },

            // NATO Coalitions
            { 0xB0 , "EURO" },
            { 0xB1 , "SCAND" },
            { 0xB2 , "CMW" },
            { 0xB3 , "BLUEDRAG" },

            { 0xB6 , "LANDJUT" },
            { 0xB8 , "NORAD" },

             // BLUFOR General
            { 0xB9, "BLUFOR" }
        };

        public static Dictionary<byte, string> PACTDictionary = new Dictionary<byte, string>
        {
            // PACT Countries
            { 0x09 , "RDA" },
            { 0x19 , "URSS" },
            { 0x29 , "POL" },
            { 0x39 , "TCH" },
            { 0x49 , "CHI" },
            { 0x59 , "NK" },

            // PACT Coalitions
            { 0x64 , "REDDRAG" },
            { 0x65 , "NSWP" },
            { 0x67 , "SNK" },

            // REDFOR General
            { 0x69, "REDFOR" }
        };
    }
}
