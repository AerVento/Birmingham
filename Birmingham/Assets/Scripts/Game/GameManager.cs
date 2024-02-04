using Framework.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameContext Context { get; private set; }
    }

}
