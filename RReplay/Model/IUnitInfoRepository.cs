namespace RReplay.Model
{
    public interface IUnitInfoRepository
    {
        TUniteAuSol GetUnit( CoalitionEnum coalition, ushort unitID );
        Era GetEra( byte eraId );
        Specialization GetSpecialization( byte specializationId );
        Nations GetNation( CoalitionEnum coalition, byte nationId );
    }

    public enum CoalitionEnum
    {
        NATO = 0x0,
        PACT = 0x1
    }
}
