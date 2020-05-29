using UnityEngine;

namespace Tanks.Environment
{
    public class Door : MonoBehaviour, IDoor
    {
        [SerializeField]
        private Transform _enter;
        [SerializeField]
        private Transform _exit;

        Vector3 IDoor.Enter => _enter.position;
        Vector3 IDoor.Exit => _exit.position;
    }
}