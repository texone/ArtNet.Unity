using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using UnityEngine;

using ArtNet.Sockets;
using ArtNet.Packets;


namespace cc.creativecomputing.artnet
{
    public class CCArtNetIO : MonoBehaviour
    {
        private static IPAddress FindFromHostName(string hostname)
        {
            var address = IPAddress.None;
            try
            {
                if (IPAddress.TryParse(hostname, out address))
                    return address;

                var addresses = Dns.GetHostAddresses(hostname);
                for (var i = 0; i < addresses.Length; i++)
                {
                    if (addresses[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        address = addresses[i];
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogErrorFormat(
                    "Failed to find IP for :\n host name = {0}\n exception={1}",
                    hostname, e);
            }
            return address;
        }
        
        public bool useBroadcast;
        public string remoteIP = "localhost";
        IPEndPoint remote;
        
        public bool isServer;

        ArtNetSocket artnet;

        [Header("send/recieved DMX data for debug")]
        [SerializeField] ArtNetDmxPacket dmxToSend;

        public CCArtNetReceiver[] receiver;


        [ContextMenu("send DMX")]
        public void Send()
        {
            if (useBroadcast && isServer)
                artnet.Send(dmxToSend);
            else
                artnet.Send(dmxToSend, remote);
        }
        public void Send(short universe, byte[] dmxData)
        {
            dmxToSend.Universe = universe;
            System.Buffer.BlockCopy(dmxData, 0, dmxToSend.DmxData, 0, dmxData.Length);
            if (useBroadcast && isServer)
                artnet.Send(dmxToSend);
            else
                artnet.Send(dmxToSend, remote);
        }

        private void OnValidate()
        {
            foreach(var r in receiver)
            {
                r.Validate();
            }
        }

        void Start()
        {
            artnet = new ArtNetSocket();

            if (isServer)
                artnet.Open(FindFromHostName("localhost"), null);

            dmxToSend.DmxData = new byte[512];

            artnet.NewPacket += (object sender, NewPacketEventArgs<ArtNetPacket> e) =>
            {
                
            };

            if (!useBroadcast || !isServer)
                remote = new IPEndPoint(FindFromHostName(remoteIP), ArtNetSocket.Port);

            
        }

        private void OnDestroy()
        {
            artnet.Close();
        }

        private void Update()
        {
            foreach (var r in receiver)
            {
                r.Update();
            }
        }

        

        
    }
}
