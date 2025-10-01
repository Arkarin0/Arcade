// Created/modified by Arkarin0 under one more more license(s).

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Arkarin0.Arcade.Common
{
    public class ArcadeHttpMessageHandler : HttpMessageHandler
    {
        public virtual RequestResponseHelper [] RequestResponses { get; set; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => SendAsync(request, CancellationToken.None);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach(var requestResponse in RequestResponses)
            {
                if(request.RequestUri.ToString().StartsWith(requestResponse.RequestMessage.RequestUri.ToString()) &&
                    request.Method.Equals(requestResponse.RequestMessage.Method))
                {
                    return Task.FromResult(new HttpResponseMessage()
                    {
                        StatusCode = requestResponse.ResponseMessage.StatusCode,
                        Content = requestResponse.ResponseMessage.Content
                    });
                }
            }
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent($"No response specified in RequestResponses for '({request.Method}) {request.RequestUri}")
            });
        }
    }
}
