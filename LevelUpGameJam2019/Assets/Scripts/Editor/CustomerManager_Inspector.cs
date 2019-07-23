using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomerManager))]
public class CustomerManager_Inspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //var targetInspectorObject = (CustomerManager)target;
        //if (GUILayout.Button("Randomly Generate New Order [ALPHA]"))
        //{
        //    targetInspectorObject.RandomlyGenerateNewOrder();
        //}
    }
}
