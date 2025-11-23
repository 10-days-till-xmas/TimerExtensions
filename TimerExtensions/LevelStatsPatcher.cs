using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using static HarmonyLib.AccessTools;
using static System.Reflection.Emit.OpCodes;

namespace TimerExtensions;

[HarmonyPatch(typeof(LevelStats))]
public static class LevelStatsPatcher
{
    public static float offset;

    [HarmonyTranspiler]
    [HarmonyDebug]
    [HarmonyPatch("CheckStats")]
    private static IEnumerable<CodeInstruction> CheckStatsTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);
        matcher.RemoveInstructions(
                    new CodeMatch(Ldarg_0),                                             // IL_0062: ldarg.0
                    new CodeMatch(Ldflda, Field(typeof(LevelStats), "minutes")),        // IL_0063: ldflda float32 LevelStats::minutes
                    new CodeMatch(Call, Method(typeof(float), nameof(float.ToString))), // IL_0068: call instance string [netstandard]System.Single::ToString()
                    new CodeMatch(Ldstr, ":"),                                          // IL_006d: ldstr ":"
                    new CodeMatch(Ldarg_0),                                             // IL_0072: ldarg.0
                    new CodeMatch(Ldflda),                                              // IL_0073: ldflda float32 LevelStats::seconds
                    new CodeMatch(Ldstr),                                               // IL_0078: ldstr "00.000"
                    new CodeMatch(Call),                                                // IL_007d: call instance string [netstandard]System.Single::ToString(string)
                    new CodeMatch(Call))                                                // IL_0082: call string [netstandard]System.String::Concat(string, string, string)
               .SearchForward(static ci => ci.opcode == Callvirt && ci.operand is MethodInfo { Name: "set_text" })
               .ThrowIfInvalid("Could not find set_text property call")
               .Insert(CodeInstruction.Call(static string () => GetTimeString()));
        return matcher.InstructionEnumeration();
    }

    private static string GetTimeString()
    {
        var secs = StatsManager.Instance.seconds;
        var checkSecs = secs - offset;
        return $"""
                <color=#26e045>{GetTimeString(checkSecs)}</color>
                {GetTimeString(secs)}
                """;
    }

    private static string GetTimeString(float seconds)
    {
        return $"{seconds / 60:N0}:{seconds % 60:00.000}";
    }
}