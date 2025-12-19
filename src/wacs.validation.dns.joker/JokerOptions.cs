using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PKISharp.WACS.Plugins.Base.Options;
using PKISharp.WACS.Services.Serialization;

namespace wacs.validation.dns.joker
{
    [JsonSerializable(typeof(JokerOptions))]
    internal partial class JokerJson : JsonSerializerContext
    {
        public JokerJson(WacsJsonPluginsOptionsFactory optionsFactory) : base(optionsFactory.Options) { }
    }

    internal class JokerOptions : ValidationPluginOptions
    {
        public ProtectedString? Username { get; set; }
        public ProtectedString? Password { get; set; }
    }
}
