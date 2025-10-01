// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Arkarin0.Arcade.Common
{
    public class RequestResponseHelper
    {
        public HttpRequestMessage RequestMessage { get; set; }
        public HttpResponseMessage ResponseMessage { get; set;}
    }
}
