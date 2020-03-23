// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Microsoft.Identity.Web.Test
{
    public class WebAppServiceCollectionExtensions
    {
        [Fact]
        public void AddMicrosoftIdentityPlatform()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddMsal()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddWebAppCallsProtectedWebApi()
        {
            throw new NotImplementedException();
            //Check TokenAcquisitionService
            //CheckHttPContextAccessor
            //ConfidentialClientApplicationOptions and MicrosoftIdentityOptions with default, valid and invalid options
            //OpenIdConnectOptions
            //  scope, responsetype, initial scopes if set, if not set
            //  OnAuthorizationCodeReceived
            //  OnTokenValidated
            //  OnRedirectToIdentityProviderForSignOut
        }

        [Fact]
        public void AddSignIn()
        {
            throw new NotImplementedException();
        }
    }
}
