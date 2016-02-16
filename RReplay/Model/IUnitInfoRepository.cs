using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay.Model
{
    public interface IUnitInfoRepository
    {
        UnitesUnit GetUnit( CoalitionEnum coalition, ushort unitID );
    }
}
