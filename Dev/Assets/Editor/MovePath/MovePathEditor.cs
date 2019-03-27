/***************************************************************


 *
 *
 * Filename:  	MovePathEditor.cs	
 * Summary: 	
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/05/19 11:45
 ***************************************************************/
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovePathMono))]
public class MovePathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MovePathMono mpm = target as MovePathMono;
        serializedObject.Update();
        NGUIEditorTools.SetLabelWidth(110f);

        GUILayout.Label("Path Name:         " + mpm.PathName);
        GUILayout.Space(1f);
        //NGUIEditorTools.DrawProperty("Path Name", serializedObject, "pathName");
        SerializedProperty sp = NGUIEditorTools.DrawProperty("Path Type", serializedObject, "ePathType");
        MovePathMono.ECameraPathType pathType = (MovePathMono.ECameraPathType)sp.intValue;

        if (pathType == MovePathMono.ECameraPathType.Straightline)
        {

            NGUIEditorTools.DrawProperty("Point Begin", serializedObject, "pointBegin");
            NGUIEditorTools.DrawProperty("Point End", serializedObject, "pointEnd");
        }
        else if (pathType == MovePathMono.ECameraPathType.Bezier || pathType == MovePathMono.ECameraPathType.Arcline || pathType == MovePathMono.ECameraPathType.Circle)
        {
            NGUIEditorTools.DrawProperty("Point Begin", serializedObject, "pointBegin");
            NGUIEditorTools.DrawProperty("Point End", serializedObject, "pointEnd");
            NGUIEditorTools.DrawProperty("Point Param", serializedObject, "pointParam");
        }
        else if (pathType == MovePathMono.ECameraPathType.CylinderSpirals)
        {
            NGUIEditorTools.DrawProperty("Point Begin", serializedObject, "pointBegin");
            NGUIEditorTools.DrawProperty("Point Param1", serializedObject, "pointParam");
            NGUIEditorTools.DrawProperty("Point Param2", serializedObject, "pointParam2");
            NGUIEditorTools.DrawProperty("Distance", serializedObject, "Distance");
            NGUIEditorTools.DrawProperty("PerCircleDistance", serializedObject, "PerCircleDistance");
        }


        SerializedProperty spLookat = NGUIEditorTools.DrawProperty("Look At Type", serializedObject, "elookAtType");
        MovePathMono.ELookAtType lookAtType = (MovePathMono.ELookAtType)spLookat.intValue;

        if (lookAtType == MovePathMono.ELookAtType.None)
        {
        }
        else if (lookAtType == MovePathMono.ELookAtType.LookAtPoint || lookAtType == MovePathMono.ELookAtType.LookAtPointReverse )
        {
            NGUIEditorTools.DrawProperty("Point Look At", serializedObject, "pointLookAt1");
        }
        else if (lookAtType == MovePathMono.ELookAtType.LookAtLine || lookAtType == MovePathMono.ELookAtType.LookAtLineReverse)
        {
            NGUIEditorTools.DrawProperty("Point Look At Begin", serializedObject, "pointLookAt1");
            NGUIEditorTools.DrawProperty("Point Look At End", serializedObject, "pointLookAt2");
            NGUIEditorTools.DrawProperty("Point Look At Offset", serializedObject, "PosLookAtOffset");
        }
        else if (lookAtType == MovePathMono.ELookAtType.LookAtTwoPoints)
        {
            NGUIEditorTools.DrawProperty("Point Look At Begin", serializedObject, "pointLookAt1");
            NGUIEditorTools.DrawProperty("Point Look At End", serializedObject, "pointLookAt2");
        }
        serializedObject.ApplyModifiedProperties();
    }
}
