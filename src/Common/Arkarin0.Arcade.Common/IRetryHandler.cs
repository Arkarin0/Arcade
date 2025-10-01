// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arkarin0.Arcade.Common
{
    public interface IRetryHandler
    {
        Task<bool> RunAsync(
            Func<int, Task<bool>> actionSuccessfulAsync);

        Task<bool> RunAsync(
            Func<int, Task<bool>> actionSuccessfulAsync,
            CancellationToken cancellationToken);
    }
}
