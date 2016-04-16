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
          
            app.MapSignalR();
        }
    }
}