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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHub.NotificationEngine.Common.Constants;
using TradeHub.NotificationEngine.Common.ValueObject;
using TradeHub.NotificationEngine.NotificationCenter.Services;

namespace TradeHub.NotificationEngine.Server.Tests.Integration
{
    [TestFixture]
    class NotificationControllerTestCases
    {
        private NotificationController _notificationController;

        [SetUp]
        public void SetUp()
        {
            _notificationController = new NotificationController();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        [Category("Integration")]
        public void NewNotification_SendEmailNotificationToReceiver()
        {
            OrderNotification orderNotification= new OrderNotification(NotificationType.Email, OrderNotificationType.Accepted);

            Order order = new Order("1234",OrderSide.BUY,100,OrderTif.DAY,"",new Security(){Symbol = "TC"},  OrderExecutionProvider.Simulated);
            orderNotification.SetOrder(order);

            _notificationController.NewNotificationArrived(orderNotification);
        }
    }
}
