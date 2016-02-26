using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RReplay.Model
{
    public class UnitInfoRepository : IUnitInfoRepository
    {
        Dictionary<int, TUniteAuSol> otanUnits;
        Dictionary<int, TUniteAuSol> pactUnits;
        Dictionary<Tuple<CoalitionEnum, byte>, Nations> nations;
        Dictionary<byte, Era> eras;
        Dictionary<byte, Specialization> specializations;

        public UnitInfoRepository()
        {
            using ( var unitsContext = new UnitsContext() )
            {
                otanUnits = unitsContext.OtanUnits
                  .Include("TUniteAuSol")
                  .Include("TUniteAuSol.Units_Translation_US")
                  .ToDictionary(u => u.DeckId, u => u.TUniteAuSol);

                pactUnits = unitsContext.PactUnits
                  .Include("TUniteAuSol")
                  .Include("TUniteAuSol.Units_Translation_US")
                  .ToDictionary(u => u.DeckId, u => u.TUniteAuSol);

                nations = unitsContext.Nations
                  .ToDictionary(u => new Tuple<CoalitionEnum, byte>((CoalitionEnum)u.Coalition,u.NationID), u => u);

                eras = unitsContext.Era
                    .ToDictionary(u => u.EraId, u => u);

                specializations = unitsContext.Specialization
                    .ToDictionary(u => u.SpecializationID, u => u);
            }
        }

        public TUniteAuSol GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            if (coalition == CoalitionEnum.NATO)
            {
                return otanUnits[unitID];
            }
            else
            {
                return pactUnits[unitID];
            }
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
