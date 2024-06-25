using DemoLibrary.Application.CQRS.Blacklist;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DeleteExpiredTokensTask : BackgroundService
{
    private readonly LoggerBase _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DeleteExpiredTokensTask(ILoggerBaseFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = loggerFactory.CreateRabbitMqLogger("Task-DeleteExpiredTokens");
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.WriteLog("Starting execution at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                var deletedTokens = await mediator.Send(new DeleteExpiredBlacklistedCommand());
                deletedTokens.ForEach(token => _logger.WriteLog("Deleted token: " + token.Jti));
            }
            catch (DeleteExpiredBlacklistedTokensException ex)
            {
                ex.deletedTokens.ForEach(token => _logger.WriteLog("Deleted token: " + token.Jti));
                ex.errorTokens.ForEach(token => _logger.WriteLog("Could not delete token: " + token.Jti));
            }
            catch (Exception ex)
            {
                _logger.WriteLog("Error: " + ex.Message);
            }

            // task runs daily
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}