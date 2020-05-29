using UnityEngine;

namespace Tanks.DI
{
    public class Door : MonoBehaviour, IDoor
    {
        public Transform Enter;
        public Transform Exit;

        Vector3 IDoor.Enter => Enter.position;
        Vector3 IDoor.Exit => Exit.position;
    }
}