namespace Musuko.Framework.DataModels
{
    public class ServiceReturn
    {
        public string Message { get; set; }
        public string ReturnedId { get; set; }
        public bool Success { get; set; } = true;
    }
}
