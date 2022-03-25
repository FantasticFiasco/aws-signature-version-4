using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Util;
using AwsSignatureVersion4.Private;

namespace AwsSignatureVersion4
{
    /// <summary>
    /// HTTP message handler capable of signing requests using Signature Version 4. This class is
    /// designed to be compatible with
    /// <see href="https://docs.microsoft.com/dotnet/api/system.net.http.ihttpclientfactory">
    /// IHttpClientFactory</see> and its request pipeline. For more information about message
    /// handlers and their usage, please see
    /// <see href="https://docs.microsoft.com/aspnet/web-api/overview/advanced/http-message-handlers">
    /// HTTP Message Handlers in ASP.NET Web API</see>.
    /// </summary>
    /// <remarks>
    /// Please make sure to leave the request unchanged after having it signed. Any changes in
    /// subsequent message handlers will render the signature invalid.
    /// </remarks>
    public class AwsSignatureHandler : DelegatingHandler
    {
        private static readonly KeyValuePair<string, IEnumerable<string>>[] EmptyRequestHeaders =
            new KeyValuePair<string, IEnumerable<string>>[0];

        private readonly AwsSignatureHandlerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSignatureHandler"/> class.
        /// </summary>
        /// <param name="settings">The signature settings.</param>
        public AwsSignatureHandler(AwsSignatureHandlerSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RemoveHeaders(request);

            var immutableCredentials = await settings.Credentials.GetCredentialsAsync().ConfigureAwait(false);

            await Signer
                .SignAsync(
                    request,
                    null,
                    EmptyRequestHeaders,
                    DateTime.UtcNow,
                    settings.RegionName,
                    settings.ServiceName,
                    immutableCredentials)
                .ConfigureAwait(false);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

#if NET5_0_OR_GREATER

        /// <inheritdoc />
        protected override HttpResponseMessage Send(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RemoveHeaders(request);

            var immutableCredentials = settings.Credentials.GetCredentials();

            Signer.Sign(
                request,
                null,
                EmptyRequestHeaders,
                DateTime.UtcNow,
                settings.RegionName,
                settings.ServiceName,
                immutableCredentials);

            return base.Send(request, cancellationToken);
        }

#endif

        /// <summary>
        /// Given the idempotent nature of message handlers, lets remove request headers that
        /// might have been added by an prior attempt to send the request.
        /// </summary>
        private static void RemoveHeaders(HttpRequestMessage request)
        {
            request.Headers.Remove(HeaderKeys.AuthorizationHeader);
            request.Headers.Remove(HeaderKeys.XAmzContentSha256Header);
            request.Headers.Remove(HeaderKeys.XAmzDateHeader);
            request.Headers.Remove(HeaderKeys.XAmzSecurityTokenHeader);
        }
    }
}
