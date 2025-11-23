using HarmonyLib;

namespace TimerExtensions;

[HarmonyPatch(typeof(StatsManager))]
public static class StatsManagerPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void StatsManagerAwakePostfix(StatsManager __instance)
    {
        StatsManager.checkpointRestart += OnCheckpointAction;
    }

    private static void OnCheckpointAction() => LevelStatsPatcher.offset = StatsManager.Instance.seconds;
}