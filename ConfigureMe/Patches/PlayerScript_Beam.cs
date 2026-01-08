using GadgetCore.API;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch("Beam")]
    [HarmonyGadget(nameof(ConfigureMe))]
    public static class PlayerScript_Beam
    {
        public static Coroutine CoroutineInstance;
        [HarmonyPrefix]
        public static void Prefix(PlayerScript __instance)
        {
            // For some reason the coroutine is not null but also not running after quitting to menu and re-entering
            // the game, so just stop and re-start it instead.
            if (CoroutineInstance != null)
            {
                __instance.StopCoroutine(CoroutineInstance);
            }
            CoroutineInstance = __instance.StartCoroutine(TimeScaleCoroutine());
            
        }

        static List<float> loggedUnexpected = new List<float>();
        const float UpdatesDelay = 0.4311f;
        public static IEnumerator TimeScaleCoroutine()
        {
            while (true)
            {
                float timeScale = Time.timeScale;
                if (timeScale != ConfigureMe.TimeScale)
                {
                    if (timeScale == 1)
                    {
                        Time.timeScale = ConfigureMe.TimeScale;
                        ConfigureMe.Log("Overrode timescale to " + Time.timeScale);
                    }
                    else if (timeScale != 0 && !loggedUnexpected.Contains(timeScale))
                    {
                        ConfigureMe.Log("Unexpected time scale: " + timeScale);
                        loggedUnexpected.Add(timeScale);
                    }
                }
                yield return new WaitForSecondsRealtime(UpdatesDelay);
            }
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
