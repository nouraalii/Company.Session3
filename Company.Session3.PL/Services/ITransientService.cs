namespace Company.Session3.PL.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
