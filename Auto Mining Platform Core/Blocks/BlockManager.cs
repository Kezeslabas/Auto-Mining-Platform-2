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
        /// Register the implementation to a BlockManager to give access to blocks is the Terminal System.
        /// </summary>
        public interface IBlockConsumer
        {
            /// <summary>
            /// Consume the provided block.
            /// </summary>
            /// <param name="block">The Block to Consume</param>
            void ConsumeBlock(IMyTerminalBlock block);

            /// <summary>
            /// Clear the blocks maintained by your class.
            /// </summary>
            void ClearBlocks();
        }

        /// <summary>
        /// Manages blocks and block consumers.
        /// <c>IBlockConsumer</c>s can be registered, and the class will handles the respective consmption and clear events.
        /// </summary>
        public class BlockManager
        {
            private readonly List<IBlockConsumer> blockConsumers = new List<IBlockConsumer>();
            private readonly List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

            /// <summary>
            /// Clear the lists of blocks from this class, and from each of the registered <c>IBlockConsumer</c>s.
            /// </summary>
            public void ClearAll()
            {
                blocks.Clear();
                blockConsumers.ForEach(p => p.ClearBlocks());
            }

            /// <summary>
            /// Register a BlockConsumer.
            /// </summary>
            /// <param name="blockConsumer">Consumer to register</param>
            public void RegisterBlockConsumer(IBlockConsumer blockConsumer)
            {
                blockConsumers.Add(blockConsumer);
            }

            /// <summary>
            /// </summary>
            /// <returns>List of Stored Blocks</returns>
            public List<IMyTerminalBlock> GetBlockList()
            {
                return blocks;
            }
            
            /// <summary>
            /// Each registered consumer will consume each block in the Block List of this class.
            /// </summary>
            public void ConsumeAll()
            {
                blocks.ForEach(p => blockConsumers.ForEach(k => k.ConsumeBlock(p)));
            }
        }
    }
}
