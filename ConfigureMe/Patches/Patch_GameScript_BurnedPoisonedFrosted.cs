using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.Frosted))]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_GameScript_Frosted
    {
        [HarmonyPrefix]
        public static void Prefix(ref int a)
        {
            if (a != 0)
            {
                a = Mathf.RoundToInt(a * ConfigureMe.FrostReceivedMultiplier);
                if (a == 0)
                    a = 1;
                if (a + GameScript.frost > ConfigureMe.FrostReceivedLimit)
                    a = Mathf.RoundToInt(ConfigureMe.FrostReceivedLimit - GameScript.frost);
            }
        }
    }

    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.Burned))]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_GameScript_Burned
    {
        [HarmonyPrefix]
        public static void Prefix(ref int a)
        {
            if (a != 0)
            {
                a = Mathf.RoundToInt(a * ConfigureMe.BurnReceivedMultiplier);
                if (a == 0)
                    a = 1;
                if (a + GameScript.burn > ConfigureMe.BurnReceivedLimit)
                    a = Mathf.RoundToInt(ConfigureMe.BurnReceivedLimit - GameScript.burn);
            }
        }
    }

    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.Poisoned))]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_GameScript_Poisoned
    {
        [HarmonyPrefix]
        public static void Prefix(ref int a)
        {
            if (a != 0)
            {
                a = Mathf.RoundToInt(a * ConfigureMe.PoisonReceivedMultiplier);
                if (a == 0)
                    a = 1;
                if (a + GameScript.poison > ConfigureMe.PoisonReceivedLimit)
                    a = Mathf.RoundToInt(ConfigureMe.PoisonReceivedLimit - GameScript.poison);
            }
        }
    }
}
