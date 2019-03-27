using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIRoundGrid : UIGrid {
    public float R = 100f;//半径
    public float angle = 90f;//间隔角度
    public float startAngle = 0;//起始角度

    protected override void ResetPosition(List<Transform> list)
    {
        mReposition = false;

        // Epic hack: Unparent all children so that we get to control the order in which they are re-added back in
        // EDIT: Turns out this does nothing.
        //for (int i = 0, imax = list.Count; i < imax; ++i)
        //	list[i].parent = null;

        
        //Transform myTrans = transform;
        float curRadians = 0;
        
        // Re-add the children in the same order we have them in and position them accordingly
        for (int i = 0, imax = list.Count; i < imax; ++i) {
            Transform t = list[i];
            // See above
            //t.parent = myTrans;

            Vector3 pos = t.localPosition;
            float depth = pos.z;

            curRadians = Mathf.PI * ((startAngle + i * angle) / 180);
            float posX = Mathf.Sin(curRadians) * R;
            float posY = Mathf.Cos(curRadians) * R;
            pos = new Vector3(posX, posY, depth);

            //if (arrangement == Arrangement.CellSnap) {
            //    if (cellWidth > 0) pos.x = Mathf.Round(pos.x / cellWidth) * cellWidth;
            //    if (cellHeight > 0) pos.y = Mathf.Round(pos.y / cellHeight) * cellHeight;
            //} else pos = (arrangement == Arrangement.Horizontal) ?
            //      new Vector3(cellWidth * x, -cellHeight * y, depth) :
            //      new Vector3(cellWidth * y, -cellHeight * x, depth);

            if (animateSmoothly && Application.isPlaying) {
                SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
                sp.updateScrollView = true;
                sp.ignoreTimeScale = true;
            } else t.localPosition = pos;
        }
    }
}
