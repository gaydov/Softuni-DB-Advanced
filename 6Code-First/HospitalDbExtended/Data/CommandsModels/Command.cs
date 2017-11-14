using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Data.CommandsModels
{
    public abstract class Command
    {
        private readonly IReader reader;
        private readonly IWriter writer;
        private readonly HospitalContext context;
        private bool isLogged;
        private int loggedDoctorId;

        protected Command(HospitalContext context, bool isLogged, int loggedDoctorId, IReader reader, IWriter writer)
        {
            this.context = context;
            this.IsLogged = isLogged;
            this.LoggedDoctorId = loggedDoctorId;
            this.reader = reader;
            this.writer = writer;
        }

        public bool IsLogged
        {
            get { return this.isLogged; }

            set { this.isLogged = value; }
        }

        public int LoggedDoctorId
        {
            get { return this.loggedDoctorId; }

            set { this.loggedDoctorId = value; }
        }

        protected HospitalContext Context
        {
            get { return this.context; }
        }
        
        protected IReader Reader
        {
            get { return this.reader; }
        }

        protected IWriter Writer
        {
            get { return this.writer; }
        }

        public abstract void Execute();
    }
}