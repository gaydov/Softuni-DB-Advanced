namespace Mankind.Utilities
{
    public static class ErrorMessages
    {
        // Human messages
        public const string NameInvalid = "Expected upper case letter! Argument: {0}";
        public const string NameMinLenghtInvalid = "Expected length at least {0} symbols! Argument: {1}";

        // Student messages
        public const string StudentInvalidFacultyNumber = "Invalid faculty number!";

        // Worker messages
        public const string WorkerInvalidSalary = "Expected value mismatch! Argument: {0}";
        public const string WorkerInvalidWorkHours = "Expected value mismatch! Argument: {0}";
    }
}