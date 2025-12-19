using PKISharp.WACS.Clients.DNS;
using PKISharp.WACS.Plugins.Base.Capabilities;
using PKISharp.WACS.Plugins.Interfaces;
using PKISharp.WACS.Plugins.ValidationPlugins;
using PKISharp.WACS.Services;
using wacs.validation.dns.joker.Joker;

namespace wacs.validation.dns.joker
{
    [IPlugin.Plugin1<JokerOptions, JokerOptionsFactory,
    DnsValidationCapability, JokerJson, JokerArguments>(
        "40421038-9975-46F7-8E44-22762CD569CE", "Joker", "Create verification records in Joker DNS.",
        External = true)]
    internal class JokerDnsValidation(
        LookupClientProvider dnsClient,
        ILogService logService,
        ISettings settings,
        DomainParseService domainParser,
        JokerOptions options,
        SecretServiceManager ssm,
        IProxyService proxyService) : DnsValidation<JokerDnsValidation>(dnsClient, logService, settings)
    {
        private readonly DnsManagementClient _client = new(
            ssm.EvaluateSecret(options.Username).Result ?? "",
            ssm.EvaluateSecret(options.Password).Result ?? "",
            logService,
            proxyService);

        public override async Task<bool> CreateRecord(DnsValidationRecord record)
        {
            try
            {
                var domain = domainParser.GetRegisterableDomain(record.Authority.Domain);
                var recordName = RelativeRecordName(domain, record.Authority.Domain);
                await _client.ReplaceRecord(domain, recordName, RecordType.TXT, record.Value);
                return true;
            }
            catch (Exception ex)
            {
                _log.Warning(ex, $"Unable to create record at Joker");
                return false;
            }
        }

        public override async Task DeleteRecord(DnsValidationRecord record)
        {
            try
            {
                var domain = domainParser.GetRegisterableDomain(record.Authority.Domain);
                var recordName = RelativeRecordName(domain, record.Authority.Domain);
                await _client.ReplaceRecord(domain, recordName, RecordType.TXT, "");
            }
            catch (Exception ex)
            {
                _log.Warning(ex, $"Unable to delete record at Joker");
            }
        }
    }
}
