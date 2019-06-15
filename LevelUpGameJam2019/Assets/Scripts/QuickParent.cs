using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuickParent : EditorWindow
{
private string newName;
public GameObject[] objList;
public Vector3[] objPosList;
public Vector3 averagePosition;
public Bounds itemBounds;

public int selGridInt = 0;
public string[] selStrings = new string[] { "Center Group", "Center World"};

[MenuItem("Window/QuickParent")]
public static void ShowWindow()
{
    GetWindow<QuickParent>("QuickParent");
}

void OnGUI()
{
    GUILayout.Space(10f);
    GUILayout.Label("Enter new Parent Object Name", EditorStyles.boldLabel);
    newName = EditorGUILayout.TextField("Name:", newName);

    if (GUILayout.Button("Create new Parent"))
    {
        MakeNewParent();
    }

    GUILayout.Space(10f);
    selGridInt = GUI.SelectionGrid(new Rect(60, 100, 200, 30), selGridInt, selStrings, 2);
}

void MakeNewParent()
{
    //Make a new GameObject
    var newGO = new GameObject();
    newGO.name = newName;

    //Get a list of all the currently selected objects
    objList = Selection.gameObjects;

    //If the Center Group checkbox is checked set the postion to the center of the group
    if (selGridInt == 0)
    {
        if (objList.Length > -1)
        {
            objPosList = new Vector3[objList.Length];

            for (int i = 0; i < (objList.Length); i++)
            {
                //Get the position of the object
                itemBounds.Encapsulate(objList[i].transform.position);
                Debug.Log(itemBounds);
            }
        }
        //Set the position to the bounds.center
        newGO.transform.position = itemBounds.center;
    }

    //Add all the objects as children to the new GO
    if (objList.Length > -1)
    {
        objPosList = new Vector3[objList.Length];

        for (int i = 0; i < (objList.Length); i++)
        {
            //Get the position of the object
            //Add the object's position to the position list
            //itemBounds.Encapsulate(objList[i].transform.position);

            //Add the object as a child to the new gameobject
            objList[i].transform.parent = newGO.transform;
        }
    }
}
}