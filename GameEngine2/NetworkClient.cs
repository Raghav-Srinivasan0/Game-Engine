using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;

//https://dev.to/deagahelio/getting-started-with-litenetlib-2fok

namespace GameEngine2
{
    public class NetworkClient : INetEventListener
    {
        private NetManager client;
        private NetPeer server;

        public void Connect()
        {
            client = new NetManager(this)
            {
                AutoRecycle = true,
            };
        }

        public void _Process(float delta)
        {
            if (client != null)
            {
                client.PollEvents();
            }
        }
        public void OnPeerConnected(NetPeer peer)
        {
            server = peer;
        }

        public class JoinPacket
        {
            public string username { get; set; }
        }
    }
}
