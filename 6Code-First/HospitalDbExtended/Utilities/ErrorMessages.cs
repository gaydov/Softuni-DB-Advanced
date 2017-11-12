namespace HospitalDbExtended.Utilities
{
    public static class ErrorMessages
    {
        public const string DoctorAlreadyExists = "A doctor with these name and specialty already exists.";
        public const string InvalidCredentials = "Invalid credentials.";
        public const string InvalidInput = "Invalid input. Please try again.";
        public const string PatientNotFound = "No patient with ID {0} was found.";
        public const string PatientDoesNotHaveVisitations = "The patient have not been visited by you so a diagnose is not possible.";
    }
}