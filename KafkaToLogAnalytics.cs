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

// documentation https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-kafka?tabs=in-process%2Cportal&pivots=programming-language-csharp

namespace KafkaToLogAnalyticsFunction
{
    public class KafkaToLogAnalytics
    {
        private static string ApiVersion = "2016-04-01";
        private static HttpClient httpClient;
        private static string WorkspaceId = Environment.GetEnvironmentVariable("LogAnalyticsWorkspaceId");
        private static string SharedKey = Environment.GetEnvironmentVariable("LogAnalyticsSharedKey");
        private static string LogAnalyticsTableName = Environment.GetEnvironmentVariable("LogAnalyticsTableName");

        private static string RequestBaseUrl { get; set; }


       [FunctionName("KafkaToLogAnalytics")]
        public static async Task Run(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName%",
                Protocol = BrokerProtocol.Plaintext,
                AuthenticationMode = BrokerAuthenticationMode.Plain,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await SendLogEntries(eventsData, LogAnalyticsTableName).ConfigureAwait(false);
        }


        public static async Task SendLogEntries(string entityAsJson, string logType)
        {
            httpClient = new HttpClient();
            RequestBaseUrl = $"https://{WorkspaceId}.ods.opinsights.azure.com/api/logs?api-version={ApiVersion}";
            var dateTimeNow = DateTime.UtcNow.ToString("r");
            var authSignature = GetAuthSignature(entityAsJson, dateTimeNow);
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

        private static string GetAuthSignature(string serializedJsonObject, string dateString)
        {
            var stringToSign =
                $"POST\n{serializedJsonObject.Length}\napplication/json\nx-ms-date:{dateString}\n/api/logs";
            string signedString;

            var encoding = new ASCIIEncoding();
            var sharedKeyBytes = Convert.FromBase64String(SharedKey);
            var stringToSignBytes = encoding.GetBytes(stringToSign);
            using (var hmacsha256Encryption = new HMACSHA256(sharedKeyBytes))
            {
                var hashBytes = hmacsha256Encryption.ComputeHash(stringToSignBytes);
                signedString = Convert.ToBase64String(hashBytes);
            }

            return $"SharedKey {WorkspaceId}:{signedString}";
        }
    }
}
