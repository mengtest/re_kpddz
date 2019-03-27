using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Camera _targetCamera;
	Quaternion direction=new Quaternion();
	
	// Use this for initialization
	void Start () {
        direction.x = transform.localRotation.x;
        direction.y = transform.localRotation.y;
        direction.z = transform.localRotation.z;
        direction.w = transform.localRotation.w;
	}
	
	// Update is called once per frame
	void Update ()
	{
        Camera cam=null;
        if (_targetCamera != null)
        {
            cam = _targetCamera;
        }
        else
        {
            cam = Camera.current;
            if (!cam)
                return;
        }
        //transform.rotation = cam.transform.rotation * direction;

        Vector3 v = cam.transform.position - transform.position;
        v.x = v.y = 0.0f;
        transform.LookAt(cam.transform.position - v); 
	}

}