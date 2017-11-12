namespace HospitalDbExtended.Data.Commands
{
    public abstract class Command
    {
        public abstract void Execute(HospitalContext context, bool startup, bool logged);
    }
}