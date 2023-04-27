using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly LayerMask ControllablesLayer = LayerMask.GetMask("Controllables");
    
    public static readonly LayerMask CollidableItemsLayer = LayerMask.NameToLayer("CollidableItems");

    public static readonly LayerMask ItemsLayer = LayerMask.NameToLayer("Weapons");


    public static readonly LayerMask PickableItems = LayerMask.GetMask("Weapons") | LayerMask.GetMask("CollidableItems");
}
