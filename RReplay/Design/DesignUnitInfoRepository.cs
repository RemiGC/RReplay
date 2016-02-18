using RReplay.Model;
using RReplay.Properties;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace RReplay.Design
{
    public class DesignUnitInfoRepository : IUnitInfoRepository
    {
        SortedList<ushort, UnitInfo> natoList;
        SortedList<ushort, UnitInfo> pactList;

        public DesignUnitInfoRepository()
        {
            var ser = new DataContractSerializer(typeof(UnitesInfo));

            natoList = new SortedList<ushort, UnitInfo>();

            // TODO find a better way to design time this
            using ( var reader = new FileStream(Path.Combine(@"F:\Dev\RedReplay\RedReplay\Release", "NATO.xml"), FileMode.Open) )
            {
                UnitesInfo natoUnits = ser.ReadObject(reader) as UnitesInfo;

                foreach ( var unit in natoUnits )
                {
                    natoList.Add(unit.ShowRoomID, unit);
                }
            }

            pactList = new SortedList<ushort, UnitInfo>();

            // TODO find a better way to design time this
            using ( var reader = new FileStream(Path.Combine(@"F:\Dev\RedReplay\RedReplay\Release", "PACT.xml"), FileMode.Open) )
            {
                UnitesInfo pactUnits = ser.ReadObject(reader) as UnitesInfo;

                foreach ( var unit in pactUnits )
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
                if ( natoList.TryGetValue(unitID, out unit) )
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
