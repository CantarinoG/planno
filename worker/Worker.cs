using System.Text.Json;
using Confluent.Kafka;
using NotificationWorker.Services;

namespace NotificationWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IEmailService emailService)
    {
        _logger = logger;
        _configuration = configuration;
        _emailService = emailService;
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
                        
                        await ProcessNotification(result.Message.Value);
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

    private async Task ProcessNotification(string messageValue)
    {
        try
        {
            var data = JsonSerializer.Deserialize<JsonElement>(messageValue);
            var action = data.GetProperty("Action").GetString();
            var title = data.GetProperty("Title").GetString();
            var id = data.GetProperty("EventId").GetString();
            var email = data.TryGetProperty("Email", out var emailProp) ? emailProp.GetString() : "Unknown Email";

            _logger.LogInformation("🔔 NOTIFICATION: Event '{title}' (ID: {id}) was {action}! Target: {email}", title, id, action, email);

            if (email != "Unknown Email")
            {
                var (color, icon) = action.ToUpper() switch
                {
                    "CREATED" => ("#10b981", "🆕"), // Emerald
                    "UPDATED" => ("#f59e0b", "📝"), // Amber
                    "DELETED" => ("#ef4444", "🗑️"), // Red
                    _ => ("#6366f1", "🔔")          // Indigo
                };

                var subject = $"{icon} LetsPlan: Event {action.ToLower()}";
                var body = $@"
<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f3f4f6; padding: 40px 20px; color: #1f2937;"">
    <div style=""max-width: 500px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);"">
        <div style=""background-color: {color}; padding: 20px; text-align: center;"">
            <h1 style=""color: #ffffff; margin: 0; font-size: 24px;"">{icon} LetsPlan</h1>
        </div>
        <div style=""padding: 30px;"">
            <h2 style=""margin-top: 0; color: #111827; font-size: 20px;"">Event {action.ToLower()}!</h2>
            <p style=""font-size: 16px; line-height: 1.5; color: #4b5563;"">
                Hello! We're letting you know that your event <strong>""{title}""</strong> has been successfully <strong>{action.ToLower()}</strong>.
            </p>
            <div style=""margin-top: 25px; padding: 15px; background-color: #f9fafb; border-left: 4px solid {color}; border-radius: 4px;"">
                <p style=""margin: 0; font-size: 14px; color: #6b7280;""><strong>Event Title:</strong> {title}</p>
                <p style=""margin: 5px 0 0 0; font-size: 12px; color: #9ca3af;""><strong>Internal ID:</strong> {id}</p>
            </div>
        </div>
        <div style=""background-color: #f9fafb; padding: 20px; text-align: center; border-top: 1px solid #e5e7eb;"">
            <p style=""margin: 0; font-size: 12px; color: #9ca3af;"">Sent by letsplan.com &bull; {DateTime.UtcNow:yyyy}</p>
        </div>
    </div>
</div>";
                
                await _emailService.SendEmailAsync(email, subject, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error parsing notification payload: {message}", ex.Message);
        }
    }
}
