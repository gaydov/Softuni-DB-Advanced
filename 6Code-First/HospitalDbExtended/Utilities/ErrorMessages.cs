namespace HospitalDbExtended.Utilities
{
    public static class ErrorMessages
    {
        public const string NoCommandEntered = "No command was entered. Please enter a valid command.";
        public const string UsernameAlreadyExists = "Username \"{0}\" already exists. Please choose another username.";
        public const string InvalidCredentials = "Invalid credentials.";
        public const string InvalidCommand = "Invalid command. Please try again.";
        public const string PatientWithIdNotFound = "No patient of yours with ID {0} was found.";
        public const string PatientsWithNameNotFound = "No patient of yours with name {0} was found.";
        public const string PatientNotVisitedByThisDoctor = "The patient has not been visited by you so a {0} is not possible.";
        public const string InvalidIntegerInput = "Invalid input. Please enter an integer number.";
        public const string InvalidFormattedDateInput = "Invalid input. Please enter date in the format {0}.";
        public const string EmptyInputString = "No text entered. Please enter a valid text.";
        public const string InvalidBoolInput = "Invalid input. Please enter \"Y\" or \"N\".";
        public const string MedicamentNotFound = "Medicament with name \"{0}\" was not found.";
        public const string PrescriptionAlreadyExists = "Medicament \"{0}\" has already been prescribed to \"{1}\" \"{2}\"";
    }
}