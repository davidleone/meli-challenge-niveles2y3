using System;

namespace ChallengeMeLiServices.DataAccess.Models
{
    /// <summary>
    /// Persistable Entity for DNA.
    /// </summary>
    public class Dna
    {
        /// <summary>
        /// Dna's ID (primary key in database).
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Dna chain (unique key in database).
        /// </summary>
        public virtual string[] Chain { get; set; }

        /// <summary>
        /// true: is Mutant | false: is Human.
        /// </summary>
        public virtual bool IsMutant { get; set; }
    }
}
