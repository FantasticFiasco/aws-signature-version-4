using System.Threading;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

#if NET5_0_OR_GREATER

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    /// <summary>
    /// Extensions to <see cref="HttpClient"/> extending it to support AWS Signature Version 4.
    /// </summary>
    public static class SendExtensions
    {
        #region Send(HttpRequestMessage, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.Send(
                request,
                completionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.Send(
                request,
                completionOption,
                CancellationToken.None,
                regionName,
                serviceName,
                credentials);

        #endregion

        #region Send(HttpRequestMessage, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                cancellationToken,
                regionName,
                serviceName,
                credentials);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                cancellationToken,
                regionName,
                serviceName,
                credentials);

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            AWSCredentials credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var immutableCredentials = credentials.GetCredentials();

            var response = self.Send(
                request,
                completionOption,
                cancellationToken,
                regionName,
                serviceName,
                immutableCredentials);

            return response;
        }

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request as a synchronous operation.
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
        /// <param name="cancellationToken">
        /// The cancellation token to cancel operation.
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
        /// The response message.
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
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            var signingTask = Signer.SignAsync(
                request,
                self.BaseAddress,
                self.DefaultRequestHeaders,
                DateTime.UtcNow,
                regionName,
                serviceName,
                credentials);

            System.Diagnostics.Debug.Assert(signingTask.IsCompletedSuccessfully, "The operation should have completed synchronously.");

            return self.Send(request, completionOption, cancellationToken);
        }

        #endregion
    }
}

#endif
