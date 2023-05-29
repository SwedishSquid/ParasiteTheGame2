using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InteractorInfo
{
    public ISavable Player;
    public ISavable Enemy;
    public ISavable Item;

    public InteractorInfo(ISavable player, ISavable enemy, ISavable item)
    {
        Player = player;
        Enemy = enemy;
        Item = item;
    }
}
