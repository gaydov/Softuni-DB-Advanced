namespace HospitalDbExtended.Utilities
{
    public static class InfoMessages
    {
        public const string DoctorRegisteredSuccessfully = "Doctor \"{0}\" with specialty \"{1}\" was registered successfully with username \"{2}\".";
        public const string ExtractedCollectionEmpty = "There are no {0} in the database.";
        public const string ExtractedDoctorCollectionEmpty = "You don't have any {0}.";
        public const string ExtractedEntityCollectionIndicator = "Your {0}:";
        public const string SuccessfullyAddedPatient = "Successfully addded patient:";
        public const string SuccessfullyPrescribedMedication = "Medicament \"{0}\" was prescribed to \"{1} {2}\".";
        public const string SuccessfullyAddedVisitation = "Visitation on {0:dd/MM/yyyy HH:mm} for patient \"{1}\" \"{2}\" was added successfully.";
        public const string SuccessfullyAddedDiagnose = "Diagnose with name \"{0}\" was added for patient \"{1} {2}\".";
    }
}