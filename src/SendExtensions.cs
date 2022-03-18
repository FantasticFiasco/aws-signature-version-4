#if NET5_0_OR_GREATER

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
    public static class SendExtensions
    {
        #region Send(HttpRequestMessage, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
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
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
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
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// One of the enumeration values that specifies when the operation should complete (as
        /// soon as a response is available or after reading the response content).
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
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
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// One of the enumeration values that specifies when the operation should complete (as
        /// soon as a response is available or after reading the response content).
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
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
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region Send(HttpRequestMessage, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// The request was canceled.
        /// <para/>
        /// -or-
        /// <para/>
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
        /// </exception>
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// The request was canceled.
        /// <para/>
        /// -or-
        /// <para/>
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
        /// </exception>
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken) =>
            self.Send(
                request,
                SendAsyncExtensions.DefaultCompletionOption,
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        #endregion

        #region Send(HttpRequestMessage, HttpCompletionOption, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// One of the enumeration values that specifies when the operation should complete (as
        /// soon as a response is available or after reading the response content).
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// The request was canceled.
        /// <para/>
        /// -or-
        /// <para/>
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
        /// </exception>
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var immutableCredentials = credentials.GetCredentials();

            var response = self.Send(
                request,
                completionOption,
                regionName,
                serviceName,
                immutableCredentials,
                cancellationToken);

            return response;
        }

        /// <summary>
        /// Send an Signature Version 4 signed HTTP request.
        /// </summary>
        /// <param name="self">
        /// The extension target.
        /// </param>
        /// <param name="request">
        /// The HTTP request message to send.
        /// </param>
        /// <param name="completionOption">
        /// One of the enumeration values that specifies when the operation should complete (as
        /// soon as a response is available or after reading the response content).
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
        /// The HTTP response message.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The request message was already sent by the <see cref="HttpClient"/> instance.
        /// </exception>
        /// <exception cref="HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS
        /// failure, or server certificate validation.
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// The request was canceled.
        /// <para/>
        /// -or-
        /// <para/>
        /// If the <see cref="TaskCanceledException"/> exception nests the
        /// <see cref="TimeoutException"/>: The request failed due to timeout.
        /// </exception>
        public static HttpResponseMessage Send(
            this HttpClient self,
            HttpRequestMessage request,
            HttpCompletionOption completionOption,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            Signer.Sign(
                request,
                self.BaseAddress,
                self.DefaultRequestHeaders,
                DateTime.UtcNow,
                regionName,
                serviceName,
                credentials);

            return self.Send(request, completionOption, cancellationToken);
        }

        #endregion
    }
}

#endif
