using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace UnderwriteCancellation
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

        public void SendPublicMessage(string name, string message)
        {
            // Call the addNewNotice method to update clients.
            Clients.All.ReceivePublicMessage(name, message);
        }

        public void SendGroupNotice(string groupname, string message)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).ReceiveGroupNotice(message);
        }
    }
}