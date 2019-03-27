using UnityEngine;
using System.Collections;

public class FollowToSceneCamera : MonoBehaviour
{
    public Transform camera_tf;
    void Update()
    {
        if (camera_tf == null)
            return;

        transform.position = camera_tf.position;
    }
}
