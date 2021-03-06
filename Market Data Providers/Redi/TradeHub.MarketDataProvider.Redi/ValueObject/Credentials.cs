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
using System.Text;

namespace TradeHub.MarketDataProvider.Redi.ValueObject
{
    /// <summary>
    /// Contains Properties required for REDI Connection
    /// </summary>
    public class Credentials
    {
        private string _username;
        private string _password;
        private string _ipAddress;
        private string _port;

        #region Properties

        /// <summary>
        /// Gets/Sets Username for connection
        /// </summary>
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }

        /// <summary>
        /// Gets/Sets Password for connection
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }

        /// <summary>
        /// IP Address to  be used on which to send request
        /// </summary>
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        /// <summary>
        /// Port to be used along with the IP
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        #endregion

        public Credentials()
        {
            
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        public Credentials(string username, string password, string ipAddress, string port)
        {
            _username = username;
            _password = password;
            _ipAddress = ipAddress;
            _port = port;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Attributes :: ");
            stringBuilder.Append(" | Username:" + _username);
            stringBuilder.Append(" | Ip Address:" + _ipAddress);
            stringBuilder.Append(" | Port:" + _port);

            return stringBuilder.ToString();
        }
    }
}
