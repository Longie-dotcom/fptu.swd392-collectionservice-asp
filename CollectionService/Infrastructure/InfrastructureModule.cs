using Application.Helper;
using Application.Interface.IGrpcClient;
using Application.Interface.IPublisher;
using Domain.IRepository;
using Infrastructure.Grpc;
using Infrastructure.InfrastructureException;
using Infrastructure.MessageBroker.Publisher;
using Infrastructure.Messaging.Consumer;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureModule
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddInfrastructure(
                this IServiceCollection services)
        {
            ServiceLogger.Warning(
                Level.Infrastructure, "Starting Infrastructure configuration");

            // ======================
            // 1. Database
            // ======================
            try
            {
                ServiceLogger.Warning(
                    Level.Infrastructure, "Configuring SQL Server database connection");

                // Configure the database connection
                var connectionString = Environment.GetEnvironmentVariable("COLLECTION_DB_CONNECTION");
                if (string.IsNullOrEmpty(connectionString))
                {
                    ServiceLogger.Error(
                        Level.Infrastructure, "Missing environment variable: COLLECTION_DB_CONNECTION");
                    throw new DatabaseConnectionException(
                        "Failed to configure Collection database.");
                }

                services.AddDbContext<CollectionDBContext>(options =>
                    options.UseSqlServer(connectionString));

                // Register repositories + Unit of Work
                services.AddScoped<IAuditLogRepository, AuditLogRepository>();
                services.AddScoped<ICollectorProfileRepository, CollectorProfileRepository>();

                services.AddScoped<IUnitOfWork, UnitOfWork>();

                ServiceLogger.Logging(
                    Level.Infrastructure, "Database and repositories configured successfully.");
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"Failed to configure Collection database: {ex.Message}");
                throw new DatabaseConnectionException(
                    "Failed to configure Collection database.");
            }

             //======================
             //2.RabbitMQ
             //======================
            try
            {
                ServiceLogger.Warning(
                    Level.Infrastructure, "Configuring RabbitMQ connection");

                services.AddMassTransit(x =>
                {
                    // Add all consumers for this service
                    x.AddConsumer<UserDeleteConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
                        var rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER");
                        var rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS");

                        if (
                        string.IsNullOrEmpty(rabbitHost) 
                        || string.IsNullOrEmpty(rabbitUser) 
                        || string.IsNullOrEmpty(rabbitPassword))
                        {
                            ServiceLogger.Error(
                                Level.Infrastructure, "Missing RabbitMQ environment variables");
                            throw new MessagingConnectionException("Failed to configure message broker.");
                        }

                        cfg.Host(rabbitHost, "/", h =>
                        {
                            h.Username(rabbitUser);
                            h.Password(rabbitPassword);
                        });

                        cfg.ReceiveEndpoint("collection_delete_consumer", e =>
                        {
                            e.ConfigureConsumer<UserDeleteConsumer>(context);
                        });
                    });
                });

                services.AddScoped<IEmailSendPublisher, EmailSendPublisher>();
                services.AddScoped<ISignalRPublisher, SignalRPublisher>();

                ServiceLogger.Logging(
                    Level.Infrastructure, "RabbitMQ successfully configured.");
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"RabbitMQ configuration failed: {ex.Message}");
                throw new MessagingConnectionException(
                    "Failed to configure RabbitMQ infrastructure.");
            }

             //======================
             //3.gRPC Clients
             //======================
            try
            {
                ServiceLogger.Warning(
                    Level.Infrastructure, $"Configuring gRPC connection.");

                services.AddScoped<IGrpcClient, GrpcClient>();

                ServiceLogger.Logging(
                    Level.Infrastructure, "gRPC configured successfully.");
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"gRPC configuration failed: {ex.Message}");
                throw new GrpcCommunicationException(
                    "Failed to configure gRPC client.");
            }

            return services;
        }
        #endregion
    }
}
