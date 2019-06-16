using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SharkController))]
public class SharkController_Inspector : Editor
{
   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetInspectorObject = (SharkController)target;
        if (GUILayout.Button("Reset Shark Position"))
        {
            targetInspectorObject.ResetSharkPositionTest();
        }
    }
}
