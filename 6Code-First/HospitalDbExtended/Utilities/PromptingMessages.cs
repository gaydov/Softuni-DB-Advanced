namespace HospitalDbExtended.Utilities
{
    public static class PromptingMessages
    {
        public const string RegisterOrLogin = "Do you want to login or register? (\"help\" to list all commands): ";
        public const string PromptForCommand = "Please enter command (\"help\" to list all commands): ";
        public const string ExitConfirmation = "Are you sure you want to exit? (Y/N): ";
        public const string LogoffConfirmation = "Are you sure you want to logoff? (Y/N): ";
        public const string PatientHasInsurance = "Does the {0} has insurance? (Y/N): ";
        public const string ShouldCollectionEntitiesBeAdded = "Would you like to enter {0} for this patient? (Y/N): ";
        public const string ShouldMoreCollectionEntitiesBeAdded = "Would you like to enter more {0} for this patient? (Y/N): ";
        public const string VisitationDate = "Visitation date (format DD/MM/YYYY HH:MM): ";
    }
}