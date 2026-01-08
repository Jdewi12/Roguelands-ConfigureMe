using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch("GetFinalStat")]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_GameScript_GetFinalStat
    {        
        [HarmonyPostfix]
        public static void Postfix(int a, ref int __result)
        {
            __result += ConfigureMe.BonusStats[a];
        }
    }

    [HarmonyPatch()]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class Patch_BigNumberCore_DoubleCoreAPI_GetFinalStat
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var BNC = Gadgets.GetGadget("BigNumberCore");
            if(BNC == null)
            {
                //ConfigureMe.Log("Not patching BigNumberCore because it wasn't found.");
                return null;
            }
            string targetClass = "BigNumberCore.DoubleCoreAPI, " + BNC.GetType().Assembly.FullName;
            MethodInfo toPatch = Type.GetType(targetClass)?.GetMethod("GetFinalStat", BindingFlags.Public | BindingFlags.Static);
            if(toPatch == null)
            {
                ConfigureMe.Log("Failed to patch " + targetClass);
            }
            return toPatch;
        }
        [HarmonyPostfix]
        public static void Postfix(int a, ref double __result)
        {
            __result += ConfigureMe.BonusStats[a];
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
