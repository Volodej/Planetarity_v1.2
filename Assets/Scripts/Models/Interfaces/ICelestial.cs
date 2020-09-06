using UnityEngine;

namespace Planetarity.Models.Interfaces
{
    public interface ICelestial
    {
        float GravitationalParameter { get; }
        Vector3 Position { get; }
    }
}