using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Network.JSON
{
    [Serializable]
    public class UrlDict
    {
        public List<BaseIpToUrl> ips = new List<BaseIpToUrl>();

        public void add(BaseIpToUrl ip) {
            Boolean a = true;
            foreach (BaseIpToUrl ip2 in ips)
            {
                if (ip2.Equals(ip)) {
                    a = false;
                    if (ip2.url != ip.url)
                    {
                        ip2.url = ip.url;
                        
                    }
                    break;
                }

            }
            if(a)
            {
                ips.Add(ip);
            }
            
        }

        public BaseIpToUrl get(string baseIp)
        {
            foreach (BaseIpToUrl ip in ips)
            {
                if(ip.baseIp == baseIp) return ip;

            }
            return null;

        }
    }
}
