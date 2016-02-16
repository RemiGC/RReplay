using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay.Model
{
    public interface IUnitInfoRepository
    {
        UnitInfo GetUnit( CoalitionEnum coalition, ushort unitID );
    }

    public struct UnitInfo
    {
        public string classNameDebug;
        public string alias;
        public int category;
        public int instanceID;
        public int classNumber;
        public string imagePath;
    }
}
