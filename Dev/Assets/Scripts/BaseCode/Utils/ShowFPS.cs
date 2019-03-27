using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {

    public float f_UpdateInterval = 0.5F;

    public const float m_KBSize = 1024.0f * 1024.0f;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    private string s_MemInfo = "";

    private GUIStyle style = null;

    public static bool bShow = false;

    void Start() 
    {
        //Application.targetFrameRate=60;

#if UNITY_EDITOR
        bShow = true;
#endif
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;

        style = new GUIStyle();
        style.normal.background = null;    //�������
        style.normal.textColor = new Color(1, 0, 0);   //��ɫ
        style.fontSize = 20;       //�����С
    }


    void OnGUI() 
    {
        if (!bShow)
        {
            return;
        }


        GUI.color = Color.red;
        GUI.Label(new Rect(0, 0, 200, 50), "FPS: " + f_Fps.ToString("f2"), style);

#if !UNITY_EDITOR
        //GUI.Label(new Rect(0, 60, 200, 50), "MEM: " + s_MemInfo, style);
#endif
    }

    void Update() 
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval) 
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }

        //s_MemInfo = GetMemoryInfo();
    }


    
    public string GetMemoryInfo()
    {
        float totalMemory = (float)(Profiler.GetTotalAllocatedMemory() / m_KBSize);
        float totalReservedMemory = (float)(Profiler.GetTotalReservedMemory() / m_KBSize);
        float totalUnusedReservedMemory = (float)(Profiler.GetTotalUnusedReservedMemory() / m_KBSize);
        float monoHeapSize = (float)(Profiler.GetMonoHeapSize() / m_KBSize);
        float monoUsedSize = (float)(Profiler.GetMonoUsedSize() / m_KBSize);

        return string.Format("TotalAllocatedMemory: {0}MB,\n TotalReservedMemory:{1}MB,\n TotalUnusedReservedMemory:{2}MB,\n MonoHeapSize:{3}MB,\n MonoUsedSize:{4}MB", totalMemory, totalReservedMemory, totalUnusedReservedMemory, monoHeapSize, monoUsedSize);

    }
}
