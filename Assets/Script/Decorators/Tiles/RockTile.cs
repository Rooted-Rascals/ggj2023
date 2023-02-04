using System;

namespace Script.Decorators
{
    public class RockTile : TileDecorator
    {
        public override TileType Type => TileType.Rock;
        public override bool CanBuildRoots => false;
    }
}
