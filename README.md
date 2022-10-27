# KaftaToLogAnalytics

This Azure Function allows you to stream Kafka messages to Azure Log Analytics workspace

These settings should be specified in Configuration of Azure Function

    "TopicName": "%% kafka topic name",
    "KafkaBroker": "%% kafka broker fqdn for example xx.southcentralus.azure.confluent.cloud:9092",
    "KafkaUsername": "%% kafka user name",
    "KafkaPassword": "%% kafka password",
    "LogAnalyticsWorkspaceId": "%% log analytics workspace id, usually a guid",
    "LogAnalyticsSharedKey": "%% log analytics shared key, usually a base64 long string",
    "LogAnalyticsTableName": "%% custom table that will be created to store messages coming from kafka"

In order to adjust the frequency of ingestion, these two settings can be adjusted in the host.json

      "maxBatchSize": 256,
      "SubscriberIntervalInSeconds": 30,
