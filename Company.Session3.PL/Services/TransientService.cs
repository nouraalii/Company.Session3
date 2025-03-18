namespace Company.Session3.PL.Services
{
    public class TransientService : ITransientService
    {
        public TransientService()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; set; }

        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
