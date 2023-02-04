using System;

namespace Script.Decorators
{
    public class SandTile : TileDecorator
    {
        public override TileType Type => TileType.Sand;
        public override bool CanBuildRoots => base.CanBuildRoots && true;
        private void Start()
        {
            
        }
    }
}
