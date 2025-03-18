namespace Company.Session3.PL.Services
{
    public interface IScopedService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
