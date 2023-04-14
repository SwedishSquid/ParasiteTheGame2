using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputInfo
{
    public readonly Vector2 Axis { get; }
    public readonly Vector3 MousePos { get; }
    public readonly bool JumpoutPressed { get; }

    public readonly bool PickOrDropPressed { get; }

    public readonly bool FirePressed { get; }

    public InputInfo(Vector2 axes, Vector3 mousePos, bool jumpout, bool pickOrDrop, bool firePressed)
    {
        Axis = axes;
        MousePos = mousePos;
        JumpoutPressed = jumpout;
        PickOrDropPressed = pickOrDrop;
        FirePressed = firePressed;
    }

    /// <summary>
    /// return mouse direction from centre of screen
    /// </summary>
    public Vector2 GetMouseDir()
    {
        return new Vector2(MousePos.x - (Screen.width / 2), MousePos.y - (Screen.height / 2)).normalized;
    }
}
