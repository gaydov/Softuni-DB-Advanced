namespace P01_BillsPaymentSystem.Data.CommandsModels
{
    public abstract class Command
    {
        private BillsPaymentSystemContext context;

        protected Command(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public BillsPaymentSystemContext Context
        {
            get { return this.context; }
        }

        public abstract void Execute();
    }
}