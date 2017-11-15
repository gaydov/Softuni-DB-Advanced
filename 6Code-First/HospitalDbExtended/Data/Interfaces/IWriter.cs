namespace HospitalDbExtended.Data.Interfaces
{
    public interface IWriter
    {
        void WriteLine(string textLine);

        void Write(string textLine);
    }
}