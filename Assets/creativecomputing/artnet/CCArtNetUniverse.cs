using UnityEngine;
using UnityEditor;

namespace cc.creativecomputing.artnet
{
    [System.Serializable]
    public class CCArtNetUniverse
    {
        public string name;
        public int universe;
        public CCArtNetDevice[] devices;

        public void Initialize()
        {
            var startChannel = 0;

            foreach (var d in devices)
                if (d != null)
                {
                    d.startChannel = startChannel;
                    startChannel += d.NumChannels;
                    d.name = string.Format("{0}:({1},{2:d3}-{3:d3})", d.GetType().ToString(), universe, d.startChannel, startChannel - 1);
                }
            if (512 < startChannel)
                Debug.LogErrorFormat("The number({0}) of channels of the universe {1} exceeds the upper limit(512 channels)!", startChannel, universe);
        }
    }
}