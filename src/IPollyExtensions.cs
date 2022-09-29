using Polly.Retry;
using System;
using System.Net.Http;

namespace PollyExtensions
{
    public interface IPollyExtensions
    {
        AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry(Action doOnRetry = null);

        AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null);

        AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry(Action doOnRetry = null);

        AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null);

        AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry(Action doOnRetry = null);

        AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetryWithPredicate(Func<HttpResponseMessage, bool> predicateHandle, Action doOnRetry = null);

        AsyncRetryPolicy GenericLinearHttpRetry<T>(Action doOnRetry = null) where T : Exception;

        AsyncRetryPolicy GenericLinearHttpRetryWithPredicate<T>(Func<T, bool> predicateHandle, Action doOnRetry = null) where T : Exception;
    }
}
