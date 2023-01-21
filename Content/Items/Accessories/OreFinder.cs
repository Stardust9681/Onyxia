using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Onyxia.Content.Items.Accessories
{
    public abstract class OreFinder : ModItem
    {
        protected ushort[] tileType;
        protected int dustType;

        public sealed override void UpdateAccessory(Player player, bool hideVisual)
        {
            UpdateAcc(player, hideVisual);
            if (hideVisual)
                return;
            if(WorldUtils.Find(player.Center.ToPoint()-new Point(16, 16), new Searches.Rectangle(32, 32).Conditions(new Conditions.IsTile(tileType)), out Point pos))
            {
                for(int i = 0; i < 16; i++)
                {
                    Dust d = Dust.NewDustDirect(Vector2.Lerp(player.Center, pos.ToVector2(), i / 16f), 1, 1, dustType);
                    d.noGravity = true;
                    d.velocity = Vector2.Zero;
                }
            }
        }
        public virtual void UpdateAcc(Player player, bool hidden) { }
    }
    public class CopperFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CopperBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Copper, TileID.Amethyst };
            dustType = DustID.GemAmethyst;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
    public class TinFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TinBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Tin, TileID.Topaz };
            dustType = DustID.GemTopaz;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
    public class SilverFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SilverBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Silver, TileID.Sapphire };
            dustType = DustID.GemSapphire;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
    public class TungstenFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TungstenBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Tungsten, TileID.Emerald };
            dustType = DustID.GemEmerald;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
    public class GoldFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Gold, TileID.Ruby };
            dustType = DustID.GemRuby;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
    public class PlatinumFinder : OreFinder
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PlatinumBar;
        public override void SetDefaults()
        {
            tileType = new ushort[] { TileID.Platinum, TileID.Diamond};
            dustType = DustID.GemDiamond;
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
    }
}
