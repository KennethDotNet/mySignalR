using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SignalRAuth.Hubs
{
    public class ConnectionList
    {
        private readonly ConcurrentDictionary<string, ConnectionListItem> _connections = new ConcurrentDictionary<string, ConnectionListItem>();

        public int Count => _connections.Count;

        public void Add(ConnectionListItem connectionItem)
        {
            _connections.TryAdd(connectionItem.Connection.ConnectionId, connectionItem);
        }

        public void Remove(HubCallerContext connection)
        {
            _connections.TryRemove(connection.ConnectionId, out var nothing);
        }

        public HubCallerContext GetPrinter(string printername)
        {
            foreach (var item in _connections)
            {
                if (item.Value.Connection.User.Identity.Name.Equals(printername))
                    return item.Value.Connection;
            }

            return null;
        }

        public IEnumerable<ConnectionListItem> GetConnections()
        {
            foreach (var item in _connections)
            {
                yield return item.Value;
            }


        }
    }

    public class ConnectionListItem
    {
        public Guid CompanyNo { get; set; }
        public bool HasRole { get; set; }
        public HubCallerContext Connection { get; set; }
    }
}
