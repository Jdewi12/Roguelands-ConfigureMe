using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(ScarabScript))]
    [HarmonyPatch("Awake")]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_ScarabScript_Awake
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___hp)
        {
            ___hp = ConfigureMe.ScaleEnemyHP(___hp, Network.connections.Length);
        }
    }
}