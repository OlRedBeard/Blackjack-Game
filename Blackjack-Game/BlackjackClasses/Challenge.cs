using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    [Serializable]
    public class Challenge
    {
        public string Issuer { get; set; }
        public string Recipient { get; set; }
        public string IssuerIP { get; set; }

        public Challenge(string issuer, string recipient, string ip)
        {
            this.Issuer = issuer;
            this.Recipient = recipient;
            this.IssuerIP = ip;
        }

        public override string ToString()
        {
            return Issuer;
        }
    }
}
