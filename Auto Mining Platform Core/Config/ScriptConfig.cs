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
        /// The States that are allowed for he script
        /// </summary>
        public enum ScriptState
        {
            UNDEFINED,
            INIT,
            SET
        }

        /// <summary>
        /// Extension of <c>PlayerConfig</c>
        /// Contains all config settings for the script.
        /// </summary>
        public class ScriptConfig : PlayerConfig
        {
            public readonly Dictionary<ScriptState, StateData> STATEDATA = new Dictionary<ScriptState, StateData> 
            {
                { ScriptState.UNDEFINED,    new StateData(Color.Gray,          "Undefined State")   },
                { ScriptState.INIT,         new StateData(Color.White,         "Initializing...")   },
                { ScriptState.SET,          new StateData(Color.Magenta,       "Set")               },
            };

            public ScriptConfig(IMessageQueueAppender debugQueue) : base(debugQueue)
            {

            }
        }

        public struct StateData
        {
            public Color Color { get; }
            public string Text { get; }

            public StateData(Color color, string text)
            {
                Color = color;
                Text = text;
            }
        }
    }
}
