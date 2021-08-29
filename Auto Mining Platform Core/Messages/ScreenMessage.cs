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
        public interface IOverview
        {
            string BuildOverview();
        }
        public interface IScreenMessage
        {
            IMessageQueueAppender GetInfoQueueAppender();
            IMessageQueueAppender GetWarningQueueAppender();
            string BuildMessage(ScriptState state);
        }
        /// <summary>
        /// Maintains the information that can be displayed on Screens
        /// </summary>
        public class ScreenMessage : IScreenMessage
        {
            private readonly ScriptConfig config;

            private readonly MessageQueue debugQueue;
            private readonly IndicatorInfo indicatorBar = new IndicatorInfo();

            private readonly MessageQueue infoQueue = new MessageQueue("Info");
            private readonly MessageQueue warningQueue = new MessageQueue("Warning");

            public ScreenMessage(MessageQueue debugQueue, ScriptConfig config)
            {
                this.debugQueue = debugQueue;
                this.config = config;
            }

            public IMessageQueueAppender GetInfoQueueAppender()
            {
                return infoQueue;
            }

            public IMessageQueueAppender GetWarningQueueAppender()
            {
                return warningQueue;
            }

            /// <summary>
            /// Construct the Message, by consuming all data stored in the queues.
            /// </summary>
            /// <returns></returns>
            public string BuildMessage(ScriptState state)
            {
                string result = "";
                result += debugQueue.ConsumeAll();
                if (config.ShowPlatformName) result += "Platform: " + config.MainTag + "\n";
                result += indicatorBar.Build(config.STATEDATA[state].Text);

                result += warningQueue.ConsumeAll();
                result += infoQueue.ConsumeAll();

                return result;
            }

        }
    }
}
