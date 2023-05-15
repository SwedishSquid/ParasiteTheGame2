using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //PlayerPosition is individual for each level

    public string ControlledGUID;

    public int Health;

    public bool IsInitialised = false;
}
