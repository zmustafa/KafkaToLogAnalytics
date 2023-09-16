using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KafkaToLogAnalyticsFunction
{
    public class LogAnalyticsUtility
    {
        public static async Task SendLogEntries(string entityAsJson, string logType, string workspaceId,
            string apiVersion, string sharedKey)
        {
            using (var httpClient = new HttpClient())
            {
                var RequestBaseUrl =
                    $"https://{workspaceId}.ods.opinsights.azure.com/api/logs?api-version={apiVersion}";
                var dateTimeNow = DateTime.UtcNow.ToString("r");
                var authSignature = GetAuthSignature(entityAsJson, dateTimeNow, workspaceId, sharedKey);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", authSignature);
                httpClient.DefaultRequestHeaders.Add("Log-Type", logType);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("x-ms-date", dateTimeNow);
                httpClient.DefaultRequestHeaders.Add("time-generated-field",
                    "");

                HttpContent httpContent = new StringContent(entityAsJson, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpClient.PostAsync(new Uri(RequestBaseUrl), httpContent).ConfigureAwait(false);

                var responseContent = response.Content;
                var result = await responseContent.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private static string GetAuthSignature(string serializedJsonObject, string dateString, string workspaceId, string sharedKey)
        {
            var stringToSign =
                $"POST\n{serializedJsonObject.Length}\napplication/json\nx-ms-date:{dateString}\n/api/logs";
            string signedString;

            var encoding = new ASCIIEncoding();
            var sharedKeyBytes = Convert.FromBase64String(sharedKey);
            var stringToSignBytes = encoding.GetBytes(stringToSign);
            using (var hmacsha256Encryption = new HMACSHA256(sharedKeyBytes))
            {
                var hashBytes = hmacsha256Encryption.ComputeHash(stringToSignBytes);
                signedString = Convert.ToBase64String(hashBytes);
            }

            return $"SharedKey {workspaceId}:{signedString}";
        }
    }
}