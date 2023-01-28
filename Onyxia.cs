using System;
using System.Reflection;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.GameContent.Generation;

using Onyxia.Content.DamageClasses.Technician;
using Onyxia.Assets;
using Onyxia.Core.Utils;
using Onyxia.Content;
using System.IO;

namespace Onyxia
{
    public class Onyxia : Mod
    {
        public override void Load()
        {
            //AssetLoader.Initialise();
            base.Load();
        }
        public override void Unload()
        {
            //AssetLoader.Disengage();
            base.Unload();
        }
    }

    public class OnyxiaSystem : ModSystem
    {
        //internal static ChipUIState tinkererState;
        internal static ChipUIState tinkerState;
        private static UserInterface _techChipInterface;
        public static bool TinkerStateActive
        {
            get;
            private set;
        }

        public static EntityPlatform[] Platform = new EntityPlatform[MaxPlatforms];
        public const int MaxPlatforms = 100;

        internal ShieldState shieldState;
        private UserInterface _shieldInterface;
        internal EnergyState energyState;
        private UserInterface _energyInterface;
        public override void Load()
        {
            shieldState = new ShieldState();
            shieldState.Activate();
            _shieldInterface = new UserInterface();
            _shieldInterface.SetState(shieldState);
            tinkerState = new ChipUIState();
            tinkerState.Activate();
            _techChipInterface = new UserInterface();
            _techChipInterface.SetState(null);
            TinkerKeybind = KeybindLoader.RegisterKeybind(Mod, "Onyxia::Tinker", Microsoft.Xna.Framework.Input.Keys.X);
            energyState = new EnergyState();
            energyState.Activate();
            _energyInterface = new UserInterface();
            _energyInterface.SetState(energyState);
            
        }
        public override void Unload()
        {
            TinkerKeybind = null;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            _shieldInterface?.Update(gameTime);
            _techChipInterface?.Update(gameTime);
            _energyInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //
            GameTime gt = new GameTime();
            //Up and stolen from ExampleMod, shoot me :D
            int mouseTextIndex = -1;

            if (Main.LocalPlayer.GetModPlayer<Technician>().hasShield)
            {
                mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (mouseTextIndex != -1)
                {
                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                        "Onyxia: Technician Shield",
                        () =>
                        {
                            _shieldInterface?.Draw(Main.spriteBatch, gt);
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
            mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Onyxia: Chip Tinkering",
                    () =>
                    {
                        if(_techChipInterface.CurrentState != null && TinkerStateActive)// && _techChipInterface.IsVisible)
                            _techChipInterface?.Draw(Main.spriteBatch, gt);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            if(Main.LocalPlayer.HeldItem.ModItem.GetType().IsSubclassOf(typeof(TechItem)))
            {
                mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if(mouseTextIndex != -1)
                {
                    layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                        "Onyxia: Technician Energy",
                        () =>
                        {
                            _energyInterface?.Draw(Main.spriteBatch, gt);
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        
        public static void OpenTinkerUI(TechItem item)
        {
            Main.NewText("Opening...");
            tinkerState = new ChipUIState(item);
            tinkerState.Activate();
            _techChipInterface.SetState(tinkerState);
            TinkerStateActive = true;
        }
        public static void CloseTinkerUI()
        {
            Main.NewText("Closing...");
            tinkerState.Deactivate();
            _techChipInterface.SetState(null);
            //tinkererState = null;
            TinkerStateActive = false;
            //Main.NewText(TinkerStateActive);
        }
        

        public static ModKeybind TinkerKeybind
        {
            get;
            private set;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            //base.ModifyWorldGenTasks(tasks, ref totalWeight);
            int index = tasks.FindIndex(x => x.Name == "Ice");
            if (index != -1)
            {
                tasks.Insert(index+1, new PassLegacy("Onyxia::Extended Tundra", (progress, config) =>
                {
                    progress.Message = "Freezing over Helheim...";
                    GenerateHelheim();
                }));
            }
            else
            {
                Logging.PublicLogger.Debug("Onyxia::Extended Tundra [FAILED]");
            }
        }
        private void GenerateHelheim()
        {
            int w = Math.Abs(WorldGen.snowOriginLeft - WorldGen.snowOriginRight);
            int h = Math.Abs(WorldGen.snowBottom - Main.UnderworldLayer);
            Point start = new Point(Math.Min(WorldGen.snowOriginLeft, WorldGen.snowOriginRight)-(w/2), WorldGen.snowBottom);
            w *= 2;
            Logging.PublicLogger.Debug("Onyxia::Extended Tundra at: " + start + ", with size: " + new Point(w, h));
            WorldUtils.Gen(start+new Point(20, 0), new Shapes.Rectangle(w-40, h), new Actions.Clear());
            NoiseGen gen1 = new NoiseGen(w, h);
            float[,] vals = gen1.GenerateNoise();
            for(int j = 0; j < h; j++)
            {
                for(int i = 0; i < w; i++)
                {
                    if (WorldGen.TileEmpty(i, j))
                    {
                        if (vals[i, j] > .5f)
                            WorldGen.PlaceTile(start.X + i, start.Y + j, TileID.IceBrick);//, default, i>20&&i<w-20);
                        else if (vals[i, j] > .3f)
                            WorldGen.PlaceTile(start.X + i, start.Y + j, TileID.SnowBrick);//, default, i>20&&i<w-20);
                    }

                    //Old Comment I HOPE
                    //For some reason this is basically blank, check NoiseGen class probably
                    //But for real, not sure what the problem is.
                }
            }
            WorldUtils.Gen(start, new Shapes.Rectangle(w, h), new Actions.SetFrames(true));
        }

        public override void PreUpdateEntities()
        {
            for(int i = 0; i < MaxPlatforms; i++)
            {
                Platform[i]?.update();
            }
            Main.spriteBatch.Begin();
            for (int i = 0; i < MaxPlatforms; i++)
            {
                Platform[i]?.Draw();
            }
            Main.spriteBatch.End();
        }
        public override void OnWorldLoad()
        {
            for (int i = 0; i < MaxPlatforms; i++)
            {
                if (Platform[i]!=null && Platform[i].active)
                    Platform[i]?.Kill();
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            for(int i = 0; i < MaxPlatforms; i++)
            {
                EntityPlatform plat = Platform[i];
                writer.Write(plat.whoAmI);
                writer.Write(plat.active);
            }
        }
    }
}