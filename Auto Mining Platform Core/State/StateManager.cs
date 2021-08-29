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
        public interface IStateProvider
        {
            void SetState(ScriptState state);

            ScriptState GetState();
        }

        public interface IStateConsumer
        {
            void ConsumeState(ScriptState state);
        }


        public class StateManager : IStateProvider
        {
            private readonly List<IStateConsumer> consumers = new List<IStateConsumer>();

            private ScriptState state;

            public ScriptState GetState()
            {
                return state;
            }

            public void SetState(ScriptState state)
            {
                this.state = state;
                CallConsumers();
            }

            public void RegisterStateConsumer(IStateConsumer consumer)
            {
                consumers.Add(consumer);
            }

            private void CallConsumers()
            {
                consumers.ForEach(p => p.ConsumeState(state));
            }
        }
    }
}
