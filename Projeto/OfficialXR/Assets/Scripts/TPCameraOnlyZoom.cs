using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCameraOnlyZoom : MonoBehaviour {

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float distanceTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 70);

    public float rotationSmoothTime = 0.16f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    
    float yaw;
    float pitch;
	float rot = 0.0f;

	// Use this for initialization
	void Start () {
		distanceTarget = 4;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
        yaw += Input.GetAxis("CamX") * mouseSensitivity;
        pitch -= Input.GetAxis("CamY") * mouseSensitivity;
        pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		if (distanceTarget >= 2) {
			rot += Input.GetAxis ("CamDist") * 5;
		}

		rot = Mathf.Clamp (rot, 0, 30);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(rot, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceTarget;
	
		distanceTarget += Input.GetAxis ("CamDist")/2;

		distanceTarget = Mathf.Clamp (distanceTarget, 2, 5.5f);
	}
}