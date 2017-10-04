using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour 
{
    float rotationY = 0F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    public float sensitivityY = 15F;
    public float sensitivityX = 15F;

    private Transform _transform = null;
    private Transform _transformParent = null;

    private void Start()
    {
        _transform = transform;
        _transformParent = _transform.parent;

        Cursor.visible = false;
    }

    private void Update() 
    {
        _transformParent.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
	}
}
