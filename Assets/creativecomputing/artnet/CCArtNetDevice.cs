using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cc.creativecomputing.artnet
{
    public abstract class CCArtNetDevice : MonoBehaviour
    {
        public byte[] dmxData;
        public int startChannel;
        public abstract int NumChannels { get; }

        public virtual void SetData(byte[] theDmxData)
        {
            dmxData = theDmxData;
        }

        public enum ChannelFunction
        {
            UNKNOWN = -1,

            COLOR_R = 0,
            COLOR_R_16 = 1,
            COLOR_G = 2,
            COLOR_G_16 = 3,
            COLOR_B = 4,
            Color_B_16 = 5,
            Color_W = 6,
            Color_W_16 = 7,

            PAN = 8,
            PAN_16 = 9,
            TILT = 10,
            Tilt_16 = 11,
            ROT_SPEED = 12,
        }
    }
}
