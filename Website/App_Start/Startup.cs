﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Website.Startup))]

namespace Website
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
		}
	}
}
