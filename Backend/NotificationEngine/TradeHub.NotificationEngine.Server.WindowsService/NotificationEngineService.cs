/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Spring.Context.Support;
using TraceSourceLogger;
using TradeHub.NotificationEngine.Server.Service;

namespace TradeHub.NotificationEngine.Server.WindowsService
{
    partial class NotificationEngineService : ServiceBase
    {
        private ApplicationController _applicationController;

        public NotificationEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //set logging path
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                              "\\TradeHub Logs\\NotificationEngineServer";
            Logger.LogDirectory(path);
            try
            {
                _applicationController = ContextRegistry.GetContext()["ApplicationController"] as ApplicationController;
                if (_applicationController != null) _applicationController.StartCommunicator();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "NotificationEngineService", "OnStart");
            }
        }

        protected override void OnStop()
        {
            if (_applicationController != null) _applicationController.StopCommunicator();
        }
    }
}
