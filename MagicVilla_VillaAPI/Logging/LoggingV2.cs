namespace MagicVilla_VillaAPI.Logging
{
    public class LoggingV2 : ILogging
    {
        public void Log(string message, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error - {message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;

                case LogType.Warning:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Error - {message}");
                    
                    break;

                case LogType.Information:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(message);
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        
            
        
        }
    }

   
}
