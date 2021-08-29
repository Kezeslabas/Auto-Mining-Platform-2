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
        /// Has the neccesary methods to get and parse data from the Custom Data of a Block
        /// </summary>
        public class ConfigInitializer
        {
            protected readonly IMessageQueueAppender DebugQueue;
            private readonly MyIni INI = new MyIni();

            private bool hardChange;

            public ConfigInitializer(IMessageQueueAppender debugQueue)
            {
                DebugQueue = debugQueue;
            }

            /// <summary>
            /// Determines if the current internal state has encountered a hard change or not.
            /// A hard change is a change, that affect critical parts of your script.
            /// </summary>
            /// <returns></returns>
            public bool IsHardChange()
            {
                return hardChange;
            }

            /// <summary>
            /// Initilizes the internal memory based on teh provided custom data.
            /// </summary>
            /// <param name="customData">Custom Data of a block</param>
            /// <returns>True if the initialization was a success, false otherwise.</returns>
            protected bool InitFromCustomData(string customData)
            {
                INI.Clear();
                hardChange = false;
                MyIniParseResult _iniResult;
                if (customData == "" || !INI.TryParse(customData, out _iniResult))
                {
                    DebugQueue.Append("Cannot read Custom Data!");
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Find the value with the provided section & field pair int he initilized internal memory.
            /// </summary>
            /// <param name="section">Section int the Custom Data</param>
            /// <param name="field">Field in the section</param>
            /// <returns><c>MyIniValue</c> of the section & field pair</returns>
            public MyIniValue Get(string section, string field)
            {
                return INI.Get(section, field);
            }

            public bool CheckIfEmpty(MyIniValue iniVal)
            {
                if (iniVal.IsEmpty)
                {
                    DebugQueue.Append(iniVal.Key + " not found in Config!");
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Replaces the original value with the new value.
            /// </summary>
            /// <typeparam name="T">Type of the value</typeparam>
            /// <param name="originalVal">Original Value</param>
            /// <param name="newVal">New Value</param>
            public void SoftChange<T>(ref T originalVal, T newVal)
            {
                originalVal = newVal;
            }

            /// <summary>
            /// Replaces the original value with the new value.
            /// If the original value is changed, then changes the internal state to indicate, that a hard change occured.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="originalVal"></param>
            /// <param name="newVal"></param>
            public void HardChange<T>(ref T originalVal, T newVal) where T : IEquatable<T>
            {
                if (!originalVal.Equals(newVal))
                {
                    originalVal = newVal;
                    hardChange = true;
                }
            }

            /// <summary>
            /// Extension of <c>SoftChange()</c> to handle a soft changeable boolean value.
            /// </summary>
            /// <param name="originalVal"></param>
            /// <param name="iniVal"></param>
            public void SoftChangeBoolean(ref bool originalVal, MyIniValue iniVal)
            {
                if (!CheckIfEmpty(iniVal))
                {
                    bool _bool;
                    if (!Boolean.TryParse(iniVal.ToString(), out _bool))
                    {
                        DebugQueue.Append("Cannot Parse " + iniVal.Key);
                        return;
                    }
                    SoftChange(ref originalVal, _bool);
                }
            }

            /// <summary>
            /// Extension of <c>HardChange()</c> to handle a hard changeable string value.
            /// </summary>
            /// <param name="originalVal"></param>
            /// <param name="iniVal"></param>
            public void HardChangeString(ref string originalVal, MyIniValue iniVal)
            {
                if (!CheckIfEmpty(iniVal))
                {
                    HardChange(ref originalVal, iniVal.ToString());
                }
            }
        }
    }
}
