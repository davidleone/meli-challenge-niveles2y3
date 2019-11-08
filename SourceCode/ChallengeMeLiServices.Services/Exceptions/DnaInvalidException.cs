using System;

namespace ChallengeMeLiServices.Services.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the dna chain provided to a method is not valid.
    /// </summary>
    [Serializable]
    public class DnaInvalidException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the DnaInvalidException class with a specified error message.
        /// To simplify this challenges (level 2 and 3) I will handle only 1 error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DnaInvalidException() : base("DNA is invalid!") { }
    }
}
