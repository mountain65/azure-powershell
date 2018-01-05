﻿// 		
//		
// Copyright Microsoft Corporation		
// Licensed under the Apache License, Version 2.0 (the "License");		
// you may not use this file except in compliance with the License.		
// You may obtain a copy of the License at		
// http://www.apache.org/licenses/LICENSE2.0		
// Unless required by applicable law or agreed to in writing, software		
// distributed under the License is distributed on an "AS IS" BASIS,		
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.		
// See the License for the specific language governing permissions and		
// limitations under the License.		
// 		

using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using AutoMapper;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Commands.Network.Models;
using MNM = Microsoft.Azure.Management.Network.Models;
using System;
using System.Linq;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;

namespace Microsoft.Azure.Commands.Network		
 {			
     [Cmdlet(VerbsCommon.Get, "AzureRmExpressRouteCircuitRouteTable"),OutputType(typeof(PSExpressRouteCircuitRoutesTable))]
     public class GetAzureRmExpressRouteCircuitRouteTable : NetworkBaseCmdlet		
     {		
         [Parameter(		
             Mandatory = true,		
             ValueFromPipelineByPropertyName = true,		
             HelpMessage = "The resource group name.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]		
         public virtual string ResourceGroupName { get; set; }

        [Alias("Name", "ResourceName")]
        [Parameter(		
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The Name of ExpressRoute Circuit")]		
         [ValidateNotNullOrEmpty]		
         public string ExpressRouteCircuitName { get; set; }		
 		
         [Parameter(		
             Mandatory = false,		
             HelpMessage = "The PeeringType")]		
         [ValidateSet(		
            MNM.ExpressRouteCircuitPeeringType.AzurePrivatePeering,		
            MNM.ExpressRouteCircuitPeeringType.AzurePublicPeering,		
            MNM.ExpressRouteCircuitPeeringType.MicrosoftPeering,		
            IgnoreCase = true)]		
         public string PeeringType { get; set; }

         [Parameter(
                      Mandatory = true,
                      HelpMessage = "The DevicePath, can be either Primary or Secondary")]
         [ValidateNotNullOrEmpty]
         public DevicePathEnum DevicePath { get; set; }

        public override void Execute()		
         {
             base.Execute();
             var routeTables = this.NetworkClient.NetworkManagementClient.ExpressRouteCircuits.ListRoutesTable
                 (ResourceGroupName, ExpressRouteCircuitName, PeeringType, DevicePath.ToString()).Value.Cast<object>().ToList();
             var psRoutes = new List<PSExpressRouteCircuitRoutesTable>();
             foreach (var routeTable in routeTables)
             {
                 var psRoute = NetworkResourceManagerProfile.Mapper.Map<PSExpressRouteCircuitRoutesTable>(routeTable);
                 psRoutes.Add(psRoute);
             }
             WriteObject(psRoutes, true);
         }		
     }		
 		
 }