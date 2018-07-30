using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PING.Feature.SitecoreSettingsRedirector.Abstractions
{
	public interface ISitecoreInitializePipelineProcessor
	{
		void Process( PipelineArgs args );
	}
}
