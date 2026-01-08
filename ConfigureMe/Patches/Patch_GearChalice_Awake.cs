using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(GearChalice))]
    [HarmonyPatch(nameof(GearChalice.Awake))]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_GearChalice_Awake
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___value)
        {
            ___value = Mathf.RoundToInt(___value * ConfigureMe.GearExpMultiplier);
        }
    }
}