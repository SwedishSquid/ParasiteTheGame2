using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [NonSerialized] public IUsable thisItem; //must contain a pointer to constructed object after LoadData called

    public Vector2 ItemPosition;

    public string TypeName = "";
}
