namespace ChallengeMeLiServices.Web.Models
{
    /// <summary>
    /// Human DTO.
    /// </summary>
    public class HumanV1Dto
    {
        /// <summary>
        /// DTO version.
        /// </summary>
        public int Version { get { return 1; } }

        /// <summary>
        /// Dna chain.
        /// </summary>
        public string[] Dna { get; set; }
    }
}