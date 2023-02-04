using System;

namespace Script.Decorators
{
    public class WaterTile : TileDecorator
    {
        public override TileType Type => TileType.Water;
    }
}