using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace eWarsztaty.Web.SignalR
{
    public class TopicsHub : Hub
    {
        public async Task JoinGroup(int topicId)
        {
            await Groups.Add(Context.ConnectionId, topicId.ToString());
        }

        public void Lock(string data, int topicId)
        {
            Clients.Group(topicId.ToString()).broadcastMessage(data);
        }
    }
}