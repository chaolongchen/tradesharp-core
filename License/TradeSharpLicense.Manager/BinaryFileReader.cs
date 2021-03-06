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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TradeSharpLicense.Manager
{
    internal class BinaryFileReader
    {
        /// <summary>
        /// Read Data from file
        /// </summary>
        public string Read()
        {
            try
            {
                using (var binaryReader = new BinaryReader(File.Open("TradeSharpLicense.obj", FileMode.Open)))
                {
                    var byteBuffer = new byte[144];

                    for (int i = 0; i < 144; i++)
                    {
                        byteBuffer[i] = FromHex(binaryReader.ReadString()).First();   
                    }

                    var stringData = Encoding.ASCII.GetString(byteBuffer);
                    return stringData;
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return String.Empty;
        }

        /// <summary>
        /// Read Data from file
        /// </summary>
        public byte[] ReadBytes()
        {
            try
            {
                using (var binaryReader = new BinaryReader(File.Open("TradeSharpLicense.obj", FileMode.Open)))
                {
                    var byteBuffer = new byte[144];

                    for (int i = 0; i < 144; i++)
                    {
                        byteBuffer[i] = FromHex(binaryReader.ReadString()).First();
                    }

                    return byteBuffer;
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return null;
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}
