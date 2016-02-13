using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace RReplay.Model
{
    enum CoalitionEnum
    {
        NATO = 0x0,
        PACT = 0x1
    }

    public struct TwoTransportUnit
    {
        public byte Veterancy { get; set; }
        public ushort UnitID { get; set; }
        public ushort TransportID { get; set; }
        public ushort LandingCraftID { get; set; }
    };

    public struct OneTransportUnit
    {
        public byte Veterancy { get; set; }
        public ushort UnitID { get; set; }
        public ushort TransportID { get; set; }
    };

    public struct Unit
    {
        public byte Veterancy { get; set; }
        public ushort UnitID { get; set; }
    };

    public class Deck
    {
        private byte country;
        private byte specialization;
        private byte era;
        private CoalitionEnum coalition;
        private byte twoTransportsUnits;
        private byte oneTransportsUnits;
        private byte units;

        private string deckCode;

        public List<TwoTransportUnit> TwoTransportUnitsList;
        public List<OneTransportUnit> OneTransportUnitsList;
        public List<Unit> UnitsList;

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

        public Deck(string PlayerDeckContent )
        {
            this.deckCode = PlayerDeckContent;
            var base64EncodedBytes = System.Convert.FromBase64String(deckCode);

            var bitArray = new BitArray(base64EncodedBytes);
            var bitArrayInversed = new BitArray(bitArray.Length);

            // If it's little endian inverse every 8 bits
            if ( BitConverter.IsLittleEndian )
            {
                for ( int i = 0; i < bitArray.Length / 8; i++ )
                {
                    for ( int j = 0; j < 8; j++ )
                    {
                        bitArrayInversed[i * 8 + j] = bitArray[i * 8 + 7 - j];
                    }
                }
            }
            else
            {
                bitArrayInversed = bitArray;
            }

            int posArray = 0;

            // First bit is the coalition
            coalition = (CoalitionEnum)GetBits<byte>(bitArrayInversed, 1, ref posArray);

            // Next 8 bits is the nation
            country = GetBits<byte>(bitArrayInversed, 8, ref posArray);

            // Next 3 bits is the specialization
            specialization = GetBits<byte>(bitArrayInversed, 3, ref posArray);

            // next 2 bits is the era
            era = GetBits<byte>(bitArrayInversed, 2, ref posArray);

            // next 4 bits is the number of two transports units
            twoTransportsUnits = GetBits<byte>(bitArrayInversed, 4, ref posArray);

            // next 5 bits is the number of two transports units
            oneTransportsUnits = GetBits<byte>(bitArrayInversed, 5, ref posArray);

            TwoTransportUnitsList = new List<TwoTransportUnit>(twoTransportsUnits);

            // 33 bits for each two transport unit
            for (byte i = 0; i < twoTransportsUnits; i++ )
            {
                TwoTransportUnit unit = new TwoTransportUnit();

                unit.Veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                unit.UnitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                unit.TransportID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                unit.LandingCraftID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                TwoTransportUnitsList.Add(unit);
            }

            OneTransportUnitsList = new List<OneTransportUnit>(oneTransportsUnits);

            // 23 bits for each one tansport unit
            for ( byte i = 0; i < oneTransportsUnits; i++ )
            {
                OneTransportUnit unit = new OneTransportUnit();

                unit.Veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                unit.UnitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                unit.TransportID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                OneTransportUnitsList.Add(unit);
            }

            units = (byte)((bitArrayInversed.Length - posArray) / 13);

            UnitsList = new List<Unit>(units);

            // 13 bits for each unit
            for (byte i = 0; i < units; i++ )
            {
                Unit unit = new Unit();

                unit.Veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                unit.UnitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);
                UnitsList.Add(unit);
            }
            string test = ExportDeckCode();
        }

        private string ExportDeckCode()
        {
            int size = 24 + twoTransportsUnits * 33 + oneTransportsUnits * 23 + units * 13;
            BitArray bitArray = new BitArray(size);
            int curPos = 0;

            SetBits(ref bitArray, (byte)coalition, 1, ref curPos);
            SetBits(ref bitArray, country, 8, ref curPos);
            SetBits(ref bitArray, specialization, 3, ref curPos);
            SetBits(ref bitArray, era, 2, ref curPos);
            SetBits(ref bitArray, twoTransportsUnits, 4, ref curPos);
            SetBits(ref bitArray, oneTransportsUnits, 5, ref curPos);

            foreach( var unit in TwoTransportUnitsList )
            {
                SetBits(ref bitArray, unit.Veterancy, 3, ref curPos);
                SetBits(ref bitArray, unit.UnitID, 10, ref curPos);
                SetBits(ref bitArray, unit.TransportID, 10, ref curPos);
                SetBits(ref bitArray, unit.LandingCraftID, 10, ref curPos);
            }

            foreach ( var unit in OneTransportUnitsList )
            {
                SetBits(ref bitArray, unit.Veterancy, 3, ref curPos);
                SetBits(ref bitArray, unit.UnitID, 10, ref curPos);
                SetBits(ref bitArray, unit.TransportID, 10, ref curPos);
            }

            foreach ( var unit in UnitsList )
            {
                SetBits(ref bitArray, unit.Veterancy, 3, ref curPos);
                SetBits(ref bitArray, unit.UnitID, 10, ref curPos);
            }

            BitArray bitArrayInversed = new BitArray(size);
            if ( BitConverter.IsLittleEndian )
            {
                for ( int i = 0; i < bitArray.Length / 8; i++ )
                {
                    for ( int j = 0; j < 8; j++ )
                    {
                        bitArrayInversed[i * 8 + j] = bitArray[i * 8 + 7 - j];
                    }
                }
            }
            else
            {
                bitArrayInversed = bitArray;
            }

            byte[] array = BitArrayToByteArray(bitArrayInversed);

            var base64 = System.Convert.ToBase64String(array);
            return base64;
        }

        private static byte[] BitArrayToByteArray( BitArray bits )
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
         
        private void SetBits(ref BitArray bitArray, ushort value, byte numberOfBits, ref int posArray)
        {
            var bytes = BitConverter.GetBytes(value);

            if ( BitConverter.IsLittleEndian )
            {
            //    Array.Reverse(bytes);
            }

            BitArray bit = new BitArray(bytes);
            for ( byte j = 1; j <= numberOfBits; j++ )
            {
                bitArray[posArray++] = bit[numberOfBits - j];
            }
        }

        public string DeckCode
        {
            get
            {
                return deckCode;
            }
        }

        /// <summary>
        /// Read numberOfBits of bits from bitArray and increase startingPosArray by numberOfBits
        /// </summary>
        /// <typeparam name="T">An unsigned integer lower than 16 bits</typeparam>
        /// <param name="bitArray">The array where to read the bits</param>
        /// <param name="numberOfBits">The number of bits to read</param>
        /// <param name="startingPosArray">The starting position to read</param>
        /// <returns>An unsigned integer</returns>
        private T GetBits<T>(BitArray bitArray, byte numberOfBits , ref int startingPosArray)
        {
            ushort bits = 0;

            for ( byte j = 0; j < numberOfBits; j++ )
            {
                if ( bitArray[startingPosArray++] )
                {
                    bits |= (ushort)(1 << (numberOfBits - 1) - j);
                }
            }

            return (T)Convert.ChangeType(bits, typeof(T));
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
