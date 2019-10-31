using System;

namespace ChallengeMeLiServices.Services.Models
{
    public class Human
    {
        public virtual Guid Id { get; set; }
        public virtual string[] Dna { get; set; }
    }
}
