namespace MagicVilla_VillaAPI.Logging
{
    public interface ILogging
    {
        public void Log(string message, LogType type);
    }
    public enum LogType
    {
        Error,
        Warning,
        Information
    }
}
