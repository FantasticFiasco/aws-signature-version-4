using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.Credentials;
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
            Array.Empty<KeyValuePair<string, IEnumerable<string>>>();

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

            var immutableCredentials = await GetImmutableCredentialsAsync().ConfigureAwait(false);

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

            var immutableCredentials = GetImmutableCredentials();

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
        /// Resolves the credentials to sign the request with. When <see cref="AwsSignatureHandlerSettings.Credentials"/>
        /// is null, falls back to the default AWS credential search order, resolved just before
        /// signing rather than up front. <see cref="DefaultAWSCredentialsIdentityResolver"/>
        /// caches the credentials it resolves, and only re-resolves them when the environment or
        /// the credential/config files backing them change, so calling it on every request is
        /// cheap.
        /// </summary>
        private async Task<ImmutableCredentials> GetImmutableCredentialsAsync()
        {
            var credentials = settings.Credentials ?? await DefaultAWSCredentialsIdentityResolver
                .GetCredentialsAsync()
                .ConfigureAwait(false);

            return await credentials.GetCredentialsAsync().ConfigureAwait(false);
        }

#if NET5_0_OR_GREATER

        /// <summary>
        /// Synchronous counterpart to <see cref="GetImmutableCredentialsAsync"/>.
        /// </summary>
        private ImmutableCredentials GetImmutableCredentials()
        {
            var credentials = settings.Credentials ?? DefaultAWSCredentialsIdentityResolver.GetCredentials();

            return credentials.GetCredentials();
        }

#endif

        /// <summary>
        /// Given the idempotent nature of message handlers, lets remove request headers that
        /// might have been added by a prior attempt to send the request.
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
