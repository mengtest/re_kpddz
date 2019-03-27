using UnityEngine;
using System.Collections;
using EventManager;
using System.Collections.Generic;
public class PathDrawMono : MonoBehaviour
{
    public int selectPath = 10000;
    public string modelId = "11005";
    public bool isChangeModel = false;
    GameObject container;
    List<Transform> _besizerPoint=new List<Transform>();
    void Awake()
    {
        container = transform.Find("ptct").gameObject;
    }
    public void DelChild()
    {
        if (container == null)
            return;
        int count = container.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = container.transform.GetChild(0);
            if (child != null)
                DestroyImmediate(child.gameObject,true);
        }
        container.transform.DetachChildren();
    }
    void OnDrawGizmos()
    {
        Transform pre = null;
        container = transform.Find("ptct").gameObject;
        _besizerPoint.Clear();
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (pre == null)
            {
                pre = container.transform.GetChild(i);
            }
            else
            {
                Transform now = container.transform.GetChild(i);
                if (now.GetComponent<ComponentData>().Id == 1)
                {
                    _besizerPoint.Add(now);
                    continue;
                }
                else
                {
                    if (_besizerPoint.Count == 1)
                    {
                        Gizmos.color = new Color(255f, 255f, 255f);
                        //Gizmos.DrawLine(pointParam.transform.position, pre.position);
                        //Gizmos.DrawLine(pointParam.transform.position, now.transform.position);
                        MovePathBase bezier = new PathBezier(pre.localPosition, _besizerPoint[0].localPosition, _besizerPoint[0].localPosition, now.localPosition);

                        Vector3 tempPoint = Vector3.zero;
                        Gizmos.color = new Color(0f, 255f, 0f);
                        Vector3 vec = Vector3.zero;
                        //绘制贝塞尔曲线
                        for (int j = 1; j <= 100; j++)
                        {
                            bezier.GetPointAtTime((float)(j * 0.01), out vec);
                            //vec = BattleScene2DController.TransformToAdjustPos(vec);
                            /*vec = BattleScene2DMono.TransformPoint(vec);
                            if (j != 1)
                            { 
                                Gizmos.DrawLine(tempPoint, vec);
                            }
                            tempPoint = vec;*/
                        }
                        _besizerPoint.Clear();
                        pre = now;
                    }
                    else if (_besizerPoint.Count >= 2)
                    {
                        Gizmos.color = new Color(255f, 255f, 255f);
                        //Gizmos.DrawLine(pointParam.transform.position, pre.position);
                        //Gizmos.DrawLine(pointParam.transform.position, now.transform.position);
                        MovePathBase bezier = new PathBezier(pre.localPosition, _besizerPoint[0].localPosition, _besizerPoint[1].localPosition, now.localPosition);

                        Vector3 tempPoint = Vector3.zero;
                        Gizmos.color = new Color(0f, 255f, 0f);
                        Vector3 vec = Vector3.zero;
                        //绘制贝塞尔曲线
                        for (int j = 1; j <= 100; j++)
                        {
                            bezier.GetPointAtTime((float)(j * 0.01), out vec);
                            //vec = BattleScene2DController.TransformToAdjustPos(vec);
                            /*vec = BattleScene2DMono.TransformPoint(vec);
                            if (j != 1)
                            {
                                Gizmos.DrawLine(tempPoint, vec);
                            }
                            tempPoint = vec;*/
                        }
                        _besizerPoint.Clear();
                        pre = now;
                    }
                    else
                    {
                        Gizmos.color = new Color(255f, 255f, 255f);
                        Gizmos.DrawLine(pre.position, now.position);
                        pre = now;
                    }
                }

                
            }

        }
    }
}
