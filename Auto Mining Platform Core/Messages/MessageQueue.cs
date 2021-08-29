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
    public partial class Program
    {
        /// <summary>
        /// Enables to Append to a <c>MessageQueue</c>
        /// </summary>
        public interface IMessageQueueAppender
        {
            void Append(string msg);
        }

        /// <summary>
        /// Stores Messages that can be consumed to create display information.
        /// Declare the name to use for the messages in the constructor.
        /// </summary>
        public class MessageQueue : IMessageQueueAppender
        {
            private readonly List<string> messages = new List<string>();
            private readonly string name;

            public MessageQueue(string name)
            {
                this.name = name;
            }

            public void Append(string msg)
            {
                messages.Add(msg);
            }

            public void Clear()
            {
                messages.Clear();
            }

            /// <summary>
            /// Construct the Message string by consuming all messages in the queue.
            /// The queue will be cleared after every message is consumed.
            /// </summary>
            /// <returns>Messages in one string</returns>
            public string ConsumeAll()
            {
                string result = "";

                messages.ForEach(p => result += "["+name+"]: " + p + "\n");
                messages.Clear();

                return result;
            }
        }
    }
}
