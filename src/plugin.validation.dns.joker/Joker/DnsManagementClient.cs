using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PKISharp.WACS.Services;

namespace wacs.validation.dns.joker.Joker
{
    public class DnsManagementClient(string username, string password, ILogService logService, IProxyService proxyService)
    {
        public async Task ReplaceRecord(string zone, string label, RecordType recordType, string value)
        {
            const string endpointUrl = " https://svc.joker.com/nic/replace";
            
            var requestContent = new FormUrlEncodedContent(
            [
                new ("username", username),
                new ("password", password),
                new ("zone", zone),
                new ("label", label),
                new ("type", recordType.ToString()),
                new ("value", value),
            ]); 

            using (var client = await proxyService.GetHttpClient())
            {
                var res = await client.PostAsync(endpointUrl, requestContent);

                if (res.IsSuccessStatusCode)
                {
                    var responseMessage = await res.Content.ReadAsStringAsync();
                    logService.Verbose($"Joker API request succeeded with HTTP status code: {res.StatusCode}, and response: {responseMessage}");
                }
                else
                {
                    var errorDetail = await res.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Failed to update DNS record. Joker API returned HTTP status code {res.StatusCode.ToString()} and content {errorDetail}");
                }
            }
        }
    }
}
