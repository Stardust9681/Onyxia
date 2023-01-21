using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Onyxia.Content.Globals
{
    public class OnyxiaProjectile : GlobalProjectile
    {
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == ProjectileID.FallingStar)
            {
                if (Terraria.Star.starfallBoost > 200 && Main.rand.NextFloat() < .01f)
                {
                    WorldGen.gen = true;
                    WorldGen.noTileActions = true;
                    Dictionary<ushort, int> tileOutput = new Dictionary<ushort, int>();
                    ushort[] tileTypes = new ushort[] { TileID.Dirt, TileID.Grass, TileID.Stone, TileID.Mud, TileID.JungleGrass };
                    WorldUtils.Gen(
                        new Point((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f)),
                        new Shapes.Circle(2),
                        new Actions.TileScanner().Output(tileOutput)
                        );

                    foreach (ushort key in tileOutput.Keys)
                    {
                        if (!tileTypes.Contains(key))
                            return base.OnTileCollide(projectile, oldVelocity);
                        tileOutput.Remove(key);
                    }

                    WorldUtils.Gen(
                        new Point((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f)),
                        new Shapes.Circle(2),
                        Actions.Chain(
                            new Actions.ClearTile(),
                            new Actions.PlaceTile(TileID.Sunplate),
                            new Actions.SetFrames(true)
                        ));

                    WorldGen.noTileActions = false;
                    WorldGen.gen = false;
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
    }
}
