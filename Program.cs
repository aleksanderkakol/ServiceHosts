using System.ServiceProcess;

namespace Hosts
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Hosts()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
