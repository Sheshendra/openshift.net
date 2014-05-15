﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Uhuru.Openshift.Runtime;
using Uhuru.Openshift.Runtime.Config;
using Uhuru.Openshift.Runtime.Utils;

namespace Uhuru.Openshift.Tests
{
    public class TestHelper
    {
        const string nodeConfigFile = "Assets/node_test.conf";
        const string resourceLimitsFile = "Assets/resource_limits.conf";
        const string sampletManifestFile = "Assets/SampleManifest.yaml";

        public static string GetNodeConfigPath()
        {
            string path = Path.GetFullPath(nodeConfigFile);
            return path;
        }

        public static void InitTests()
        {
            Environment.SetEnvironmentVariable("OPENSHIFT_CONF_DIR", @"Assets/");
        }

        public static ApplicationContainer CreateAppContainer()
        {
            string applicationUuid = Guid.NewGuid().ToString("N");
            string containerUuid = applicationUuid;            
            NodeConfig config = new NodeConfig();
            // EtcUser etcUser = new Etc(config).GetPwanam(containerUuid);
            string userId = WindowsIdentity.GetCurrent().Name;
            string applicationName = "testApp";
            string containerName = applicationName;
            string namespaceName = "uhuru";
            object quotaBlocks = null;
            object quotaFiles = null;
            Hourglass hourglass = null;            
            Node.LimitsFile = Path.GetFullPath(resourceLimitsFile);
            ApplicationContainer container = new ApplicationContainer(
                applicationUuid, containerUuid, null,
                applicationName, containerName, namespaceName,
                quotaBlocks, quotaFiles, hourglass);
            
            return container;
        }

        public static Manifest GetSampleManifest()
        {           
            return new Manifest(sampletManifestFile,null,"file");
        }
    }
}
