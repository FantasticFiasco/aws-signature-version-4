using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    /// <summary>
    /// Extensions to <see cref="HttpClient"/> extending it to support GET requests signed with
    /// AWS Signature Version 4.
    /// </summary>
    public static class GetAsyncExtensions
    {
        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            string requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri.ToUri(),
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            Uri requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri,
                SendAsyncExtensions.DefaultCompletionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with an HTTP
        /// completion option as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="completionOption">
        /// An HTTP completion option value that indicates when the operation should be considered
        /// completed.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. Depending on the value of the
        /// <paramref name="completionOption"/> parameter, the returned <see cref="Task{TResult}"/>
        /// object will complete as soon as a response is available or the entire response
        /// including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            string requestUri,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri.ToUri(),
                completionOption,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with a cancellation
        /// token as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            string requestUri,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri.ToUri(),
                cancellationToken,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with an HTTP
        /// completion option as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="completionOption">
        /// An HTTP completion option value that indicates when the operation should be considered
        /// completed.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. Depending on the value of the
        /// <paramref name="completionOption"/> parameter, the returned <see cref="Task{TResult}"/>
        /// object will complete as soon as a response is available or the entire response
        /// including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            Uri requestUri,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri,
                completionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with a cancellation
        /// token as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            Uri requestUri,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri,
                SendAsyncExtensions.DefaultCompletionOption,
                cancellationToken,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with an HTTP
        /// completion option and a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="completionOption">
        /// An HTTP completion option value that indicates when the operation should be considered
        /// completed.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. Depending on the value of the
        /// <paramref name="completionOption"/> parameter, the returned <see cref="Task{TResult}"/>
        /// object will complete as soon as a response is available or the entire response
        /// including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            string requestUri,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.GetAsync(
                requestUri.ToUri(),
                completionOption,
                cancellationToken,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send a Signature Version 4 signed GET request to the specified Uri with an HTTP
        /// completion option and a cancellation token as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="requestUri">
        /// The Uri the request is sent to.
        /// </param>
        /// <param name="completionOption">
        /// An HTTP completion option value that indicates when the operation should be considered
        /// completed.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "s3".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role. Default value is null.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="requestUri"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. Depending on the value of the
        /// <paramref name="completionOption"/> parameter, the returned <see cref="Task{TResult}"/>
        /// object will complete as soon as a response is available or the entire response
        /// including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> GetAsync(
            this HttpClient self,
            Uri requestUri,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, requestUri),
                completionOption,
                cancellationToken,
                regionName,
                serviceName,
                credentials);
    }
}
