using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(eWarsztaty.Web.SignalR.Startup))]

namespace eWarsztaty.Web.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            var hubConfiguration = new HubConfiguration
            {
                // You can enable JSONP by uncommenting line below.
                // JSONP requests are insecure but some older browsers (and some
                // versions of IE) require JSONP to work cross domain
                EnableJSONP = true
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}