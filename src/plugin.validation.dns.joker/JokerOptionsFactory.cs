using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKISharp.WACS;
using PKISharp.WACS.Configuration;
using PKISharp.WACS.Plugins.Base.Factories;
using PKISharp.WACS.Services;
using PKISharp.WACS.Services.Serialization;

namespace wacs.validation.dns.joker
{
    internal class JokerOptionsFactory(ArgumentsInputService arguments) : PluginOptionsFactory<JokerOptions>
    {
        private ArgumentResult<ProtectedString?> Username => arguments
            .GetProtectedString<JokerArguments>(a => a.Username).Required();
        private ArgumentResult<ProtectedString?> Password => arguments
            .GetProtectedString<JokerArguments>(a => a.Password).Required();

        public override async Task<JokerOptions?> Aquire(IInputService input, RunLevel runLevel)
        {
            return new JokerOptions()
            {
                Username = await Username.Interactive(input).GetValue(),
                Password = await Password.Interactive(input).GetValue()
            };
        }

        public override async Task<JokerOptions?> Default()
        {
            return new JokerOptions()
            {
                Username = await Username.GetValue(),
                Password = await Password.GetValue()
            };
        }

        public override IEnumerable<(CommandLineAttribute, object?)> Describe(JokerOptions options)
        {
            yield return (Username.Meta, options.Username);
            yield return (Password.Meta, options.Password);
        }
    }
}
