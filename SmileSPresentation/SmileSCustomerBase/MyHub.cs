using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SmileSCustomerBase
{
    public class MyHub:Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendNotice(string name,string message)
        {
            // Call the addNewNotice method to update clients.
            Clients.All.SendNoticeResult(name,message);
        }

        public void RefreshQueueManager(string message)
        {
            Clients.All.RefreshQueueManagerResult(message);
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId,groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId,groupName);
        }

        public void SendNoticeGroup(string name,string message,string groupname)
        {
            //Send SendNoticeGroupResult
            Clients.Group(groupname).SendNoticeResult(name,message);
        }
    }
}