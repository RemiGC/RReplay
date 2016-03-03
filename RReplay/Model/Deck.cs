using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RReplay.Model
{
    /// <summary>
    /// A class that represent a Complete Deck build from a deck code
    /// </summary>
    public class Deck: INotifyPropertyChanged, IEquatable<Deck>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Nations nation;
        private Specialization specialization;
        private Era era;
        private CoalitionEnum coalition;
        private byte twoTransportsUnits;
        private byte oneTransportsUnits;
        private byte units;

        private string deckCode;

        public List<Unit> UnitsList;

        //private Era era;

        public string Country
        {
            get
            {
                return nation.Description;
            }
        }

        private void NotifyPropertyChanged( [CallerMemberName] String propertyName = "" )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Era
        {
            get
            {
                return era.Description;
            }
        }

        public string Specialization
        {
            get
            {
                return specialization.Description;
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

        public Deck(string PlayerDeckContent, IUnitInfoRepository repository )
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
            nation = repository.GetNation(coalition, GetBits<byte>(bitArrayInversed, 8, ref posArray));

            // Next 3 bits is the specialization
            specialization = repository.GetSpecialization(GetBits<byte>(bitArrayInversed, 3, ref posArray));

            // next 2 bits is the era
            era = repository.GetEra(GetBits<byte>(bitArrayInversed, 2, ref posArray));

            // next 4 bits is the number of two transports units
            twoTransportsUnits = GetBits<byte>(bitArrayInversed, 4, ref posArray);

            // next 5 bits is the number of two transports units
            oneTransportsUnits = GetBits<byte>(bitArrayInversed, 5, ref posArray);


            UnitsList = new List<Unit>();

            // 33 bits for each two transport unit
            for (byte i = 0; i < twoTransportsUnits; i++ )
            {
                byte veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                ushort unitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                ushort transportID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                ushort landingCraftID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                UnitsList.Add(new TwoTransportUnit(coalition,veterancy,unitID,transportID,landingCraftID));
            }

            // 23 bits for each one tansport unit
            for ( byte i = 0; i < oneTransportsUnits; i++ )
            {
                byte veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                ushort unitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                ushort transportID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                UnitsList.Add(new OneTransportUnit(coalition, veterancy, unitID, transportID));
            }

            units = (byte)((bitArrayInversed.Length - posArray) / 13);

            // 13 bits for each unit
            for (byte i = 0; i < units; i++ )
            {
                byte veterancy = GetBits<byte>(bitArrayInversed, 3, ref posArray);

                ushort unitID = GetBits<ushort>(bitArrayInversed, 10, ref posArray);

                UnitsList.Add(new Unit(coalition, veterancy, unitID));
            }
        }

        public void NewList(IEnumerable<Unit> list)
        {
            UnitsList = list.ToList();
            PropertyChanged(this, new PropertyChangedEventArgs("DeckCode"));
        }

        /// <summary>
        /// Export the deck code to a usable form for the game
        /// </summary>
        /// <returns></returns>
        private string ExportDeckCode()
        {
            int size = 23 + twoTransportsUnits * 33 + oneTransportsUnits * 23 + units * 13;
            size = ((size - 1) / 8 + 1) * 8; //round up to an exact number of bytes
            BitArray bitArray = new BitArray(size);
            int curPos = 0;

            SetBits(ref bitArray, (byte)coalition, 1, ref curPos);
            SetBits(ref bitArray, nation.NationID, 8, ref curPos);
            SetBits(ref bitArray, specialization.SpecializationID, 3, ref curPos);
            SetBits(ref bitArray, era.EraId, 2, ref curPos);
            SetBits(ref bitArray, twoTransportsUnits, 4, ref curPos);
            SetBits(ref bitArray, oneTransportsUnits, 5, ref curPos);

            foreach( var unit in UnitsList.OfType<TwoTransportUnit>())
            {
                SetBits(ref bitArray, unit.Veterancy, 3, ref curPos);
                SetBits(ref bitArray, unit.UnitID, 10, ref curPos);
                SetBits(ref bitArray, unit.TransportID, 10, ref curPos);
                SetBits(ref bitArray, unit.LandingCraftID, 10, ref curPos);
            }

            foreach ( var unit in UnitsList.OfType<OneTransportUnit>().Where(i => i.GetType() == typeof(OneTransportUnit)) )
            {
                SetBits(ref bitArray, unit.Veterancy, 3, ref curPos);
                SetBits(ref bitArray, unit.UnitID, 10, ref curPos);
                SetBits(ref bitArray, unit.TransportID, 10, ref curPos);
            }

            foreach ( var unit in UnitsList.OfType<Unit>().Where(i => i.GetType() == typeof(Unit)) )
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
                return ExportDeckCode();
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

        public bool Equals( Deck other )
        {
            return DeckCode == other.DeckCode;
        }
    }
}
