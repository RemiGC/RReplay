using RReplay.Properties;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace RReplay.Model
{
    public class UnitInfoRepository : IUnitInfoRepository
    {
        SortedList<ushort, UnitInfo> natoList;
        SortedList<ushort, UnitInfo> pactList;

        public UnitInfoRepository()
        {
            var ser = new DataContractSerializer(typeof(UnitesInfo));

            natoList = new SortedList<ushort, UnitInfo>();

            using ( var reader = new FileStream(Path.Combine(Settings.Default.exeFolder, "NATO.xml"), FileMode.Open) )
            {
                UnitesInfo natoUnits = ser.ReadObject(reader) as UnitesInfo;

                foreach ( var unit in natoUnits )
                {
                    natoList.Add(unit.ShowRoomID, unit);
                }
            }

            pactList = new SortedList<ushort, UnitInfo>();

            using ( var reader = new FileStream(Path.Combine(Settings.Default.exeFolder, "PACT.xml"), FileMode.Open) )
            {
                UnitesInfo pactUnits = ser.ReadObject(reader) as UnitesInfo;

                foreach ( var unit in pactUnits)
                {
                    pactList.Add(unit.ShowRoomID, unit);
                }
            }
        }

        public UnitInfo GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            if ( coalition == CoalitionEnum.NATO )
            {
                UnitInfo unit;
                if(natoList.TryGetValue(unitID, out unit))
                {
                    return unit;
                }
                else
                {
                    return null;
                }                
            }
            else
            {
                UnitInfo unit;
                if ( pactList.TryGetValue(unitID, out unit) )
                {
                    return unit;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
