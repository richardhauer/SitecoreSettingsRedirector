using PING.Feature.SitecoreSettingsRedirector.Services;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Sitecore.Admin
{
	public class ShowConfig : global::Sitecore.sitecore.admin.ShowConfig
	{

		public ShowConfig()
		{
			this.Load += Page_Load;
		}

		protected new void Page_Load( object sender, EventArgs e )
		{
			CheckSecurity();

			var xdoc = new ShowConfigService( Request.QueryString ).GetConfiguration();

			Response.ContentType = "text/xml";
			Response.Write( xdoc.OuterXml );
		}

	}
}
