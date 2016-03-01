using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RReplay.Model
{
    public class UnitInfoRepository : IUnitInfoRepository
    {
        Dictionary<Tuple<CoalitionEnum, int>, SimpleUnit> simpleUnits;
        Dictionary<Tuple<CoalitionEnum, byte>, Nations> nations;
        Dictionary<byte, Era> eras;
        Dictionary<byte, Specialization> specializations;

        public UnitInfoRepository()
        {
            using ( var unitsContext = new UnitsContext() )
            {
                simpleUnits = unitsContext.MainUnitsView
                  .ToDictionary(u => new Tuple<CoalitionEnum, int>((CoalitionEnum)u.Coalition,u.DeckId), u => u);

                nations = unitsContext.Nations
                  .ToDictionary(u => new Tuple<CoalitionEnum, byte>((CoalitionEnum)u.Coalition,u.NationID), u => u);

                eras = unitsContext.Era
                    .ToDictionary(u => u.EraId, u => u);

                specializations = unitsContext.Specialization
                    .ToDictionary(u => u.SpecializationID, u => u);
            }
        }

        public SimpleUnit GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            return simpleUnits[new Tuple<CoalitionEnum, int>(coalition, unitID)];
        }

        public Nations GetNation( CoalitionEnum coalition, byte nationId )
        {
            var nat = nations[new Tuple<CoalitionEnum, byte>(coalition, nationId)];
            return nat;
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
