using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAuth.Hubs
{
    [Authorize]
    public class TestHub : Hub
    {
        private ConnectionList _connectionList;
       
        public TestHub(ConnectionList connectionList)
        {
            _connectionList = connectionList;
        }
        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            await Send(username);
            await base.OnConnectedAsync();
        }
        [Authorize]
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
        }
        [Authorize]
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("send",message);
        }




    }


}

