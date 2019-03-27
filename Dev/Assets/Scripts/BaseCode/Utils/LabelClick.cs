using UnityEngine;
using System.Collections;

[SLua.CustomLuaClass]
public class LabelClick : MonoBehaviour {

    public delegate void VoidDelegate(string url);

    public VoidDelegate goToTask;

    void OnClick()
    {
     
        UILabel lbl = GetComponent<UILabel>();
        if (lbl != null)
        {
            string url = lbl.GetUrlAtPosition(UICamera.lastHit.point);
            if (!string.IsNullOrEmpty(url)) {
                if (goToTask != null) {
                    goToTask(url);
                }
            }
                
        }
    }
}
