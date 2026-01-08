using UnityEngine;
using HarmonyLib;
using GadgetCore.API;
using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using System.Reflection;

namespace ConfigureMe.Patches
{
    [HarmonyPatch(typeof(AttackCube))]
    [HarmonyPatch(nameof(AttackCube.SendDamage))]
    [HarmonyGadget("ConfigureMe")]
    /// Scales melee damage
    public static class Patch_AttackCube_SendDamage
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool insertedScaling = false;
            foreach (var instruction in instructions)
            {
                // scale damage right before checking whether to rpc the damage to the server or call TD locally
                if(!insertedScaling && instruction.opcode == OpCodes.Call && instruction.operand.ToString().Contains("isServer"))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1) { labels = instruction.labels }; // load array. Move jump labels to here
                    // Consume array on stack
                    yield return new CodeInstruction(OpCodes.Call, typeof(Patch_AttackCube_SendDamage).GetMethod(nameof(ScaleDamage), BindingFlags.Public | BindingFlags.Static));
                    instruction.labels = new List<Label>(); // Labels have been copied to newly inserted instruction. Not sure if should be null or empty list.
                    insertedScaling = true;
                }

                yield return instruction;
            }
        }

        public static void ScaleDamage(float[] TDArgs)
        {
            TDArgs[0] *= ConfigureMe.MeleeDamageMultiplier;
        }
    }
}