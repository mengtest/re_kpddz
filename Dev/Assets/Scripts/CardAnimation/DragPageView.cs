using UnityEngine;
using System.Collections;

public class DragPageView : MonoBehaviour {

    public CardAnimation pageView = null;

    void OnPress(bool pressed)
    {
        if (pageView && enabled && NGUITools.GetActive(gameObject))
        {
            pageView.press(pressed);
        }
    }

    /// <summary>
    /// Drag the object along the plane.
    /// </summary>

    void OnDrag(Vector2 delta)
    {
        if (pageView && NGUITools.GetActive(this))
            pageView.drag(delta);
    }
}
