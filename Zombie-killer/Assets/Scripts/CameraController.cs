using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private Transform orientation;
    
    [Header("Camera")]
    private float cameraHorizontalPosition;
    
    [SerializeField] private float rotationSpeed;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
        // Update is called once per frame
    void Update()
    {
        var localRotation = orientation.localRotation;
        float x = localRotation.eulerAngles.x;
        float z = localRotation.eulerAngles.z;
            
        localRotation = Quaternion.Euler(x, mainCamera.localRotation.eulerAngles.y, z);
        orientation.localRotation = localRotation;
            
        playerModel.localRotation = Quaternion.Lerp(playerModel.localRotation, localRotation, rotationSpeed);
    
    }
}
