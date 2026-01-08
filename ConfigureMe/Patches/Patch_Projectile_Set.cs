using UnityEngine;
using HarmonyLib;
using GadgetCore.API;
using System.Collections.Generic;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(Projectile))]
    [HarmonyPatch(nameof(Projectile.Set))]
    [HarmonyGadget("ConfigureMe")]
    /// Scale gun and cannon damage
    public static class Patch_Projectile_Set
    {
        [HarmonyPostfix]
        public static void Postfix(Projectile __instance, ref int ___damage)
        {
            if(__instance.gunorcannon)
                ___damage = Mathf.RoundToInt(___damage * ConfigureMe.GunCannonDamageMultiplier);
        }
    }
}