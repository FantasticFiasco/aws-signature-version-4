using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using AwsSignatureVersion4.Private;

// ReSharper disable once CheckNamespace
namespace System.Net.Http
{
    /// <summary>
    /// Extensions to <see cref="HttpClient"/> extending it to support DELETE requests signed with
    /// AWS Signature Version 4.
    /// </summary>
    public static class DeleteAsyncExtensions
    {
        #region DeleteAsync(string, string, string, <credentials>)

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri as an
        /// asynchronous operation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            string requestUri,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.DeleteAsync(
                requestUri,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri as an
        /// asynchronous operation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            string requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.DeleteAsync(
                requestUri,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region DeleteAsync(Uri, string, string, <credentials>)

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri as an
        /// asynchronous operation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            Uri requestUri,
            string regionName,
            string serviceName,
            AWSCredentials credentials) =>
            self.DeleteAsync(
                requestUri,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri as an
        /// asynchronous operation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            Uri requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials) =>
            self.DeleteAsync(
                requestUri,
                regionName,
                serviceName,
                credentials,
                CancellationToken.None);

        #endregion

        #region DeleteAsync(string, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri with a
        /// cancellation token as an asynchronous operation.
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
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            string requestUri,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken) =>
            self.DeleteAsync(
                requestUri.ToUri(),
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri with a
        /// cancellation token as an asynchronous operation.
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
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            string requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken) =>
            self.DeleteAsync(
                requestUri.ToUri(),
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        #endregion

        #region DeleteAsync(Uri, CancellationToken, string, string, <credentials>)

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri with a
        /// cancellation token as an asynchronous operation.
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
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
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
        public static async Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            Uri requestUri,
            string regionName,
            string serviceName,
            AWSCredentials credentials,
            CancellationToken cancellationToken)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            var immutableCredentials = await credentials.GetCredentialsAsync().ConfigureAwait(false);

            var response = await self
                .DeleteAsync(
                    requestUri,
                    regionName,
                    serviceName,
                    immutableCredentials,
                    cancellationToken)
                .ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Send a Signature Version 4 signed DELETE request to the specified Uri with a
        /// cancellation token as an asynchronous operation.
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
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
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
        public static Task<HttpResponseMessage> DeleteAsync(
            this HttpClient self,
            Uri requestUri,
            string regionName,
            string serviceName,
            ImmutableCredentials credentials,
            CancellationToken cancellationToken) =>
            self.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, requestUri),
                regionName,
                serviceName,
                credentials,
                cancellationToken);

        #endregion
    }
}
