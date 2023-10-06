using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace JoinCodeSender;

[BepInPlugin(ModGUID, ModName, ModVersion)]
internal class Plugin : BaseUnityPlugin
{
    public List<string> messagesList = new()
        { "Hey, server is online! Join code is: #JoinCode", "Join code: #JoinCode", "Join us by code #JoinCode" };


    private void Awake()
    {
        _self = this;

        #region config

        Config.SaveOnConfigSet = false;

        urlConfig = config("Main", "url", "", "");
        messageConfig = config("Main", "Messages",
            "Hey, server is online! Join code is: #JoinCode | Join code: #JoinCode | Join us by code #JoinCode", "");

        Config.SaveOnConfigSet = true;

        #endregion

        SetupWatcher();
        Config.SettingChanged += (_, _) => { UpdateConfiguration(); };
        Config.ConfigReloaded += (_, _) => { UpdateConfiguration(); };

        Config.Save();

        harmony.PatchAll();
    }

    #region values

    internal const string ModName = "JoinCodeSender", ModVersion = "1.2.0", ModGUID = "com.Frogger." + ModName;
    internal static Harmony harmony = new(ModGUID);
    internal static Plugin _self;

    #endregion

    #region ConfigSettings

    #region tools

    private static readonly string ConfigFileName = "com.Frogger.JoinCodeSender.cfg";
    private DateTime LastConfigChange;

    public static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description)
    {
        var configEntry = _self.Config.Bind(group, name, value, description);
        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description)
    {
        return config(group, name, value, new ConfigDescription(description));
    }

    public enum Toggle
    {
        On = 1,
        Off = 0
    }

    #endregion

    #region configs

    internal static ConfigEntry<string> urlConfig;
    internal static ConfigEntry<string> messageConfig;

    #endregion

    #endregion

    #region Config

    private void SetupWatcher()
    {
        FileSystemWatcher fileSystemWatcher = new(Paths.ConfigPath, ConfigFileName);
        fileSystemWatcher.Changed += ConfigChanged;
        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void ConfigChanged(object sender, FileSystemEventArgs e)
    {
        if ((DateTime.Now - LastConfigChange).TotalSeconds <= 3.0) return;

        LastConfigChange = DateTime.Now;
        try
        {
            Config.Reload();
        }
        catch
        {
            DebugError("Can't reload Config", true);
        }
    }

    private void UpdateConfiguration()
    {
        Task.Run(() =>
        {
            messagesList = new List<string>();
            var messegesListString = messageConfig.Value;
            var messeges = messegesListString.Split('|');
            foreach (var msg in messeges) messagesList.Add(msg);
        });

        Task.WaitAll();
        Debug("Configuration Received");
    }

    #endregion

    #region tools

    public static void Debug(object msg) { _self.Logger.LogInfo(msg); }

    public static void DebugError(object msg, bool showWriteToDev)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogError(msg);
    }

    public static void DebugWarning(string msg, bool showWriteToDev)
    {
        if (showWriteToDev) msg += "Write to the developer and moderator if this happens often.";

        _self.Logger.LogWarning(msg);
    }

    #endregion
}