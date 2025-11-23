using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace TimerExtensions;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public sealed class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger { get; private set; } = null!;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        gameObject.hideFlags = HideFlags.DontSaveInEditor;
        var harmony = new Harmony( MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(StatsManagerPatcher));
        harmony.PatchAll(typeof(LevelStatsPatcher));
    }
}