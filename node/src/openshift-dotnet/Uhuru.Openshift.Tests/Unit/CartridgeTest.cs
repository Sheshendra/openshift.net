﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uhuru.Openshift.Common.Models;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel.Serialization;
using YamlDotNet.Core;
using Uhuru.Openshift.Runtime;

namespace Uhuru.Openshift.Tests
{
    [TestClass]
    public class CartridgeTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Test_Cartridge()
        {
            string cartridgePath = Path.Combine(CartridgeRepository.CartridgeBasePath, "dotnet");
            string manifestPath = Path.Combine(cartridgePath, "metadata", "manifest.yml");
            string document = File.ReadAllText(manifestPath);
            var input = new StringReader(document);
            var deserializer = new Deserializer();
            dynamic spec = (dynamic)deserializer.Deserialize(input);
            Cartridge cart = Cartridge.FromDescriptor(spec);            
            Assert.AreEqual("dotnet", cart.OriginalName);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Test_ToDescriptor()
        {
            string cartridgePath = Path.Combine(CartridgeRepository.CartridgeBasePath, "dotnet");
            string manifestPath = Path.Combine(cartridgePath, "metadata", "manifest.yml");
            string document = File.ReadAllText(manifestPath);
            var input = new StringReader(document);
            var deserializer = new Deserializer();
            dynamic spec = (dynamic)deserializer.Deserialize(input);
            Cartridge cart = Cartridge.FromDescriptor(spec);
            dynamic desc = cart.ToDescriptor();
            Assert.IsTrue(desc.ContainsKey("Name"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Test_CartridgeFromDescriptor()
        {
            bool testresult = false;
            Manifest sampleManifest = TestHelper.GetSampleManifest();
            var input = new StringReader(sampleManifest.ToManifestString());            
            var deserializer = new Deserializer();
            dynamic spec = (dynamic)deserializer.Deserialize(input);
            Cartridge cart = Cartridge.FromDescriptor(spec);
            if (cart != null)
            {
                testresult = true;
            }
            Assert.AreEqual(true, testresult);
        }
    }
}
