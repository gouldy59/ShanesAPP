namespace ShanesAPP.Infrastructure
{
    public class Constants
    {
        // Scopes
        public const string Scope = "https%3A//www.googleapis.com/auth/userinfo.profile";

        // ErrorMessages
        public const string LogOutErrorMessage = "There was an error logging out";
        public const string TokenErrorMessage = "Token was invalid";
        public const string NoCodeReturned = "No Code Returned";
        public const string IncorrectStateReturned = "Incorrect State returned";
        public const string UnexpectedException = "An unexpected Exception occurred";
    }
}
