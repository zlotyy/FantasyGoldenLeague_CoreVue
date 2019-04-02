using Newtonsoft.Json;
using NLog;
using System;

namespace FantasyGoldenLeague.Utils
{
    public interface IExecutor
    {
        TResult Execute<TResult, TRequest>(TRequest model, Func<TRequest, TResult> func) where TResult : new();
        TResult Execute<TResult>(Func<TResult> func) where TResult : new();
    }

    public class Executor : IExecutor
    {
        readonly ILogger _logger;

        public Executor(ILogger logger)
        {
            _logger = logger;
        }

        public TResult Execute<TResult>(Func<TResult> func) where TResult : new()
        {
            try
            {
                _logger.Trace($"Call: {func.Method.Name}");
                return func();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Call: {MethodName}", func.Method.Name);
            }
            return new TResult();
        }

        public TResult Execute<TResult, TRequest>(TRequest model, Func<TRequest, TResult> func) where TResult : new()
        {
            try
            {
                _logger.Trace($"Call: {func.Method.Name}");
                _logger.Trace($"Params model: {JsonConvert.SerializeObject(model)}");
                return func(model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Call: {MethodName} with model: {model}", func.Method.Name, model);
            }
            return new TResult();
        }
    }
}
