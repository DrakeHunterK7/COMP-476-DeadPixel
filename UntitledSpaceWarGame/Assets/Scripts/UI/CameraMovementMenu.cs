using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementMenu : MonoBehaviour
{
    //Camera Variables
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private float _cameraRotationSpeed;
    private GameObject _target;

    // Update is called once per frame
    void Update()
    {
        MoveCamera(_target);
    }

    //Move camera to next player
    public void MoveCamera(GameObject target)
    {
        if (target != null)
        {
            // Smooth move camera to the location of the currently active player or "agent"
            transform.position = Vector3.Lerp(transform.position, target.transform.position, _cameraMoveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, _cameraRotationSpeed * Time.deltaTime);
        }
    }

    public void SetCameraTarget(GameObject target)
    {
        _target = target;
    }
}
