using UnityEngine;

namespace Tanks.Tank.Abilities
{
    public class Missile : MonoBehaviour
    {
        public Rigidbody _body;
        
        public void Fire(Vector3 force)
        {
            _body.AddForce(force, ForceMode.Impulse);
        }
    }
}