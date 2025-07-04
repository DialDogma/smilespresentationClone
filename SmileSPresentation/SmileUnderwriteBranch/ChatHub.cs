using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SmileUnderwriteBranch
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

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

        public void SendGroupToUpdateQueue(string groupname, string message)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).ReceiveGroupToUpdateQueue(message);
        }

        public void SendGroupMessage(string name, string message, string groupname)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).ReceiveGroupMessage(name, message);
        }

        public void SendNoticeGroupMessage(string groupname, string message)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).ReceiveNotice(message);
        }
    }
}