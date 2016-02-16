using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RReplay.Model
{
    public class UnitInfoRepository : IUnitInfoRepository
    {
        XDocument natoDoc;
        XDocument pactDoc;

        public UnitInfoRepository()
        {
            natoDoc = XDocument.Load("NATO.xml");
            pactDoc = XDocument.Load("PACT.xml");
        }

        public UnitInfo GetUnit( CoalitionEnum coalition, ushort unitID )
        {
            UnitInfo unit = new UnitInfo();
            XDocument xdoc;

            if ( coalition == CoalitionEnum.NATO )
            {
                xdoc = natoDoc;
            }
            else
            {
                xdoc = pactDoc;
            }

            var query = from units in xdoc.Element("Unites").Elements("Unit")
                        where ushort.Parse(units.Attribute("ShowRoomID").Value) == unitID
                        select units;

            XElement oneUnit = query.FirstOrDefault();
            if ( oneUnit != null )
            {
                unit.alias = oneUnit.Element("AliasName").Value.ToString();
                unit.classNameDebug = oneUnit.Element("ClassNameForDebug").Value.ToString();

                int.TryParse(oneUnit.Element("Category").Value, out unit.category);
                int.TryParse(oneUnit.Element("Class").Value, out unit.classNumber);
                int.TryParse(oneUnit.Element("InstanceID").Value, out unit.instanceID);
            }

            string exe = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if ( !String.IsNullOrEmpty(unit.classNameDebug) )
            {
                
                unit.imagePath = Path.Combine(exe, "Icons", "Units" , unit.classNameDebug + ".jpg");
            }

            if( !File.Exists(unit.imagePath) )
            {
                unit.imagePath = Path.Combine(exe, "Icons", "Units", "NoImage.jpg");
            }

            return unit;
        }
    }
}
