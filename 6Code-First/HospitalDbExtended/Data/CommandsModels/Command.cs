using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Data.CommandsModels
{
    public abstract class Command
    {
        private readonly IReader reader;
        private readonly IWriter writer;
        private readonly HospitalContext context;
        private bool isDoctorLogged;
        private int loggedDoctorId;

        protected Command(HospitalContext context, bool isDoctorLogged, int loggedDoctorId, IReader reader, IWriter writer)
        {
            this.context = context;
            this.IsDoctorLogged = isDoctorLogged;
            this.LoggedDoctorId = loggedDoctorId;
            this.reader = reader;
            this.writer = writer;
        }

        public bool IsDoctorLogged
        {
            get { return this.isDoctorLogged; }

            set { this.isDoctorLogged = value; }
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