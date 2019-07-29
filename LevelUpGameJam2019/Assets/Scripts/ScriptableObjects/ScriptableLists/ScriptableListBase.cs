using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableListBase<T> : RichScriptableObject
{
    public List<T> list;

    protected virtual void OnEnable()
    {
        ValidateArray();
    }

    protected virtual void ValidateArray()
    {
        while (list.Remove(default(T)) && list.Count != 0) { }//remove nulls until none or nothing left
    }
}
