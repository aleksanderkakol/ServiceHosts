using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Hosts
{
    partial class Connection : Log,IDisposable
    {
        public bool Check(string ip)
        {
            using (Ping pingRequest = new Ping())
            {
                PingReply requestReply = pingRequest.Send(IPAddress.Parse(ip));
                try
                {
                    if (requestReply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            requestReply = pingRequest.Send(IPAddress.Parse(ip));
                            if (requestReply.Status == IPStatus.Success)
                            {
                                return true;
                            }
                            System.Threading.Thread.Sleep(2000);
                        }
                        return false;
                    }
                } catch (Exception ex)
                {
                    WriteToFile("Connection Error, " + ex.Message, "error");
                    return false;
                }
            }
        }
    }
}
