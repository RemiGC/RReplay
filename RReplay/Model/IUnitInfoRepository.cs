namespace RReplay.Model
{
    /// <summary>
    /// A repository with all the information about the units of the game
    /// </summary>
    public interface IDeckInfoRepository
    {
        /// <summary>
        /// Return a <c>SimpleUnit</c> based on the unique combinaison of <c>CoalitionEnum</c> and <c>unitID</c>
        /// </summary>
        /// <param name="coalition">The coalition of the units</param>
        /// <param name="unitID">The unique show room ID of the unit</param>
        /// <returns>A <c>SimpleUnit</c></returns>
        SimpleUnit GetUnit( CoalitionEnum coalition, ushort unitID );

        /// <summary>
        /// Get the <c>Era</c> description based on the <c>eraID</c>
        /// </summary>
        /// <param name="eraId">The unique ID of the Era</param>
        /// <returns>The era description</returns>
        Era GetEra( byte eraId );

        /// <summary>
        /// Get the <c>Specialization</c> based on the <c>specializationId</c>
        /// </summary>
        /// <param name="specializationId">The unique ID for the specia</param>
        /// <returns>The Specialization description</returns>
        Specialization GetSpecialization( byte specializationId );

        /// <summary>
        /// Return the <c>Nations</c> based on the unique combinaison of <c>CoalitionEnum</c> and <c>nationId</c>
        /// </summary>
        /// <param name="coalition">The coalition of the nation</param>
        /// <param name="nationId">The unique ID of the nation</param>
        /// <returns>The Nation</returns>
        Nations GetNation( CoalitionEnum coalition, byte nationId );
    }

    /// <summary>
    /// An Enum for the two possible coalition in the game
    /// </summary>
    public enum CoalitionEnum
    {
        NATO = 0x0,
        PACT = 0x1
    }
}
