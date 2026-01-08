using UnityEngine;
using HarmonyLib;
using GadgetCore.API;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(EnemyScript))]
    [HarmonyPatch("Initialize")]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_EnemyScript_Initialize
    {
        [HarmonyPostfix]
        public static void Postfix(ref int ___maxhp, ref int ___hp, ref int ___exp, ref int ___credits)
        {
            ___maxhp = ConfigureMe.ScaleEnemyHP(___maxhp, Network.connections.Length);
            ___hp = ___maxhp;
            ___exp = ConfigureMe.ScaleEnemyExp(___exp, Network.connections.Length);
            ___credits = ConfigureMe.ScaleEnemyCredits(___credits, Network.connections.Length);
            
        }
    }
}