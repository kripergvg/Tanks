using UnityEngine;

namespace Tanks.Environment
{
    public interface IDoor
    {
        Vector3 Enter { get; }
        Vector3 Exit { get; }
    }
}