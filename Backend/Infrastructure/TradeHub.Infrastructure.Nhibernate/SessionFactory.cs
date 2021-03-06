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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using TradeHub.Infrastructure.Nhibernate.NhibernateMappings;

namespace TradeHub.Infrastructure.Nhibernate
{
    /// <summary>
    /// session factory class without spring configurations
    /// </summary>
    public abstract class SessionFactory
    {
        //TODO: Have to replace session and transactions through spring
        public static ISessionFactory GetSessionFactory()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = "Server=localhost;Database=TradeHub;User ID=root;Password=;";
                x.Driver<MySqlDataDriver>();
                x.Dialect<MySQL5Dialect>();

            });
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            var mappings = GetMappings();
            cfg.AddDeserializedMapping(mappings, "NHSchemaTest");
            SchemaMetadataUpdater.QuoteTableAndColumns(cfg);
            return cfg.BuildSessionFactory();
        }

        public static HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetAssembly(typeof(SecurityMap)).GetExportedTypes());
            mapper.AddMappings(Assembly.GetAssembly(typeof(OrderMap)).GetExportedTypes());
            mapper.AddMappings(Assembly.GetAssembly(typeof(FillMap)).GetExportedTypes());
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            return mapping;
        }
    }
}
