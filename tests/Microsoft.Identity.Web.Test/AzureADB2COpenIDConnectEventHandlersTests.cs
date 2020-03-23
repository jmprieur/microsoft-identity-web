// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Identity.Web.Test.Common.TestHelpers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NSubstitute;
using Xunit;

namespace Microsoft.Identity.Web.Test
{
    public class AzureADB2COpenIDConnectEventHandlersTests
    {
        [Fact]
        public async void OnRedirectToIdentityProvider_NonDefaultSusiFlow_UpdatesContext()
        {
            var defaultUserFlow = "DefaultUserFlow";
            var nonDefaultUserFlow = "NonDefaultUserFlow";
            var defaultIssuer = $"IssuerAddress/{defaultUserFlow}/";
            var nonDefaultIssuer = $"IssuerAddress/{nonDefaultUserFlow}/";

            var options = new MicrosoftIdentityOptions() { SignUpSignInPolicyId = defaultUserFlow };
            var handler = new AzureADB2COpenIDConnectEventHandlers(OpenIdConnectDefaults.AuthenticationScheme, options);
            var _httpContext = HttpContextUtilities.CreateHttpContext();
            var _authScheme = new AuthenticationScheme(OpenIdConnectDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme, typeof(OpenIdConnectHandler));
            var _options = new OpenIdConnectOptions();
            var _authProperties = new AuthenticationProperties();

            _authProperties.Items.Add(OidcConstants.PolicyKey, nonDefaultUserFlow);
            var context = new RedirectContext(_httpContext, _authScheme, _options, _authProperties) { ProtocolMessage = new OpenIdConnectMessage() { IssuerAddress = defaultIssuer } };

            await handler.OnRedirectToIdentityProvider(context);

            Assert.Equal(OpenIdConnectScope.OpenIdProfile, context.ProtocolMessage.Scope);
            Assert.Equal(OpenIdConnectResponseType.IdToken, context.ProtocolMessage.ResponseType);
            Assert.Equal(nonDefaultIssuer, context.ProtocolMessage.IssuerAddress, true);
            Assert.False(context.Properties.Items.ContainsKey(OidcConstants.PolicyKey));
        }

        [Fact]
        public async void OnRedirectToIdentityProvider_DefaultSusiFlow_DoesntUpdateContext()
        {
            var defaultUserFlow = "DefaultUserFlow";
            var nonDefaultUserFlow = "NonDefaultUserFlow";
            var defaultIssuer = $"IssuerAddress/{defaultUserFlow}/";

            var options = new MicrosoftIdentityOptions() { SignUpSignInPolicyId = defaultUserFlow };
            var handler = new AzureADB2COpenIDConnectEventHandlers(OpenIdConnectDefaults.AuthenticationScheme, options);
            var _httpContext = HttpContextUtilities.CreateHttpContext();
            var _authScheme = new AuthenticationScheme(OpenIdConnectDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme, typeof(OpenIdConnectHandler));
            var _options = new OpenIdConnectOptions();
            var _authProperties = new AuthenticationProperties();

            _authProperties.Items.Add(OidcConstants.PolicyKey, defaultUserFlow);
            var context = new RedirectContext(_httpContext, _authScheme, _options, _authProperties) { ProtocolMessage = new OpenIdConnectMessage() { IssuerAddress = defaultIssuer } };

            await handler.OnRedirectToIdentityProvider(context);

            Assert.Null(context.ProtocolMessage.Scope);
            Assert.Null(context.ProtocolMessage.ResponseType);
            Assert.Equal(defaultIssuer, context.ProtocolMessage.IssuerAddress);
            Assert.True(context.Properties.Items.ContainsKey(OidcConstants.PolicyKey));
        }

        [Fact]
        public async void OnRemoteFailure_PasswordReset_RedirectsSuccessfully()
        {
            var passwordResetException = "'access_denied', error_description: 'AADB2C90118: The user has forgotten their password. Correlation ID: f99deff4-f43b-43cc-b4e7-36141dbaf0a0 Timestamp: 2018-03-05 02:49:35Z', error_uri: 'error_uri is null'";

            var options = new MicrosoftIdentityOptions();
            var handler = new AzureADB2COpenIDConnectEventHandlers(OpenIdConnectDefaults.AuthenticationScheme, options);
            var _httpContext = HttpContextUtilities.CreateHttpContext();
            _httpContext.Request.PathBase = "/PathBase";
            var _authScheme = new AuthenticationScheme(OpenIdConnectDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme, typeof(OpenIdConnectHandler));
            var _options = new OpenIdConnectOptions();

            var context = new RemoteFailureContext(_httpContext, _authScheme, _options, new OpenIdConnectProtocolException(passwordResetException));
            var response = Substitute.For<HttpResponse>();
            
            await handler.OnRemoteFailure(context);

        }

        [Fact]
        public async void OnRemoteFailure_Cancel_RedirectsSuccessfully()
        {
            var cancelException = "'access_denied', error_description: 'AADB2C90091: The user has canceled entering self-asserted information. Correlation ID: d01c8878-0732-4eb2-beb8-da82a57432e0 Timestamp: 2018-03-05 02:56:49Z ', error_uri: 'error_uri is null'";


        }

        [Fact]
        public async void OnRemoteFailure_OtherException_RedirectsSuccessfully()
        {
            var otherException = "Generic exception.";


        }
    }
}
