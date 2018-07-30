using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Abstractions;
using PING.Feature.SitecoreSettingsRedirector.Services;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class SitecoreSettingsAdaterTests
	{
		[TestMethod]
		public void SitecoreSettingsAdapter_Interfaces()
		{
			var adp = new SitecoreSettingsAdapter();
			Assert.IsInstanceOfType( adp, typeof(ISitecoreStaticSettings) );
		}
	}
}
