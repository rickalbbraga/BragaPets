namespace BragaPets.Domain.Notifications
{
    public class Notification
    {
        public string Code { get; }
        public string Message { get; }

        public Notification(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}