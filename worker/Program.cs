using NotificationWorker;
using NotificationWorker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
