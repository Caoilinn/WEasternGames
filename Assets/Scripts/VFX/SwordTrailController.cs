using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrailController : MonoBehaviour
{
    public MeleeWeaponTrail weaponTrailComponent;

    public void OnAnimation_IsLightAttackActive() {
        weaponTrailComponent.Emit = true;
    }

    public void OnAnimation_IsLightAttackDeactive() {
        weaponTrailComponent.Emit = false;
    }

    public void OnAnimation_IsHeavyAttackActive() {
        weaponTrailComponent.Emit = true;
    }

    public void OnAnimation_IsHeavyAttackDeactive() {
        weaponTrailComponent.Emit = false;
    }
}
