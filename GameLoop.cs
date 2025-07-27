#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archuniverse.Characters;
using Archuniverse.Combat;
using Archuniverse.Items;
using Archuniverse.Utilities;
using Archuniverse;


namespace Archuniverse
{
    public class GameLoop
    {
        private static GameLoop? _instance;
        public static GameLoop Instance => _instance ??= new GameLoop();

        private readonly List<ITickable> _tickables = new();

        public void RegisterTickable(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
                _tickables.Add(tickable);
        }

        public void UnregisterTickable(ITickable tickable)
        {
            _tickables.Remove(tickable);
        }

        public void TickAll(float deltaTime)
        {
            foreach (var tickable in _tickables.ToList())
            {
                tickable.Tick(deltaTime);
            }
        }

    }

}
