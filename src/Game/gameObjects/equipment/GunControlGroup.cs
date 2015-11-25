using System;

namespace Clouds.Game
{
    [Flags]
    enum GunControlGroup
    {
        None = 0,
        Right = 1,
        Left = 2,
        Front = 4,
        Aft = 8,
    }
}