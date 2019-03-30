// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace IdServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "IdentityServer4";

            await BuildWebHostBuilder(args).Build().RunAsync();
        }

        public static IWebHostBuilder BuildWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();
        }            
    }
}