using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Vdc.Pos.Business.Utilities
{
    public static class Utility
    { 
        public static bool IsConnectedToInternet()
            {
                try
                {
                    using (Ping ping = new Ping())
                    {
                        PingReply reply = ping.Send("www.google.com");
                        return reply.Status == IPStatus.Success;
                    }
                }
                catch
                {
                    return false; 
                }
            }
        }
    }

