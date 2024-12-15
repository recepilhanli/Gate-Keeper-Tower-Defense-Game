using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class LayerMasks
    {
        public const int LAYER_DEFAULT = 1 << 0;
        public const int LAYER_PLAYER = 1 << 6;
        public const int LAYER_ENEMY = 1 << 7;
        public const int LAYER_BOUND = 1 << 9;
        public const int LAYER_DEFAULT_AND_ENEMY = LAYER_DEFAULT | LAYER_ENEMY;
        public const int LAYER_DEFAULT_AND_PLAYER = LAYER_DEFAULT | LAYER_PLAYER;
        public const int IGNORE_BOUND = ~LAYER_BOUND;
    }
}