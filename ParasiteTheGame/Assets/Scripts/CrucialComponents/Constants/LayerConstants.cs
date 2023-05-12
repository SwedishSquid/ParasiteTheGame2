using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerConstants
{
    public static readonly LayerMask ControllablesLayer = LayerMask.GetMask("Controllables");
    
    public static readonly LayerMask CollidableItemsLayer = LayerMask.NameToLayer("CollidableItems");

    public static readonly LayerMask ItemsLayer = LayerMask.NameToLayer("Weapons");

    public static readonly LayerMask PickableItems = LayerMask.GetMask("Weapons") | LayerMask.GetMask("CollidableItems");

    public static readonly LayerMask DamageTakersLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("Controllables") | LayerMask.GetMask("Walls");
}
