using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace PollyExtensions
{
    /// <summary>
    ///  Codigos monitorados:
    ///  
    ///  • Network failures (as System.Net.Http.HttpRequestException)
    //   • HTTP 5XX status codes (server errors)
    //   • HTTP 408 status code (request timeout)
    /// </summary>
    public class PollyExtensionsImpl
    {
        private readonly PollyExtensionsSettings _configuration;

        public PollyExtensionsImpl(PollyExtensionsSettings configuration)
        {
            _configuration = configuration ?? new PollyExtensionsSettings().Default();
        }

        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry(Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .RetryAsync(_configuration.RetryCount, onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .OrResult(predicateHandle)
                                       .RetryAsync(_configuration.RetryCount, onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry(Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(_configuration.RetryAttempt), onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .OrResult(predicateHandle)
                                       .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(_configuration.RetryAttempt), onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry(Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(Math.Pow(2, _configuration.RetryAttempt)), onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null)
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .OrResult(predicateHandle)
                                       .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(Math.Pow(2, _configuration.RetryAttempt)), onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy GenericLinearHttpRetry<T>(Action doOnRetry = null) where T : Exception
        {
            return Policy.Handle<T>()
                         .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(_configuration.RetryAttempt), onRetry: (a, b) => doOnRetry());
        }

        public AsyncRetryPolicy GenericLinearHttpRetryWithPredicate<T>(Func<T, bool> predicateHandle, Action doOnRetry = null) where T : Exception
        {
            return Policy.Handle(predicateHandle)
                         .WaitAndRetryAsync(_configuration.RetryCount, x => TimeSpan.FromSeconds(_configuration.RetryAttempt), onRetry: (a, b) => doOnRetry());
        }
    }
}
