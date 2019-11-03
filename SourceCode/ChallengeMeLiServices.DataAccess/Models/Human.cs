namespace ChallengeMeLiServices.DataAccess.Models
{
    /// <summary>
    /// Non-persistable entity. Human Entity.
    /// </summary>
    public class Human
    {
        /// <summary>
        /// Dna chain.
        /// </summary>
        public virtual string[] Dna { get; set; }
    }
}
