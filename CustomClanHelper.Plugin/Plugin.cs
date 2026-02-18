using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Text;

namespace CustomClanHelper.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);

        class ConfigDescriptionBuilder
        {
            public string English { get; set; } = "";
            public string French { get; set; } = "";
            public string German { get; set; } = "";
            public string Russian { get; set; } = "";
            public string Portuguese { get; set; } = "";
            public string Chinese { get; set; } = "";
            public string Spanish { get; set; } = "";
            public string ChineseTraditional { get; set; } = "";
            public string Korean { get; set; } = "";
            public string Japanese { get; set; } = "";

            public override string ToString()
            {
                StringBuilder builder = new();
                if (!string.IsNullOrEmpty(English)) builder.AppendLine(English);
                if (!string.IsNullOrEmpty(French)) builder.AppendLine(French);
                if (!string.IsNullOrEmpty(German)) builder.AppendLine(German);
                if (!string.IsNullOrEmpty(Russian)) builder.AppendLine(Russian);
                if (!string.IsNullOrEmpty(Portuguese)) builder.AppendLine(Portuguese);
                if (!string.IsNullOrEmpty(Chinese)) builder.AppendLine(Chinese);
                if (!string.IsNullOrEmpty(Spanish)) builder.AppendLine(Spanish);
                if (!string.IsNullOrEmpty(ChineseTraditional)) builder.AppendLine(ChineseTraditional);
                if (!string.IsNullOrEmpty(Korean)) builder.AppendLine(Korean);
                if (!string.IsNullOrEmpty(Japanese)) builder.AppendLine(Japanese);
                return builder.ToString().TrimEnd();
            }
        }

        public void Awake()
        {
            Logger = base.Logger;

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            
            var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            UpdateSettings();
            Config.SettingChanged += OnAnySettingChanged;
        }

        private void UpdateSettings()
        {
            Plugin.Logger.LogInfo("Settings Updated");
        }

        private void OnAnySettingChanged(object sender, SettingChangedEventArgs e)
        {
            Plugin.Logger.LogInfo("A Setting Changed");
            UpdateSettings();
        }
    }
}
