using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(Hivemind))]
    [HarmonyPatch("Start")]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_Hivemind_Start
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___hp)
        {
            ___hp = ConfigureMe.ScaleEnemyHP(___hp, Network.connections.Length);
        }
    }
}