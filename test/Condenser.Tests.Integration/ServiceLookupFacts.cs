﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CondenserDotNet.Client;
using Xunit;

namespace Condenser.Tests.Integration
{
    public class ServiceLookupFacts
    {
        [Fact]
        public async Task TestRegisterAndCheckRegistered()
        {
            var manager = new ServiceManager("TestService3", "Id1");
            var registrationResult = await manager.RegisterServiceAsync();
            Assert.Equal(true, registrationResult);

            var manager2 = new ServiceManager("TestService3", "Id2");
            var registrationResult2 = await manager.RegisterServiceAsync();
            Assert.Equal(true, registrationResult2);

            var service = await manager.Services.GetServiceInstanceAsync("TestService3");

            Assert.Equal("TestService3", service.Service);
        }

        [Fact]
        public async Task TestRegisterAndCheckUpdates()
        {
            var manager = new ServiceManager("ServiceLookup");
            Assert.Null(await manager.Services.GetServiceInstanceAsync("ServiceLookup"));
            await manager.RegisterServiceAsync();
            //Give it 500ms to update with the new service
            await Task.Delay(500);
            Assert.NotNull(await manager.Services.GetServiceInstanceAsync("ServiceLookup"));
        }
    }
}