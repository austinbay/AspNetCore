// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for enabling Negotiate authentication.
    /// </summary>
    public static class NegotiateExtensions
    {
        /// <summary>
        /// Adds Negotiate authentication.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <returns>The original builder.</returns>
        public static AuthenticationBuilder AddNegotiate(this AuthenticationBuilder builder)
            => builder.AddNegotiate(NegotiateDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// Adds and configures Negotiate authentication.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="configureOptions">Allows for configuring the authentication handler.</param>
        /// <returns>The original builder.</returns>
        public static AuthenticationBuilder AddNegotiate(this AuthenticationBuilder builder, Action<NegotiateOptions> configureOptions)
            => builder.AddNegotiate(NegotiateDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Adds and configures Negotiate authentication.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="authenticationScheme">The scheme name used to identify the authentication handler internally.</param>
        /// <param name="configureOptions">Allows for configuring the authentication handler.</param>
        /// <returns>The original builder.</returns>
        public static AuthenticationBuilder AddNegotiate(this AuthenticationBuilder builder, string authenticationScheme, Action<NegotiateOptions> configureOptions)
            => builder.AddNegotiate(authenticationScheme, displayName: null, configureOptions: configureOptions);

        /// <summary>
        /// Adds and configures Negotiate authentication.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="authenticationScheme">The scheme name used to identify the authentication handler internally.</param>
        /// <param name="displayName">The name displayed to users when selecting an authentication handler. The default is null to prevent this from displaying.</param>
        /// <param name="configureOptions">Allows for configuring the authentication handler.</param>
        /// <returns>The original builder.</returns>
        public static AuthenticationBuilder AddNegotiate(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<NegotiateOptions> configureOptions)
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable($"ASPNETCORE_TOKEN")))
            {
                throw new NotSupportedException(
                    "The Negotiate authentication handler must not be used with IIS out-of-process mode or similar reverse proxies that share connections between users."
                    + " Use the Windows Authentication features available within IIS or IIS Express.");
            }

            return builder.AddScheme<NegotiateOptions, NegotiateHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
