using UnityEngine;
using HarmonyLib;
using GadgetCore.API;
using System.Collections.Generic;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(Projectile))]
    [HarmonyPatch(nameof(Projectile.SetStaff))]
    [HarmonyGadget("ConfigureMe")]
    /// Scale staff damage
    public static class Patch_Projectile_SetStaff
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___damage)
        {
            ___damage = Mathf.RoundToInt(___damage * ConfigureMe.StaffDamageMultiplier);
        }
    }
}