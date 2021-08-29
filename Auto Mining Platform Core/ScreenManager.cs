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
        /// Manages screens, that are added to it.
        /// </summary>
        public class ScreenManager : IBlockConsumer
        {
            private readonly ScriptConfig config;

            private readonly HashSet<IMyTextSurface> displays = new HashSet<IMyTextSurface>();
            
            private readonly IMyTextSurface coreDisplay;

            private readonly TriggeredState messageCycle = new TriggeredState();
            private string currentMessage;

            private readonly IScreenMessage screenMessage;

            public ScreenManager(ScriptConfig config, IScreenMessage screenMessage ,IMyTextSurface coreDisplay)
            {
                this.config = config;
                this.screenMessage = screenMessage;
                this.coreDisplay = coreDisplay;
                AddCoreDisplay();
            }

            /// <summary>
            /// Display the current Message on all screens
            /// </summary>
            public void DisplayOnAll()
            {
                CurrentMessage();

                foreach (IMyTextSurface surface in displays)
                {
                    surface.WriteText(currentMessage);
                }

                messageCycle.Continue();
            }

            /// <summary>
            /// Get The Current Message of this message cycle.
            /// If the current message on this cycle was not built yet, 
            /// then builds it, and triggers the current message cycle.
            /// </summary>
            /// <returns>Current message of the message cycle.</returns>
            public string CurrentMessage()
            {
                if (messageCycle.IsTriggered()) return currentMessage;

                currentMessage = screenMessage.BuildMessage();

                messageCycle.Trigger();
                return currentMessage;
            }

            /// <summary>
            /// Add the CoreDisplay to the display list.
            /// </summary>
            private void AddCoreDisplay()
            {
                if(coreDisplay != null)
                {
                    displays.Add(coreDisplay);
                }
            }

            /// <summary>
            /// </summary>
            /// <returns>Number of Displays currently added</returns>
            public int GetDisplayCount()
            {
                return displays.Count();
            }

            /// <summary>
            /// Clear all Blocks from the display list, then re-add the core display.
            /// </summary>
            public void ClearBlocks()
            {
                displays.Clear();
                AddCoreDisplay();
            }

            /// <summary>
            /// Consume the Provided Block
            /// </summary>
            /// <param name="block"></param>
            public void ConsumeBlock(IMyTerminalBlock block)
            {
                AddDisplay(block);
            }

            private void SetDisplayColor(IMyTextSurface textSurface)
            {
                return;
            }

            /// <summary>
            /// Add an <c>IMyTextPanel</c> to the display list,
            /// or, if the block is an <c>IMyTextSurfaceProvider</c>,
            /// then decode wich surface should be added form it's custom data
            /// </summary>
            /// <example>
            /// @1 /Mine 01/
            /// </example>
            /// <param name="block"></param>
            private void AddDisplay(IMyTerminalBlock block)
            {
                if(block is IMyTextPanel)
                {
                    AddDisplay(block as IMyTextSurface);
                }
                else if(block is IMyTextSurfaceProvider)
                {
                    string stringBuffer;
                    int intBuffer;
                    IMyTextSurfaceProvider providerBuffer;

                    Array.ForEach(block.CustomData.Split('\n'), s =>
                    {
                        if (s.StartsWith("@"))
                        {
                            stringBuffer = s.Substring(1);
                            if(stringBuffer.Contains(config.MainTag))
                            {
                                stringBuffer = stringBuffer.Replace(config.MainTag, "");
                                if(Int32.TryParse(stringBuffer, out intBuffer))
                                {
                                    providerBuffer = block as IMyTextSurfaceProvider;
                                    if(providerBuffer.SurfaceCount > intBuffer)
                                    {
                                        AddDisplay(providerBuffer.GetSurface(intBuffer));
                                    }
                                }
                            }
                        }
                    });
                }
            }

            /// <summary>
            /// Add a <c>IMyTextSurface</c> to th display list,
            /// and set the content type and display color.
            /// </summary>
            /// <param name="textSurface"></param>
            private void AddDisplay(IMyTextSurface textSurface)
            {
                textSurface.ContentType = ContentType.TEXT_AND_IMAGE;
                SetDisplayColor(textSurface);
                displays.Add(textSurface);
            }
        }
    }
}
