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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.MarketDataProvider;
using TradeHub.Common.Core.ValueObjects.MarketData;
using TradeHubBarFactory = TradeHub.MarketDataEngine.BarFactory.Service.BarFactory;

namespace TradeHub.MarketDataEngine.MarketDataProviderGateway.Service
{
    /// <summary>
    /// Handles all the Live Bar related queries
    /// </summary>
    public class LiveBarGenerator
    {
        private Type _type = typeof (LiveBarGenerator);

        /// <summary>
        /// BarFactory Instance for creating Bars
        /// </summary>
        private TradeHubBarFactory _barFactory;

        public event Action<Bar, List<string>> LiveBarArrived;

        /// <summary>
        /// Keeps track of all the Bar RequestIDs
        /// Key = Local generated ID to uniquily identiy a Bar(combination of Security, BarType, BarFormat)
        /// Value  = List of Request IDs for the given Bar
        /// </summary>
        private Dictionary<string, List<string>> _barRequestIdsMap = new Dictionary<string, List<string>>();

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="barFactory">TradeHub Bar Factory for creating custom bars</param>
        public LiveBarGenerator(TradeHubBarFactory barFactory)
        {
            _barFactory = barFactory;
        }

        /// <summary>
        /// Send Live Bar subscribe request to the Market Data Provider
        /// </summary>
        public void SubscribeBars(BarDataRequest barDataRequest)
        {
            try
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Sending incoming live bar subscription request to Bar Factory", _type.FullName,
                                "SubscribeBars");
                }

                string key = barDataRequest.Security.Symbol + barDataRequest.BarFormat + barDataRequest.BarPriceType +
                             barDataRequest.BarLength.ToString(CultureInfo.InvariantCulture);

                List<string> reqIds;
                if (_barRequestIdsMap.TryGetValue(key, out reqIds))
                {
                    // Update ReqIDs List
                    reqIds.Add(barDataRequest.Id);

                    // Update Bar ReqIDs Map
                    _barRequestIdsMap[key] = reqIds;
                }
                else
                {
                    // Initialize
                    reqIds = new List<string>();

                    // Update ReqIDs List
                    reqIds.Add(barDataRequest.Id);

                    // Update Bar ReqIDs Map
                    _barRequestIdsMap[key] = reqIds;

                    // Subscribe Bar Factory to obtain Live Bars
                    _barFactory.Subscribe(barDataRequest, key, OnLiveBarArrived);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "SubscribeBars");
            }
        }

        /// <summary>
        /// Send Live Bar subscribe request to the Market Data Provider
        /// </summary>
        public void UnsubscribeBars(BarDataRequest barDataRequest)
        {
            try
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Unsubscription request recieved for Bar Factory", _type.FullName,
                                "UnsubscribeBars");
                }

                string key = barDataRequest.Security.Symbol + barDataRequest.BarFormat + barDataRequest.BarPriceType +
                             barDataRequest.BarLength.ToString(CultureInfo.InvariantCulture);

                List<string> reqIds;
                if (_barRequestIdsMap.TryGetValue(key, out reqIds))
                {
                    // Update ReqIDs List
                    reqIds.Remove(barDataRequest.Id);

                    if (reqIds.Count.Equals(0))
                    {
                        // Update Bar ReqIDs Map
                        _barRequestIdsMap.Remove(key);

                        if (Logger.IsInfoEnabled)
                        {
                            Logger.Info("Sending unsubscription request recieved to Bar Factory", _type.FullName,
                                        "UnsubscribeBars");
                        }

                        // Unsubscribe Bar with given Info
                        _barFactory.Unsubscribe(barDataRequest, key, OnLiveBarArrived);
                    }
                    else
                    {
                        // Update Bar ReqIDs Map
                        _barRequestIdsMap[key] = reqIds;

                        if (Logger.IsInfoEnabled)
                        {
                            Logger.Info("Requested Bar ID removed from the Internal IDs Map", _type.FullName,
                                        "UnsubscribeBars");
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "UnsubscribeBars");
            }
        }

        /// <summary>
        /// Update Bar by adding new Tick to the processing
        /// </summary>
        /// <param name="tick">TradeHub Tick</param>
        public void UpdateBar(Tick tick)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Sending new Tick to Bar Factory: " + tick, _type.FullName, "UpdateBar");
                }

                lock (tick)
                {
                    // Send new tick to Bar Factory
                    _barFactory.Update(tick);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "UpdateBar");
            }
        }

        /// <summary>
        /// Raised when a new Bar is recieved
        /// </summary>
        /// <param name="bar">TradeHub Bar</param>
        /// <param name="key">Unique Key to identify the Bar </param>
        private void OnLiveBarArrived(Bar bar, string key)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("New Bar arrived from Bar Factory: " + bar, _type.FullName, "OnLiveBarArrived");
                }

                List<string> reqIds;
                if (_barRequestIdsMap.TryGetValue(key, out reqIds))
                {
                    // Raise Event
                    if (LiveBarArrived != null)
                    {
                        LiveBarArrived(bar, reqIds);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "OnLiveBarArrived");
            }
        }
    }
}
