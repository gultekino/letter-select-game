using System;
using UnityEngine;

public class CameraManager:Singleton<CameraManager>
{
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        //mainCamera.transform.position = GridsManager.Instance.GetGridsCenterLoc();       
    }
}