using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    public bool AlmostPassedOut { get; }

    public bool Dead { get; }

    public bool PassedOut { get; }

    public Vector2 Position { get; }

    public bool CanBeHit { get; }
}
