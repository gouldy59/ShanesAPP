using ILogger = ShanesAPP.Infrastructure.Interfaces.ILogger;

namespace ShanesAPP.Infrastructure
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
