//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIRoundGrid), true)]
public class UIRoundGridEditor : UIWidgetContainerEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty sp = NGUIEditorTools.DrawProperty("Arrangement", serializedObject, "arrangement");
    //    public float R = 100f;//半径
    //public float angle = 90f;//间隔角度
    //public float startAngle = 0;//起始角度
    NGUIEditorTools.DrawProperty("  R", serializedObject, "R");
        NGUIEditorTools.DrawProperty("  angle", serializedObject, "angle");
        NGUIEditorTools.DrawProperty("  startAngle", serializedObject, "startAngle");

        if (sp.intValue < 2) {
            //bool columns = (sp.hasMultipleDifferentValues || (UIGrid.Arrangement)sp.intValue == UIGrid.Arrangement.Horizontal);

            //GUILayout.BeginHorizontal();
            //{
            //    sp = NGUIEditorTools.DrawProperty(columns ? "  Column Limit" : "  Row Limit", serializedObject, "maxPerLine");
            //    if (sp.intValue < 0) sp.intValue = 0;
            //    if (sp.intValue == 0) GUILayout.Label("Unlimited");
            //}
            //GUILayout.EndHorizontal();

            UIGrid.Sorting sort = (UIGrid.Sorting)NGUIEditorTools.DrawProperty("Sorting", serializedObject, "sorting").intValue;

            if (sp.intValue != 0 && (sort == UIGrid.Sorting.Horizontal || sort == UIGrid.Sorting.Vertical)) {
                EditorGUILayout.HelpBox("Horizontal and Vertical sortinig only works if the number of rows/columns remains at 0.", MessageType.Warning);
            }
        }

        NGUIEditorTools.DrawProperty("Pivot", serializedObject, "pivot");
        NGUIEditorTools.DrawProperty("Smooth Tween", serializedObject, "animateSmoothly");
        NGUIEditorTools.DrawProperty("Hide Inactive", serializedObject, "hideInactive");
        NGUIEditorTools.DrawProperty("Constrain to Panel", serializedObject, "keepWithinPanel");
        serializedObject.ApplyModifiedProperties();
    }
}
