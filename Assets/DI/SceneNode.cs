using System.Collections.Generic;
using UnityEngine;

namespace Tanks.DI
{
    public abstract class SceneNode : MonoBehaviour
    {
        public virtual void InitBeforeInjected()
        {
            
        }
        
        public virtual void Init(List<SceneNode> sceneNodes)
        {
            
        }
    }
}