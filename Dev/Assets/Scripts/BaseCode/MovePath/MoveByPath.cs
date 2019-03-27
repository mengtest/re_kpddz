using UnityEngine;
using System.Collections;

/// <summary>
/// 移动目标通过路径
/// </summary>
public class MoveByPath : MonoBehaviour
{
    public float time = 1f;
    public float delay = 0f;
    public MovePathBase movePath;

    public delegate void moveFinishDelegate(GameObject go);
    public moveFinishDelegate onMoveFinish;

    private bool isPlay;
    private float fStartTime;
    Vector3 tempPos = Vector3.zero;
    public static void Play(GameObject go, MovePathBase path, float t, float delay = 0, moveFinishDelegate onFinish = null)
    {
        MoveByPath moveByPath = go.GetComponent<MoveByPath>();
        if (moveByPath == null) {
            moveByPath = go.AddComponent<MoveByPath>();
        }
        moveByPath.time = t;
        moveByPath.delay = delay;
        moveByPath.movePath = path;
        moveByPath.onMoveFinish = onFinish;
        moveByPath.play();
    }

    public void play()
    {
        isPlay = true;
        fStartTime = Time.time;
    }

    public void Update()
    {
        if (!isPlay) return;
        if (Time.time < fStartTime + delay) return;
        float curTime = Time.time - (fStartTime + delay);
        movePath.GetPointAtTime(curTime / time, out tempPos);
        transform.position = tempPos;
        if (curTime > time) {
            if (onMoveFinish != null) onMoveFinish(gameObject);
        }
    }
}