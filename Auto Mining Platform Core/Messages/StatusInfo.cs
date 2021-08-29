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
        /// Shows an inidcator that updates each time the <c>ComponentData</c> method is called.
        /// WIP: -Add Status Information
        /// </summary>
        public class StatusInfo
        {
            private bool indicator = false;

            public string ComponentData()
            {
                indicator = !indicator;
                return indicator ? "[-/-/-/]" : "[/-/-/-]";
            }
        }


    }
}
