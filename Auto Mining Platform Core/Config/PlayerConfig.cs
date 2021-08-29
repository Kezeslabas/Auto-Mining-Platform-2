using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Contains all values that are configurable by the player.
        /// </summary>
        public class PlayerConfig : ConfigInitializer
        {
            private readonly ConfigBuilder Builder = new ConfigBuilder();

            public PlayerConfig(IMessageQueueAppender debugQueue) : base(debugQueue)
            {
                AllToDefault();
            }

            private readonly string MINIG_PLATFORM_CONFIGURATION = "Mining Platform Configuration";

            // Highlighted Values
            private readonly string HIGHLIGHTED_SETTINGS = "Highlighted Settings (Hard)";
            public string MainTag;

            // Soft Vales
            private readonly string DISPLAY_SETTINGS = "Display Settings (Soft)";
            public bool ShowPlatformName; // - Done
            public bool LcdColorCoding; // - Done
            public bool UpdateDetailedInfo; // - Done
            public bool ShowAdvancedData;

            // Hard values
            private readonly string TAG_SETTINGS = "Tag Settings (Hard)";
            public string VerTag;
            public string HorTag;
            public string InvTag;

            public string StartTimerTag;
            public string PauseTimerTag;
            public string FinishTimerTag;
            public string AutoStartTimerTag;
            public string AutoPauseTimerTag;

            public void AllToDefault()
            {
                // Highlighted Values
                MainTag = "/Mine 01/";

                // Soft Vales
                ShowPlatformName = true;
                LcdColorCoding = true;
                UpdateDetailedInfo = true;
                ShowAdvancedData = false;

                // Hard values
                VerTag = "/Ver/";
                HorTag = "/Hor/";
                InvTag = "/Inv/";

                StartTimerTag = "/Start/";
                PauseTimerTag = "/Pause/";
                FinishTimerTag = "/Finish/";
                AutoStartTimerTag = "/Auto-Start/";
                AutoPauseTimerTag = "/Auto-Pause/";
        }

            /// <summary>
            /// Load the values form the provided Custom Data.
            /// If it is called as a context load, then a hard change will not be treated as critical change.
            /// </summary>
            /// <param name="customData"></param>
            /// <param name="isContextLoad"></param>
            /// <returns></returns>
            public bool LoadPlayerConfig(string customData, bool isContextLoad = false)
            {
                if (!InitFromCustomData(customData)) return false;

                // Highlighted Options
                HardChangeString(ref MainTag, Get(HIGHLIGHTED_SETTINGS, "MainTag"));

                // Soft Vales
                // -Display
                SoftChangeBoolean(ref ShowPlatformName, Get(DISPLAY_SETTINGS, "ShowPlatformName"));
                SoftChangeBoolean(ref LcdColorCoding, Get(DISPLAY_SETTINGS, "LcdColorCoding"));
                SoftChangeBoolean(ref UpdateDetailedInfo, Get(DISPLAY_SETTINGS, "UpdateDetailedInfo"));
                SoftChangeBoolean(ref ShowAdvancedData, Get(DISPLAY_SETTINGS, "ShowAdvancedData"));

                // Hard values
                // - Secondary Tags
                HardChangeString(ref VerTag, Get(TAG_SETTINGS, "VerTag"));
                HardChangeString(ref HorTag, Get(TAG_SETTINGS, "HorTag"));
                HardChangeString(ref InvTag, Get(TAG_SETTINGS, "InvTag"));

                // - Event Tags
                HardChangeString(ref StartTimerTag, Get(TAG_SETTINGS, "StartTimerTag"));
                HardChangeString(ref PauseTimerTag, Get(TAG_SETTINGS, "PauseTimerTag"));
                HardChangeString(ref FinishTimerTag, Get(TAG_SETTINGS, "FinishTimerTag"));
                HardChangeString(ref AutoStartTimerTag, Get(TAG_SETTINGS, "AutoStartTimerTag"));
                HardChangeString(ref AutoPauseTimerTag, Get(TAG_SETTINGS, "AutoPauseTimerTag"));

                if (!isContextLoad && IsHardChange())
                {
                    DebugQueue.Append("Hard Changes Detected!");
                    DebugQueue.Append("Use the SET command to Continue");
                }

                return true;
            }

            /// <summary>
            /// Constructs the Custom Data using the current values.
            /// </summary>
            /// <returns>Custom Data String</returns>
            public string BuildPlayerConfig()
            {
                return Builder.New()
                    .Header(MINIG_PLATFORM_CONFIGURATION)
                        //.Value("Version", Version)
                        .Comment(" You can Configure the script by changing the values below.")
                        .Comment(" Apply the changes with the REFRESH or SET command.")
                        .NewLine()
                        .Comment(" SOFT: Can be changed any time.")
                        .Comment(" HARD: When changed the script stops, and it has to be reset.")

                    .Section(HIGHLIGHTED_SETTINGS)
                        .NewLine()
                        .Value("MainTag", MainTag)

                    .Section(DISPLAY_SETTINGS)
                        .NewLine()
                        .Value("ShowPlatformName", ShowPlatformName)
                        .Value("LcdColorCoding", LcdColorCoding)
                        .Value("UpdateDetailedInfo", UpdateDetailedInfo)
                        .Value("ShowAdvancedData", ShowAdvancedData)

                    .Section(TAG_SETTINGS)
                        .NewLine()
                        .Value("VerTag", VerTag)
                        .Value("HorTag", HorTag)
                        .Value("InvTag", InvTag)
                        .NewLine()
                        .Value("StartTimerTag", StartTimerTag)
                        .Value("PauseTimerTag", PauseTimerTag)
                        .Value("FinishTimerTag", FinishTimerTag)
                        .Value("AutoStartTimerTag", AutoStartTimerTag)
                        .Value("AutoPauseTimerTag", AutoPauseTimerTag)

                    .End()
                    .Build();
            }
        }
    }
}
