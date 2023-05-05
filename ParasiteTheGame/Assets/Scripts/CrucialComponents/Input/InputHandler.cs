using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler
{
    // Update is called once per frame
    public InputInfo GetInputInfo()
    {
        var axes = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var mousePos = Input.mousePosition;
        var jumpout = Input.GetButton("JumpOut");
        var pickOrDrop = Input.GetButtonDown("PickOrDrop");
        var fireeee = Input.GetButton("Fire");
        var throwItem = Input.GetButtonDown("ThrowItem");
        return new InputInfo(axes, mousePos, jumpout, pickOrDrop, fireeee, throwItem);
    }
}
