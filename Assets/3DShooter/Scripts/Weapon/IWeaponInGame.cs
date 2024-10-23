using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IWeaponInGame
{
    public void Equip(ThirdPersonController player, Transform handlePoint, UnityAction OnOutOfAmmoCallBack = null);

    public void OnShoot();

    public void Reload();
}
