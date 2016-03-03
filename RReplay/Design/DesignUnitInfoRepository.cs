using RReplay.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RReplay.Design
{
    public class DesignUnitInfoRepository : IUnitInfoRepository
    {
        Dictionary<Tuple<CoalitionEnum, byte>, Nations> nations;
        Dictionary<byte, Era> eras;
        Dictionary<byte, Specialization> specializations;

        List<int> possibleFactory;

        public DesignUnitInfoRepository()
        {
            // Specialization data
            specializations = new Dictionary<byte, Specialization>();
            specializations.Add(1, new Specialization()
            {
                SpecializationID = 1,
                Description = "Armored",
            });

            specializations.Add(7, new Specialization()
            {
                SpecializationID = 7,
                Description = "ALL",
            });

            // Era data
            eras = new Dictionary<byte, Era>();
            eras.Add(0, new Era()
            {
                EraId = 0,
                Description = "Before80"
            });

            eras.Add(1, new Era()
            {
                EraId = 1,
                Description = "Before85"
            });

            eras.Add(2, new Era()
            {
                EraId = 2,
                Description = "ALL"
            });

            // Nation Data
            nations = new Dictionary<Tuple<CoalitionEnum, byte>, Nations>();
            nations.Add(new Tuple<CoalitionEnum, byte>(CoalitionEnum.NATO, 185), new Nations()
            {
                Coalition = 0,
                Description = "BLUFOR",
                NationID = 185
            });

            nations.Add(new Tuple<CoalitionEnum, byte>(CoalitionEnum.NATO, 57), new Nations()
            {
                Coalition = 0,
                Description = "RFA",
                NationID = 57
            });

            nations.Add(new Tuple<CoalitionEnum, byte>(CoalitionEnum.PACT, 57), new Nations()
            {
                Coalition = 1,
                Description = "TCH",
                NationID = 57
            });

            Units_Translation_US translation = new Units_Translation_US()
            {
                Translation = "Super long Infantry"
            };

            possibleFactory = new List<int> { 3,6,7,8,9,10,11,12,13 };

        }
        public SimpleUnit GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            var r = new Random();
            var unit = new SimpleUnit()
            {
                Coalition = (byte)coalition,
                DeckId = unitID,
                ClassNameForDebug = "Unit_2eme_rep",
                UnitName = "A Super long unit name",
                Category = possibleFactory[r.Next(0,possibleFactory.Count)],
                Factory = possibleFactory[r.Next(0, possibleFactory.Count)],
                MotherCountry = "FR",
                UnitTypeName = "Main Battle Tank",
                TypeUnitValue = 3

            };

            return unit;
        }

        public Nations GetNation( CoalitionEnum coalition, byte nationId )
        {
            return nations[new Tuple<CoalitionEnum, byte>(coalition, nationId)];
        }

        public Era GetEra( byte eraId )
        {
            return eras[eraId];
        }

        public Specialization GetSpecialization( byte specializationId )
        {
            return specializations[specializationId];
        }
    }
}
