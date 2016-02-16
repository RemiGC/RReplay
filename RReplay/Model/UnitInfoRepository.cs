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

            string exe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exePath = System.IO.Path.GetDirectoryName(exe);

            if ( !String.IsNullOrEmpty(unit.classNameDebug) )
            {
                unit.imagePath = Path.Combine(exePath, "UnitsIcons", unit.classNameDebug + ".jpg");
            }
            else
            {
                unit.imagePath = Path.Combine(exePath, "UnitsIcons", "NoImage.jpg");
            }

            return unit;
        }
    }
}
