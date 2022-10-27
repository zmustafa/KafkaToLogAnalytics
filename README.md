# KaftaToLogAnalytics

This Azure Function allows you to stream Kafka messages to Azure Log Analytics workspace

These settings shoudl be specified

    "TopicName": "%% kafka topic name",
    "KafkaBroker": "%% kafka broker fqdn for example xx.southcentralus.azure.confluent.cloud:9092",
    "KafkaUsername": "%% kafka user name",
    "KafkaPassword": "%% kafka password",
    "LogAnalyticsWorkspaceId": "%% log analytics workspace id, usually a guid",
    "LogAnalyticsSharedKey": "%% log analytics shared key, usually a base64 long string",
    "LogAnalyticsTableName": "%% custom table that will be created to store messages coming from kafka"
