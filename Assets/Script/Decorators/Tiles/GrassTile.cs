using System;

namespace Script.Decorators
{
    public class GrassTile : TileDecorator
    {
        public override TileType Type => TileType.Grass;
        public override bool CanBuildRoots => base.CanBuildRoots && true;
        private void Start()
        {
            
        }
    }
}