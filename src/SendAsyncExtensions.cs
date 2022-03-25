using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    /// <summary>
    /// Extensions to <see cref="HttpClient"/> extending it to support AWS Signature Version 4.
    /// </summary>
    public static class SendAsyncExtensions
    {
        internal const HttpCompletionOption DefaultCompletionOption = HttpCompletionOption.ResponseContentRead;

        #region SendAsync(HttpRequestMessage, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.SendAsync(
                request,
                DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.SendAsync(
                request,
                DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region SendAsync(HttpRequestMessage, HttpCompletionOption, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// When the operation should complete (as soon as a response is available or after reading
        /// the whole response content).
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
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
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.SendAsync(
                request,
                completionOption,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// When the operation should complete (as soon as a response is available or after reading
        /// the whole response content).
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
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
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.SendAsync(
                request,
                completionOption,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region SendAsync(HttpRequestMessage, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken) =>
            self.SendAsync(
                request,
                DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, server certificate validation or timeout.
        /// </exception>
        /// <remarks>
        /// This operation will not block. The returned <see cref="Task{TResult}"/> object will
        /// complete once the entire response including content is read.
        /// </remarks>
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken) =>
            self.SendAsync(
                request,
                DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        #endregion

        #region SendAsync(HttpRequestMessage, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// When the operation should complete (as soon as a response is available or after reading
        /// the whole response content).
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
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
        public static async Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var immutableCredentials = await credentials.GetCredentialsAsync().ConfigureAwait(false);

            var response = await self
                .SendAsync(
                    request,
                    completionOption,
                    regionName,
                    serviceName,
                    immutableCredentials,
                    cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// When the operation should complete (as soon as a response is available or after reading
        /// the whole response content).
        /// </param>
        /// <param name="regionName">
        /// The system name of the AWS region associated with the endpoint, e.g. "us-east-1".
        /// </param>
        /// <param name="serviceName">
        /// The signing name of the service, e.g. "execute-api".
        /// </param>
        /// <param name="credentials">
        /// AWS credentials containing the following parameters:
        /// - The AWS public key for the account making the service call.
        /// - The AWS secret key for the account making the call, in clear text.
        /// - The session token obtained from STS if request is authenticated using temporary
        ///   security credentials, e.g. a role.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
        /// </param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
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
        public static async Task<HttpResponseMessage> SendAsync(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            await Signer
                .SignAsync(
                    request,
                    self.BaseAddress,
                    self.DefaultRequestHeaders,
                    DateTime.UtcNow,
                    regionName,
                    serviceName,
                    credentials)
                .ConfigureAwait(false);

            return await self.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
