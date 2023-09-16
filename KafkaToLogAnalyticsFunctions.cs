using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// documentation https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-kafka?tabs=in-process%2Cportal&pivots=programming-language-csharp

namespace KafkaToLogAnalyticsFunction
{
    public class KafkaToLogAnalyticsFunctions
    {
        private static string ApiVersion = "2016-04-01";
        private static string WorkspaceId = Environment.GetEnvironmentVariable("LogAnalyticsWorkspaceId");
        private static string SharedKey = Environment.GetEnvironmentVariable("LogAnalyticsSharedKey");
        private static string LogAnalyticsTableName = Environment.GetEnvironmentVariable("LogAnalyticsTableName");




        [FunctionName("KafkaToLogAnalytics1")]
        public static async Task KafkaToLogAnalytics1(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName1%",
                Protocol = BrokerProtocol.Plaintext,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            CheckIfValuesAreSet("TopicName1");

            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await LogAnalyticsUtility
                .SendLogEntries(eventsData, LogAnalyticsTableName, WorkspaceId, ApiVersion, SharedKey)
                .ConfigureAwait(false);
        }

        [FunctionName("KafkaToLogAnalytics2")]
        public static async Task KafkaToLogAnalytics2(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName2%",
                Protocol = BrokerProtocol.Plaintext,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            CheckIfValuesAreSet("TopicName2");
            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await LogAnalyticsUtility
                .SendLogEntries(eventsData, LogAnalyticsTableName, WorkspaceId, ApiVersion, SharedKey)
                .ConfigureAwait(false);
        }

        [FunctionName("KafkaToLogAnalytics3")]
        public static async Task KafkaToLogAnalytics3(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName3%",
                Protocol = BrokerProtocol.Plaintext,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            CheckIfValuesAreSet("TopicName3");
            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await LogAnalyticsUtility
                .SendLogEntries(eventsData, LogAnalyticsTableName, WorkspaceId, ApiVersion, SharedKey)
                .ConfigureAwait(false);
        }

        [FunctionName("KafkaToLogAnalytics4")]
        public static async Task KafkaToLogAnalytics4(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName4%",
                Protocol = BrokerProtocol.Plaintext,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            CheckIfValuesAreSet("TopicName4");
            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await LogAnalyticsUtility
                .SendLogEntries(eventsData, LogAnalyticsTableName, WorkspaceId, ApiVersion, SharedKey)
                .ConfigureAwait(false);
        }

        [FunctionName("KafkaToLogAnalytics5")]
        public static async Task KafkaToLogAnalytics5(
            [KafkaTrigger("%KafkaBroker%",
                "%TopicName5%",
                Protocol = BrokerProtocol.Plaintext,
                ConsumerGroup = "$Default")]
            KafkaEventData<string>[] events, ILogger log)
        {
            CheckIfValuesAreSet("TopicName5");
            var eventsData = "[" + string.Join(",", events.Select(eventData => eventData.Value).ToList()) + "]";
            Console.WriteLine($"Sending {events.Count()} events.");
            await LogAnalyticsUtility
                .SendLogEntries(eventsData, LogAnalyticsTableName, WorkspaceId, ApiVersion, SharedKey)
                .ConfigureAwait(false);
        }

        private static void CheckIfValuesAreSet(string topicName)
        {
            if (LogAnalyticsTableName == null)
            {
                throw new MissingFieldException("LogAnalyticsTableName is empty");
            }

            if (WorkspaceId == null)
            {
                throw new MissingFieldException("LogAnalyticsWorkspaceId is empty");
            }

            if (SharedKey == null)
            {
                throw new MissingFieldException("LogAnalyticsSharedKey is empty");
            }

            if (Environment.GetEnvironmentVariable("KafkaBroker") == null)
            {
                throw new MissingFieldException("KafkaBroker is empty");
            }

            if (Environment.GetEnvironmentVariable(topicName) == null)
            {
                throw new MissingFieldException(topicName + " is empty");
            }
        }
    }
}