using System.Collections;
using System.Collections.Generic;
using Tanks.DINodes;
using Tanks.Tank;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks
{
    public class AppStart : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(LoadScenes());
        }

        private IEnumerator LoadScenes()
        {
            var coreEnvironmentLoading= SceneManager.LoadSceneAsync("CoreEnviroment", LoadSceneMode.Additive);
            var playerUiLoading = SceneManager.LoadSceneAsync("PlayerUI", LoadSceneMode.Additive);
            while (!coreEnvironmentLoading.isDone || !playerUiLoading.isDone)
            {
                yield return null;
            }

            InjectDeps();
        }

        private void InjectDeps()
        {
            var sceneNodes = GatherDeps();
            foreach (var sceneNode in sceneNodes)
            {
                sceneNode.Init(sceneNodes);
            }
        }

        private static List<SceneNode> GatherDeps()
        {
            var nodes = new List<SceneNode>(SceneManager.sceneCount);
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var rootGameObjects = scene.GetRootGameObjects();
                foreach (var rootGameObject in rootGameObjects)
                {
                    var sceneNode = rootGameObject.GetComponent<SceneNode>();
                    if (sceneNode != null)
                    {
                        sceneNode.InitBeforeInjected();
                        nodes.Add(sceneNode);
                    }
                }
            }

            return nodes;
        }
    }
}