using System.Collections;
using System.Collections.Generic;
using Tanks.Mobs;
using UnityEngine;

// TODO Мб перенести в ViewModel и сделать шарповый скрипт 
public class FaceCamera : MonoBehaviour
{
    private bool _inited;
    private MainCameraStorage _cameraStorage;

    public void Init(MainCameraStorage cameraStorage)
    {
        _cameraStorage = cameraStorage;
        _inited = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_inited && _cameraStorage.Exists())
        {
            transform.LookAt(_cameraStorage.Get().transform);
        }
    }
}
