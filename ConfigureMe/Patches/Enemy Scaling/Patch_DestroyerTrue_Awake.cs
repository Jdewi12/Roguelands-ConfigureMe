using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(DestroyerTrue))]
    [HarmonyPatch("Awake")]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_DestroyerTrue_Awake
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___hp, ref int ___exp)
        {
            ___hp = ConfigureMe.ScaleEnemyHP(___hp, Network.connections.Length);
            ___exp = ConfigureMe.ScaleEnemyExp(___exp, Network.connections.Length);
            //___credits = ConfigureMe.ScaleEnemyCredits(___credits, Network.connections.Length); // todo: there's no variable for credits...
        }
    }
}