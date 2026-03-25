using System.Text.Json;
using Confluent.Kafka;

namespace NotificationWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["KafkaSettings:BootstrapServers"] ?? "kafka:9092",
            GroupId = _configuration["KafkaSettings:GroupId"] ?? "notification-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        var topic = _configuration["KafkaSettings:EventsTopic"] ?? "calendar-events";

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        _logger.LogInformation("Worker started, listening on topic {topic}...", topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result != null)
                    {
                        _logger.LogInformation("Received message: Key={key}, Value={value}", result.Message.Key, result.Message.Value);
                        
                        ProcessNotification(result.Message.Value);
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error occurred: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown
        }
        finally
        {
            consumer.Close();
        }
    }

    private void ProcessNotification(string messageValue)
    {
        try
        {
            var data = JsonSerializer.Deserialize<JsonElement>(messageValue);
            var action = data.GetProperty("Action").GetString();
            var title = data.GetProperty("Title").GetString();
            var id = data.GetProperty("EventId").GetString();
            var email = data.TryGetProperty("Email", out var emailProp) ? emailProp.GetString() : "Unknown Email";

            _logger.LogInformation("🔔 NOTIFICATION: Event '{title}' (ID: {id}) was {action}! Target: {email}", title, id, action, email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error parsing notification payload: {message}", ex.Message);
        }
    }
}
