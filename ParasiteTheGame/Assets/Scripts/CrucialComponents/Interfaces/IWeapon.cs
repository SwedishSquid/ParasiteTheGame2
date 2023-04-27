using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    /// <summary>
    /// handles rotation of weapon if needed
    /// and carries weapon after user
    /// </summary>
    /// <param name="desiredRotation">vector from centre</param>
    /// <param name="centre">center of IControllable user</param>
    /// <param name="radius">radius of a circle around which rotation may take place</param>
    public void RotateAndMove(Vector2 desiredRotation, Vector2 centre, float radius);

    /// <summary>
    /// called when player presses a button
    /// </summary>
    public void Use();

    public void OnPickUp(IControlable user);

    public void OnDropDown(IControlable user);
}
