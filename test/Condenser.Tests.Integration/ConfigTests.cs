﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CondenserDotNet.Client;
using Xunit;

namespace Condenser.Tests.Integration
{
    public class ConfigTests
    {
        [Fact]
        public async Task TestRegister()
        {
            var manager = new ServiceManager("TestService");
            await manager.Config.SetKeyAsync("org/test/test1", "testValue1");
            await manager.Config.SetKeyAsync("org/test/test2", "testValue2");

            var result = await manager.Config.AddStaticKeyPathAsync("org/test/");

            var firstValue = manager.Config["test1"];
            var secondValue = manager.Config["test2"];
            
            Assert.Equal("testValue1", firstValue);
            Assert.Equal("testValue2", secondValue);
        }

        [Fact]
        public async Task DontPickUpChangesFact()
        {
            var manager = new ServiceManager("TestService");
            await manager.Config.SetKeyAsync("org/test2/test1", "testValue1");

            var result = await manager.Config.AddStaticKeyPathAsync("org/test2/");

            await manager.Config.SetKeyAsync("org/test2/test1", "testValue2");

            var firstValue = manager.Config["test1"];

            Assert.Equal("testValue1", firstValue);
        }

        [Fact]
        public async Task PickUpChangesFact()
        {
            var manager = new ServiceManager("TestService");
            await manager.Config.SetKeyAsync("org/test2/test1", "testValue1");

            var result = await manager.Config.AddUpdatingPathAsync("org/test2/");

            await manager.Config.SetKeyAsync("org/test2/test1", "testValue2");

            await Task.Delay(1000); //give it a second to sync

            var firstValue = manager.Config["test1"];

            Assert.Equal("testValue2", firstValue);
        }
    }
}
