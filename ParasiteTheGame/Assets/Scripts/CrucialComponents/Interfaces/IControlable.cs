using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlable
{
    public bool CanBeCaptured { get; }
    public void ControlledUpdate(InputInfo inpInf);

    /// <summary>
    /// do not forget about carrying player after the controlled object
    /// </summary>
    public void UpdatePlayerPos(Transform playerTransform);

    /// <summary>
    /// called when picking or dropping items
    /// </summary>
    public void ActOnPickOrDrop();

    /// <summary>
    /// called when creature gets captured
    /// </summary>
    public void OnCapture(PlayerController player);

    /// <summary>
    /// called when creature gets released (player made jumpout)
    /// </summary>
    public void OnRelease(PlayerController player);
}
