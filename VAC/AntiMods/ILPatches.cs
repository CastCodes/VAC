using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine.UI;
using VConfig;

namespace VAC.AntiMods
{
  internal class ILPatches
  {
    private static readonly ConstructorInfo _zpackageConstructor = AccessTools.Constructor(typeof (ZPackage));
    private static readonly MethodInfo _zpackageWriteStr = AccessTools.Method(typeof (ZPackage), "Write", new System.Type[1]
    {
      typeof (string)
    });
    private static readonly MethodInfo _zpackageWriteLong = AccessTools.Method(typeof (ZPackage), "Write", new System.Type[1]
    {
      typeof (long)
    });
    private static readonly MethodInfo _zpackageWriteByteArray = AccessTools.Method(typeof (ZPackage), "Write", new System.Type[1]
    {
      typeof (byte[])
    });
    private static readonly MethodInfo _uiSetText = AccessTools.Method(typeof (Text), "set_text", new System.Type[1]
    {
      typeof (string)
    });

    private static IEnumerable<CodeInstruction> SendPeerInfo_Transpile(
      IEnumerable<CodeInstruction> instructions)
    {
      if ((object) ILPatches._zpackageConstructor == null || (object) ILPatches._zpackageWriteStr == null || (object) ILPatches._zpackageWriteLong == null)
      {
        ZLog.LogError((object) "[AntiMods] Could not find ZPackage:ZPackage, ZPackage:Write(string) or ZPackage:Write(long)!");
        return instructions;
      }
      List<CodeInstruction> codeInstructionList = new List<CodeInstruction>(instructions);
      int lastIndex = codeInstructionList.FindLastIndex((Predicate<CodeInstruction>) (x => x.Calls(ILPatches._zpackageWriteByteArray)));
      if (lastIndex == -1)
      {
        ZLog.LogError((object) "[AntiMods] Could not get ZPackage:ZPackage() instruction!");
        return (IEnumerable<CodeInstruction>) codeInstructionList;
      }
      codeInstructionList.InsertRange(lastIndex + 1, (IEnumerable<CodeInstruction>) new CodeInstruction[3]
      {
        new CodeInstruction(OpCodes.Ldloc, (object) 0),
        new CodeInstruction(OpCodes.Ldstr, (object) VACPlugin.PluginsHash),
        new CodeInstruction(OpCodes.Callvirt, (object) ILPatches._zpackageWriteStr)
      });
      codeInstructionList.ForEach((Action<CodeInstruction>) (x => ZLog.Log((object) string.Format("[{0}] {1} -> {2}", (object) "AntiCheat", (object) x.opcode, (object) (x.operand?.ToString() ?? "<none>")))));
      return (IEnumerable<CodeInstruction>) codeInstructionList;
    }

    private static IEnumerable<CodeInstruction> ShowConnectError_Transpile(
      IEnumerable<CodeInstruction> instructions)
    {
      if ((object) ILPatches._zpackageConstructor == null || (object) ILPatches._zpackageWriteStr == null || (object) ILPatches._zpackageWriteLong == null)
      {
        ZLog.LogError((object) "[AntiMods] Could not find ZPackage:ZPackage, ZPackage:Write(string) or ZPackage:Write(long)!");
        return instructions;
      }
      List<CodeInstruction> codeInstructionList = new List<CodeInstruction>(instructions);
      int lastIndex = codeInstructionList.FindLastIndex((Predicate<CodeInstruction>) (x => (int) x.opcode.Value == (int) OpCodes.Ret.Value));
      if (lastIndex == -1)
      {
        ZLog.LogError((object) "[AntiMods] Could not get FejdStartup:ShowConnectError() instruction!");
        return (IEnumerable<CodeInstruction>) codeInstructionList;
      }
      codeInstructionList.InsertRange(lastIndex, (IEnumerable<CodeInstruction>) new CodeInstruction[5]
      {
        new CodeInstruction(OpCodes.Ldarg_0),
        new CodeInstruction(OpCodes.Ldfld, (object) VACPlugin.PluginsHash),
        new CodeInstruction(OpCodes.Ldstr, (object) "[AntiMods]: You have been kicked for using Prohibited Mods."),
        new CodeInstruction(OpCodes.Callvirt, (object) ILPatches._uiSetText),
        new CodeInstruction(OpCodes.Ret)
      });
      if(Configuration.Current.Server.debugmode)
        codeInstructionList.ForEach((Action<CodeInstruction>) (x => ZLog.Log((object) string.Format("[{0}] {1} -> {2}", (object) "AntiCheat", (object) x.opcode, (object) (x.operand?.ToString() ?? "<none>")))));
      return (IEnumerable<CodeInstruction>) codeInstructionList;
    }
  }
}