using System;

namespace ChallengeMeLiServices.Services.Models
{
    /// <summary>
    /// Human Entity.
    /// </summary>
    public class Human
    {
        /// <summary>
        /// Unique human ID.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Dna chain.
        /// </summary>
        public virtual string[] Dna { get; set; }
    }
}
