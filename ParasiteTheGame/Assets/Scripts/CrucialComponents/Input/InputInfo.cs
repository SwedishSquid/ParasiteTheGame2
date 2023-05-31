using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct InputInfo
{
    public readonly Vector2 Axis { get; }
    public readonly Vector3 MouseDirection { get; }
    public readonly bool JumpoutPressed { get; }

    public readonly bool PickOrDropPressed { get; }

    public readonly bool FirePressed { get; }

    public readonly bool ThrowItemPressed { get; }

    public readonly bool SuperAttackPressed { get; }
    
    public InputInfo(Vector2 axes, Vector3 mouseDirection, bool jumpout, bool pickOrDrop, bool firePressed, bool throwItem) : this(axes, mouseDirection, jumpout, pickOrDrop, firePressed, throwItem, false)
    {
    }

    public InputInfo(Vector2 axes, Vector3 mouseDirection, bool jumpout, bool pickOrDrop, bool firePressed, bool throwItem, bool superAttack)
    {
        Axis = axes;
        MouseDirection = mouseDirection;
        JumpoutPressed = jumpout;
        PickOrDropPressed = pickOrDrop;
        FirePressed = firePressed;
        ThrowItemPressed = throwItem;
        SuperAttackPressed = superAttack;
    }

    /// <summary>
    /// return mouse direction from centre of screen
    /// </summary>
    public Vector2 GetMouseDir()
    {
        return MouseDirection.normalized;
    }

    public InputInfo ConstructForFrozen()
    {
        return new InputInfo(Vector2.zero, MouseDirection, JumpoutPressed, false, false, false);
    }
}
