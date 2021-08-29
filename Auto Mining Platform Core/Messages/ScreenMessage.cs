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
        public interface IScreenMessage
        {
            IMessageQueueAppender GetInfoQueueAppender();
            IMessageQueueAppender GetWarningQueueAppender();
            string BuildMessage();
        }
        /// <summary>
        /// Maintains the information that can be displayed on Screens
        /// </summary>
        public class ScreenMessage : IScreenMessage
        {
            private readonly MessageQueue debugQueue;
            private readonly StatusInfo statusBar = new StatusInfo();

            private readonly MessageQueue infoQueue = new MessageQueue("Info");
            private readonly MessageQueue warningQueue = new MessageQueue("Warning");

            public ScreenMessage(MessageQueue debugQueue)
            {
                this.debugQueue = debugQueue;
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
            public string BuildMessage()
            {
                string result = "";
                result += debugQueue.ConsumeAll();
                result += statusBar.ComponentData();

                result += infoQueue.ConsumeAll();
                return result;
            }

        }
    }
}
