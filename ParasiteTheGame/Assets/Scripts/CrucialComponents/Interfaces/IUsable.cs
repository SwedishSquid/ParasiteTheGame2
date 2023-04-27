using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    public void HandleUpdate(InputInfo inpInf);

    public void Throw(InputInfo inpInf);

    public void OnPickUp(IUser user);

    public void OnDropDown(IUser user);

    public void DealDamageByThrow(IDamagable damagable);
}
