# Stream Kafka messages to Azure Log Analytics workspace
This Azure Function allows you to stream Kafka (Apache Kafka, Confluent Kafka, etc) messages to Azure Log Analytics workspace.

The messages are read from the specified topic in Kafka and then sent to a specified Custom Log table in Azure Log Analytics workspace table.

## How it works
Azure Function creates a Kafka based trigger and wait for messages. Once the mesage arrives, it connects to Azure Log Analytics Workspace using Data Collector API to upload the messages. The messages are batched together based on batching setting specified below. 

## Documentation
Documentation on [Apache Kafka trigger for Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-kafka-trigger)

Documentation on [Log Analytics Data Collector API](https://learn.microsoft.com/en-us/rest/api/loganalytics/create-request)

## Mandatory Settings
These settings should be specified in Configuration of Azure Function

    "TopicName1": "Topic1",
    "TopicName2": "Topic2",
    "TopicName3": "Topic3",
    "TopicName4": "Topic4",
    "TopicName5": "Topic5",
    "KafkaBroker": "1.1.1.1:9092",
    "LogAnalyticsWorkspaceId": "xxx",
    "LogAnalyticsSharedKey": "xxxx",
    "LogAnalyticsTableName": "LogAnalyticsTableName",
    "AzureWebJobs.KafkaToLogAnalytics1.Disabled": false,
    "AzureWebJobs.KafkaToLogAnalytics2.Disabled": true,
    "AzureWebJobs.KafkaToLogAnalytics3.Disabled": true,
    "AzureWebJobs.KafkaToLogAnalytics4.Disabled": true,
    "AzureWebJobs.KafkaToLogAnalytics5.Disabled": true

## Optional Settings
In order to adjust the frequency of ingestion, these settings can be modified in the host.json. More details are [here](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-kafka?tabs=in-process%2Cportal&pivots=programming-language-csharp#hostjson-settings).

      "maxBatchSize": 256,
      "SubscriberIntervalInSeconds": 30,
      "ExecutorChannelCapacity": 1,
      "ChannelFullRetryIntervalInMs": 50
