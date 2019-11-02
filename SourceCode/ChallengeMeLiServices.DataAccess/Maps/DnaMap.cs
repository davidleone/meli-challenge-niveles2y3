using ChallengeMeLiServices.DataAccess.Models;
using FluentNHibernate.Mapping;

namespace ChallengeMeLiServices.DataAccess.Maps
{
    /// <summary>
    /// The DNA database mapping.
    /// </summary>
    public class DnaMap : ClassMap<Dna>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnaMap"/> class.
        /// </summary>
        public DnaMap()
        {
            Table("dna");
            Id(x => x.Id, "dna_id");
            Map(x => x.Chain, "chain");
            Map(x => x.IsMutant, "is_mutant");
        }
    }
}
