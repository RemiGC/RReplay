namespace RReplay.Model
{
    public interface IUnitInfoRepository
    {
        UnitInfo GetUnit( CoalitionEnum coalition, ushort unitID );
    }
}
