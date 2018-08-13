using System.Xml;

namespace PING.Feature.SitecoreSettingsRedirector.Abstractions
{
	public interface IShowConfigService
	{
		XmlDocument GetConfiguration();
	}
}