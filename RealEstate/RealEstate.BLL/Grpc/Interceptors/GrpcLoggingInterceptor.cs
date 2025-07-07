using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RealEstate.BLL.Grpc.Interceptors
{
    public class GrpcLoggingInterceptor(ILogger<GrpcLoggingInterceptor> logger) : Interceptor
    {
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var method = context.Method;
            var stopwatch = Stopwatch.StartNew();

            logger.LogInformation("gRPC request started: {Method} | Request: {@Request}", method, request);

            try
            {
                var response = continuation(request, context);

                stopwatch.Stop();
                logger.LogInformation("gRPC request finished: {Method} | Duration: {Elapsed}ms | Response: {@Response}",
                    method, stopwatch.ElapsedMilliseconds, response);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "gRPC request failed: {Method} | Duration: {Elapsed}ms",
                    method, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
