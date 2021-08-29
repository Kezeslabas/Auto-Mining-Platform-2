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
        /// Manage a state, that is either triggered or not.
        /// Use the <c>Continue()</c> to reset the state, 
        /// use the <c>Trigger</c> to trigger the state,
        /// and use the <c>IsTriggered()</c> to see if the state was triggered.
        /// </summary>
        public class TriggeredState
        {
            private bool triggered = false;

            public void Continue()
            {
                triggered = false;
            }

            public void Trigger()
            {
                triggered = true;
            }

            public bool IsTriggered()
            {
                return triggered;
            }
        }
    }
}
