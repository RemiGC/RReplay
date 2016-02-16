using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RReplay.Model
{
    public class UnitInfoRepository : IUnitInfoRepository
    {
        SortedList<ushort, UnitesUnit> natoList;
        SortedList<ushort, UnitesUnit> pactList;

        public UnitInfoRepository()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Unites));

            Unites natoUnits = ser.Deserialize(new FileStream("NATO.xml", FileMode.Open)) as Unites;

            natoList = new SortedList<ushort, UnitesUnit>(natoUnits.Unit.Length);

            foreach (var unit in natoUnits.Unit)
            {
                natoList.Add(unit.ShowRoomID, unit);
            }

            Unites pactUnits = ser.Deserialize(new FileStream("PACT.xml", FileMode.Open)) as Unites;

            pactList = new SortedList<ushort, UnitesUnit>(pactUnits.Unit.Length);

            foreach ( var unit in pactUnits.Unit )
            {
                pactList.Add(unit.ShowRoomID, unit);
            }
        }

        public UnitesUnit GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            if ( coalition == CoalitionEnum.NATO )
            {
                UnitesUnit unit;
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
                UnitesUnit unit;
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
