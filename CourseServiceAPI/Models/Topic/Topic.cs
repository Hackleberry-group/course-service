﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Helpers;

namespace CourseServiceAPI.Models.Topic;

public class Topic : ITableEntity
{
    public string PartitionKey { get; set; }

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid ModuleId { get; set; }

    public int Order { get; set; }

    public Topic()
    {
        PartitionKey = EntityConstants.TopicPartitionKey;
        Id = Guid.NewGuid();
        RowKey = Id.ToString();
    }
}