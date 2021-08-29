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
        /// Custom Builder Class to help construct CustomData for the Programmable Block
        /// </summary>
        public class ConfigBuilder
        {
            private StringBuilder builder = new StringBuilder();

            public ConfigBuilder New()
            {
                builder.Clear();
                return this;
            }

            public ConfigBuilder Header(string header)
            {
                builder.Append("[" + header + "]\n");
                return this;
            }

            public ConfigBuilder Section(string section)
            {
                builder.Append("\n[" + section + "]\n");
                return this;
            }

            public ConfigBuilder Value<T>(string key, T value)
            {
                builder.Append(key + "=" + value.ToString() + "\n");
                return this;
            }

            public ConfigBuilder Comment(string comment)
            {
                builder.Append(";" + comment + "\n");
                return this;
            }

            public ConfigBuilder NewLine()
            {
                builder.Append("\n");
                return this;
            }

            public ConfigBuilder End()
            {
                builder.Append("\n---");
                return this;
            }

            public string Build()
            {
                return builder.ToString();
            }
        }
    }
}
