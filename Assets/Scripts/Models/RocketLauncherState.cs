using System;
using Planetarity.Rockets;

namespace Planetarity.Models
{
    [Serializable]
    public class RocketLauncherState
    {
        public RocketType RocketType;
        public float LeftCooldown;
    }
}