using HarmonyLib;

namespace TimerExtensions;

public static class HarmonyHelpers
{
    extension(CodeMatcher matcher)
    {
        public CodeMatcher RemoveInstructions(params CodeMatch[] matches)
        {
            matcher.MatchForward(useEnd: false, matches)
                   .ThrowIfInvalid("Failed to find the match");
            Plugin.Logger.LogInfo($"inst: {matcher.Instruction}");
            matcher.RemoveInstructions(matches.Length);
            return matcher;
        }
    }
}