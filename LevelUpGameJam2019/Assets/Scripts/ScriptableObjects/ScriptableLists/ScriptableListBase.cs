using System.Collections.Generic;
using UnityEngine;

public class ScriptableListBase<T> : RichScriptableObject
{
    public List<T> list;

    private void OnEnable()
    {
        ValidateArray();
    }

    private void ValidateArray()
    {
        while (list.Remove(default(T)) && list.Count != 0) { }//remove nulls until none or nothing left
    }
}
