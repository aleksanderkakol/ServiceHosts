using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace Hosts
{
    public partial class Hosts : ServiceBase
    {
        Log log = new Log();
        Timer _timer;

        public Hosts()
        {
            InitializeComponent();
            _timer = new Timer(OnElapsedTime, null, Timeout.Infinite, Timeout.Infinite);
        }

        protected int Seconds()
        {
            Int32 defaultInterval = 180000;
            try
            {
                string seconds = ConfigurationManager.AppSettings.Get("Seconds");
                int interval = Int32.Parse(seconds) * 1000;
                return interval;
            } catch (Exception ex)
            {
                log.WriteToFile(ex.Message, "error");
                return defaultInterval;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                int interval = Seconds();
                log.WriteToFile("Started");
                _timer.Change(1000, Timeout.Infinite);
            } catch (Exception ex)
            {
                log.WriteToFile(ex.Message, "error");
            }
        }

        protected override void OnStop()
        {
            log.WriteToFile("Stopped");
            _timer.Dispose();
            _timer = null;
        }
        protected void OnElapsedTime(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            string primaryIP = ConfigurationManager.AppSettings.Get("PrimaryIP");
            string secondaryIP = ConfigurationManager.AppSettings.Get("SecondaryIP");

            using (Connection connection = new Connection())
            {
                using (File file = new File())
                {
                    if (!connection.Check(primaryIP) && connection.Check(secondaryIP))
                    {
                        file.Change(primaryIP, secondaryIP);
                    }
                    else if (connection.Check(primaryIP))
                    {
                        file.Change(secondaryIP, primaryIP);
                    }
                }
            }
            int interval = Seconds();
            _timer.Change(interval, Timeout.Infinite);
        }
    }
}
