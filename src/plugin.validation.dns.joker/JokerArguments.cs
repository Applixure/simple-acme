using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKISharp.WACS.Configuration;
using PKISharp.WACS.Configuration.Arguments;

namespace wacs.validation.dns.joker
{
    public class JokerArguments : BaseArguments
    {
        public override string Name => "Joker";
        public override string Group => "Validation";

        [CommandLine(Description = "Joker API username", Secret = true)]
        public string? Username { get; set; }

        [CommandLine(Description = "Joker API password", Secret = true)]
        public string? Password { get; set; }
    }
}
