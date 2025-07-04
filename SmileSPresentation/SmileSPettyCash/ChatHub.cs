using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SmileSPettyCash
{
    public class ChatHub : Hub
    {
        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void SendGroupMessage(string name, string message, string groupname)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).ReceiveGroupMessage(name, message);
        }
    }
}