using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler
{
    public InputInfo GetInputInfo()
    {
        var axes = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var mousePos = Input.mousePosition;
        var jumpout = Input.GetButton("JumpOut");
        var pickOrDrop = Input.GetButtonDown("PickOrDrop");
        var fireeee = Input.GetButton("Fire");
        var throwItem = Input.GetButtonDown("ThrowItem");

        var mouseDir = new Vector2(mousePos.x - (Screen.width / 2), mousePos.y - (Screen.height / 2)).normalized;

        return new InputInfo(axes, mouseDir, jumpout, pickOrDrop, fireeee, throwItem);
    }
}
