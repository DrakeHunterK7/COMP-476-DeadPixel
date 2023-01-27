using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private GameObject cameraPoint;
    [SerializeField] private Camera cameraThirdPerson;

    [SerializeField] private GameObject shipModel;


    private float shipSpeed = 10f;
    private float shipTurnSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow();
        var forwardInput = -Input.GetAxis("Vertical");
        var horizontalInput = Mathf.Clamp(Input.mousePosition.x-(Screen.width/2), -1, 1);
        var verticalInput = Mathf.Clamp(-Input.mousePosition.y-(Screen.height/2), -1, 1);

        transform.position += forwardInput * shipModel.transform.right * shipSpeed * Time.deltaTime;
        transform.RotateAround(shipModel.transform.position, shipModel.transform.up, horizontalInput*shipTurnSpeed*Time.deltaTime);
        transform.RotateAround(shipModel.transform.position, shipModel.transform.forward, verticalInput*shipTurnSpeed*Time.deltaTime);

    }

    void CameraFollow()
    {

        cameraThirdPerson.transform.position = cameraPoint.transform.position;

        cameraThirdPerson.transform.rotation = shipModel.transform.rotation * Quaternion.Euler(0f, -90, 0f);
    }
}
