using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch(nameof(PlayerScript.TD))]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_PlayerScript_TD
    {
        [HarmonyPrefix]
        public static void Prefix(ref int dmg)
        {
            if (dmg > 0)
            {
                dmg = Mathf.RoundToInt(dmg * ConfigureMe.DamageTakenMultiplier);
                if (dmg > ConfigureMe.DamageTakenCap)
                    dmg = ConfigureMe.DamageTakenCap;
                if (dmg < 1)
                {
                    dmg = 1;
                }
            }
        }
    }

    [HarmonyPatch()]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_BigNumberCore_DoubleStatsTracker_TDDouble
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var BNC = Gadgets.GetGadget("BigNumberCore");
            if (BNC == null)
            {
                //ConfigureMe.Log("Not patching BigNumberCore because it wasn't found.");
                return null;
            }
            string targetClass = "BigNumberCore.DoubleStatsTracker, " + BNC.GetType().Assembly.FullName;
            MethodInfo toPatch = Type.GetType(targetClass)?.GetMethod("TDDouble", BindingFlags.NonPublic | BindingFlags.Instance);
            if (toPatch == null)
            {
                ConfigureMe.Log("Failed to patch " + targetClass);
            }
            return toPatch;
        }

        [HarmonyPrefix]
        public static void Prefix(ref byte[] dmgBytes)
        {
            double dmg = BytesToDouble(dmgBytes, 0);
            if (dmg > 0)
            {
                dmg *= ConfigureMe.DamageTakenMultiplier;
                // for BigNumberCore we ignore the cap if it's the max possible
                if (dmg > ConfigureMe.DamageTakenCap && ConfigureMe.DamageTakenCap != int.MaxValue) 
                    dmg = ConfigureMe.DamageTakenCap;
                if (dmg < 1)
                {
                    dmg = 1;
                }
            }
            dmgBytes = DoubleToBytes(dmg);
        }

        public static double BytesToDouble(byte[] value, int startIndex = 0)
        {
            return BitConverter.ToDouble(value, startIndex);
        }

        public static byte[] DoubleToBytes(double value)
        {
            return BitConverter.GetBytes(value);
        }

    }
}



/*using GadgetCore.API;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using System;
using System.Reflection;
using GadgetCore.Loader;

namespace ConfigureMe.Patches
{
    [HarmonyPatch()]
    [HarmonyGadget("ConfigureMe")]
    public static class Patch_All_TimeScale_Set
    {
        static MethodInfo timeScalePropertySetter = typeof(Time).GetProperty("timeScale", BindingFlags.Public | BindingFlags.Static).GetSetMethod();
        static MethodInfo newSetTimeScaleMethod = typeof(Patch_All_TimeScale_Set).GetMethod(nameof(NewSetTimeScale), BindingFlags.Public | BindingFlags.Static);

        // patch every method in every type in every assembly
        [HarmonyTargetMethod] 
        public static IEnumerable<MethodBase> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var assemblies = GadgetMods.ListAllMods().Select(mod => mod.Assembly).AddItem(
                typeof(GameScript).Assembly
                );
            foreach(var assembly in assemblies)
            {
                ConfigureMe.Log(assembly.FullName);
                foreach(var type in assembly.GetTypes())
                {
                    ConfigureMe.Log("|-" + type.FullName);
                    foreach(var method in type.GetMethods(flags))
                    {
                        if(method.GetMethodBody() != null)
                            yield return method;
                    }
                }
            }
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            
            foreach(CodeInstruction code in instructions)
            {
                if(code != null && code.opcode == OpCodes.Call && code.operand == timeScalePropertySetter)
                    code.operand = newSetTimeScaleMethod;
                if(AccessTools.)
                yield return code;
            }
        }

        public static void NewSetTimeScale(float val)
        {
            Time.timeScale = val * ConfigureMe.TimeScale;
        }
    }
}
*/
