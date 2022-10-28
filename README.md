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

    "TopicName": "%% kafka topic name",
    "KafkaBroker": "%% kafka broker fqdn for example xx.southcentralus.azure.confluent.cloud:9092",
    "KafkaUsername": "%% kafka user name",
    "KafkaPassword": "%% kafka password",
    "LogAnalyticsWorkspaceId": "%% log analytics workspace id, usually a guid",
    "LogAnalyticsSharedKey": "%% log analytics shared key, usually a base64 long string",
    "LogAnalyticsTableName": "%% custom table that will be created to store messages coming from kafka"

## Optional Settings
In order to adjust the frequency of ingestion, these two settings can be modified in the host.json. More details are [here](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-kafka?tabs=in-process%2Cportal&pivots=programming-language-csharp#hostjson-settings).

    "maxBatchSize": 256,
    "SubscriberIntervalInSeconds": 30,
