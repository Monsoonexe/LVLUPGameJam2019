﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Customer))]
public class Customer_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var customer = (Customer)target;
        if (GUILayout.Button("Randomize Appearance Material"))
        {
            customer.RandomizeVisuals();
        }
    }
}
