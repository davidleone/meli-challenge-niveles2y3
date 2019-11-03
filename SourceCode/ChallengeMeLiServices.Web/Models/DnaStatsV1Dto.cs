namespace ChallengeMeLiServices.Web.Models
{
    /// <summary>
    /// Contains the stats between Mutants and Humans.
    /// </summary>
    public class DnaStatsV1Dto
    {
        /// <summary>
        /// Count of verified Mutants.
        /// </summary>
        public int CountMutantDna { get; set; }

        /// <summary>
        /// Count of verified Humans.
        /// </summary>
        public int CountHumanDna { get; set; }

        /// <summary>
        /// Ratio between Mutants and Humans, from 0 to 1.
        /// This represents how many humans are mutants, so 0 means all are humans, and 1 means all are mutants.
        /// </summary>
        public decimal Ratio { get; set; }
    }
}