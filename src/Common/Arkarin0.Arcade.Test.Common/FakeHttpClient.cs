// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Arcade.Test.Common
{
    public static class FakeHttpClient
    {
        
        public static HttpClient WithResponses(params HttpResponseMessage[] responses)
            => new HttpClient( // lgtm [cs/httpclient-checkcertrevlist-disabled] HttpClient used in unit tests
                new FakeHttpMessageHandler(responses));

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly IEnumerator<HttpResponseMessage> _responseEnumerator;

            public FakeHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
            {
                _responseEnumerator = responses.GetEnumerator();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _responseEnumerator.Dispose();
                }
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!_responseEnumerator.MoveNext())
                    throw new InvalidOperationException($"Unexpected end of response sequence. Number of predefined responses should be at least equal to number of requests invoked by HttpClient.");

                return Task.FromResult(_responseEnumerator.Current);
            }
        }
    }
}
