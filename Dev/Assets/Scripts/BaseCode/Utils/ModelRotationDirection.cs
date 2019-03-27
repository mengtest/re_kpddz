/***************************************************************

 *
 *
 * Filename:  	RotationDirection.cs	
 * Summary: 	先按指定轴，转指定度数，再自转
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/10/14 20:14
 ***************************************************************/
using UnityEngine;
using System.Collections;

public class ModelRotationDirection : MonoBehaviour
{
    public float speedX = -77f;
    public float speedY = -180f;
    public float speedZ = -180f;
    public bool autoRotate = false;
    public float fSpeed = 100f;

    private Transform tr;
    private Quaternion q;
    private Vector3 axis;
    void Awake()
    {
        tr = transform;
    }

    void Start() {
        q = Quaternion.Euler(speedX, speedY, speedZ);
        tr.localRotation = q;
    }

    public void Rotate(float value)
    {
        tr.Rotate(Vector3.back * value);
    }

    void Update() {
        if (!autoRotate) return;
        Rotate(Time.deltaTime * fSpeed);
    }
}