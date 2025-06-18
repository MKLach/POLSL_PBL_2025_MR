using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Oculus.Interaction.OptionalAttribute;
using UnityEngine.InputSystem;

namespace Assets.Network.JSON
{
    [Serializable]
    public class BaseIpToUrl
    {
        public string baseIp;
        public string url;

        public bool wasLastConnecctionSuccessfull;

        public BaseIpToUrl(string baseIp, string url, bool wasLastConnecctionSuccessfull)
        {
            this.baseIp = baseIp;
            this.url = url;
            this.wasLastConnecctionSuccessfull = wasLastConnecctionSuccessfull;
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseIpToUrl other)
            {
                return baseIp == other.baseIp;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return baseIp.GetHashCode();
        }
    }
}
