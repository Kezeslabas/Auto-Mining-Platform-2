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
    partial class Program : MyGridProgram
    {
        readonly BlockManager BLOCKMANAGER = new BlockManager();
        readonly StateManager STATEMANAGER = new StateManager();

        readonly MessageQueue DEBUGQUEUE = new MessageQueue("Debug");

        readonly ScriptConfig CONFIG;

        readonly ScreenMessage SCREENMESSAGE;
        readonly ScreenManager SCREENS;

        public Program()
        {
            CONFIG = new ScriptConfig(DEBUGQUEUE);
            SCREENMESSAGE = new ScreenMessage(DEBUGQUEUE,
                                              CONFIG);

            SCREENS = new ScreenManager(CONFIG, 
                                        STATEMANAGER, 
                                        SCREENMESSAGE, 
                                        GetMyScreen());

            BLOCKMANAGER.RegisterBlockConsumer(SCREENS);

            STATEMANAGER.RegisterStateConsumer(SCREENS);

            RefreshBlocks();
            DisplayMessages();
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Update100) != 0)
            {
                //Automatic run
            }
            else
            {
                //Manual Run
            }

            DisplayMessages();
        }

        private void RefreshBlocks()
        {
            BLOCKMANAGER.ClearAll();

            GridTerminalSystem.SearchBlocksOfName(CONFIG.MainTag, BLOCKMANAGER.GetBlockList());

            BLOCKMANAGER.ConsumeAll();
        }

        private void DisplayMessages()
        {
            if(CONFIG.UpdateDetailedInfo)Echo(SCREENS.CurrentMessage());
            SCREENS.DisplayOnAll();
        }

        private IMyTextSurface GetMyScreen()
        {
            return Me.SurfaceCount >= 1 ? Me.GetSurface(0) : null;
        }
    }
}
