using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using ArtNet.Sockets;
using ArtNet.Packets;

namespace cc.creativecomputing.artnet
{

    public class CCArtNetReceiver : MonoBehaviour
    {

        public CCArtNetUniverse[] universes; 


        Dictionary<int, byte[]> dmxDataMap;

        public bool newPacket;
        byte[] _dmxData;

        [SerializeField] ArtNetDmxPacket latestReceivedDMX;

        public CCArtNetReceiver()
        {
            dmxDataMap = new Dictionary<int, byte[]>();
        }

        public void Validate()
        {
            foreach (var u in universes)
                u.Initialize();
        }

        public void OnReceive(object sender, NewPacketEventArgs<ArtNetPacket> e)
        {
            newPacket = true;
            if (e.Packet.OpCode == ArtNet.Enums.ArtNetOpCodes.Dmx)
            {
                var packet = latestReceivedDMX = e.Packet as ArtNetDmxPacket;

                if (packet.DmxData != _dmxData)
                    _dmxData = packet.DmxData;

                var universe = packet.Universe;
                if (dmxDataMap.ContainsKey(universe))
                    dmxDataMap[universe] = packet.DmxData;
                else
                    dmxDataMap.Add(universe, packet.DmxData);
            }
        }

        public void Update()
        {
            var keys = dmxDataMap.Keys.ToArray();

            for (var i = 0; i < keys.Length; i++)
            {
                var universe = keys[i];
                var dmxData = dmxDataMap[universe];
                if (dmxData == null)
                    continue;

                var universeDevices = universes.Where(u => u.universe == universe).FirstOrDefault();
                if (universeDevices != null)
                    foreach (var d in universeDevices.devices)
                        d.SetData(dmxData.Skip(d.startChannel).Take(d.NumChannels).ToArray());

                dmxDataMap[universe] = null;
            }
        }
    }
}