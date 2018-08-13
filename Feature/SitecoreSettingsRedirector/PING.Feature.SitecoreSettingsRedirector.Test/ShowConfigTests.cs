using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PING.Feature.SitecoreSettingsRedirector.Sitecore.Admin;

namespace PING.Feature.SitecoreSettingsRedirector.Test
{
	[TestClass]
	public class ShowConfigTests
	{
		[TestMethod]
		public void ShowConfig_ImplementsBase()
		{
			var page = new ShowConfig();

			Assert.IsInstanceOfType( page, typeof( global::Sitecore.sitecore.admin.ShowConfig ) );
		}
	}
}
