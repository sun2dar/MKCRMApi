using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CCARE.Hubs
{
    public class CCAREHub : Hub
    {
        public void data(string guid, string msg)
        {
            Clients.All.refresh(guid, msg);
        }
    }
}