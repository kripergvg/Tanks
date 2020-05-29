using UnityEngine;

namespace Tanks.DI
{
    public interface IDoor
    {
        Vector3 Enter { get; }
        Vector3 Exit { get; }
    }
}