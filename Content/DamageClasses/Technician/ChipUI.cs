﻿using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using ReLogic.Content;
using ReLogic.Graphics;

namespace Onyxia.Content.DamageClasses.Technician
{
    /*
    public class ChipUIState : UIState {
        ChipUIItem[] chips;
        public ChipUIState(TechItem item) {
            chips = new ChipUIItem[item.ModSlots];
            for(int i = 0; i < item.ModSlots; i++) {
                if(item.GetModule(i, out Chip c)) {
                    chips[i] = new ChipUIItem(c); } } } }
    public class ChipUIItem : UIElement {
        public Chip ChipItem {
            get;
            private set; }
        public ChipUIItem(Chip c) {
            ChipItem = c; } }
    */
    public class ChipUIState : UIState
    {
        TechItem tItem;
        ChipUIContainer cont;
        
        public ChipUIState()
        {
            cont = new ChipUIContainer();
            tItem = new TechItem_Default();
        }
        public ChipUIState(TechItem item)
        {
            tItem = item;
            cont = new ChipUIContainer(item);
            Append(cont);
        }
        public override void MouseOver(UIMouseEvent evt)
        {
            Main.LocalPlayer.mouseInterface = true;
        }
    }
    public class ChipUIItem : UIElement
    {
        TechItem baseItem;
        int index;
        Item item;
        //Chip module;
        Asset<Texture2D> itemTex;
        Rectangle rect;
        public ChipUIItem()
        {
            item = new Item();
            //module = new Chip_Default(); //Blank item, basically
            //module.Item.TurnToAir();
        }
        public ChipUIItem(TechItem tItem, int index, ref Item i)
        {
            baseItem = tItem;
            this.index = index;
            item = i;
            //module = c;
        }
        public Chip Module
        {
            get
            {
                return item.ModItem as Chip;
            }
        }
        public override bool ContainsPoint(Vector2 point)
        {
            return rect.Contains(point.ToPoint());
        }

        bool justSwapped = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool mouseHover = ContainsPoint(Main.MouseScreen);
            rect = new Rectangle((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);
            if(mouseHover)
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if(Main.mouseLeft)
            {
                if(!justSwapped && mouseHover)
                {
                    if((Main.mouseItem.IsAir ^ item.IsAir) ||
                        (Main.mouseItem.ModItem?.GetType().IsAssignableTo(typeof(Chip))) == true)
                    {
                        /*
                        if (Main.mouseItem.IsAir)
                        {
                            Main.NewText("Taking Item");
                            Main.mouseItem = item.Clone();
                            item.TurnToAir();
                            itemTex = null;
                        }
                        else if(item.IsAir)
                        {
                            Main.NewText("Placing Item");
                            //Utils.Swap(ref item, ref Main.mouseItem);
                            item = Main.mouseItem.Clone();
                            Main.mouseItem.TurnToAir();
                            itemTex = null;
                        }
                        else
                        {
                            Main.NewText("Swapping Item");
                            Item inter = item.Clone();
                            item = Main.mouseItem.Clone();
                            item = inter.Clone();
                            itemTex = null;
                        }*/

                        if (baseItem.SwapModule(index, ref Main.mouseItem))
                        {
                            item = baseItem.GetChip(index);
                            itemTex = null;
                        }

                        /*if (!otherItem.IsAir)
                        {
                            module.Entity.SetDefaults(otherItem.type);
                            module.Entity.stack = otherItem.stack;
                        }
                        else
                            module.Entity.TurnToAir();*/
                        justSwapped = true;
                    }
                }
            }
            else
            {
                justSwapped = false;
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            //Draw Tooltip '~'
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            bool mouseHover = ContainsPoint(Main.MouseScreen);
            if(mouseHover && !item.IsAir)
            {
                Main.HoverItem = item;
                Main.hoverItemName = item.Name;
                Main.showItemText = true;
            }

            Rectangle drawRect = new Rectangle((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

            drawRect.Location += new Point(3, 3);
            drawRect.Width -= 6;
            drawRect.Height -= 6;
            spriteBatch.Draw(TextureAssets.InventoryBack.Value, drawRect, Color.White);

            if (item == null || item.IsAir) return;

            //drawRect.Location += new Point(3, 3);
            //drawRect.Width -= 6;
            //drawRect.Height -= 6;
            if (itemTex == null)
                itemTex = ModContent.Request<Texture2D>(Module.Texture);
            else
            {
                Vector2 pos = drawRect.Center.ToVector2();
                Vector2 diff = itemTex.Size();
                spriteBatch.Draw(itemTex.Value, pos - (diff*.5f), Color.White);
            }
        }
    }
    public class ChipUIContainer : UIElement
    {
        int size = 52;
        internal ChipUIContainer()
        {
            rect = new Rectangle(Main.screenWidth / 2, Main.screenHeight / 3, size, size);
        }
        public ChipUIContainer(TechItem tItem)
        {
            int lng = tItem.ModSlots;
            this.HAlign = .5f;
            this.VAlign = .334f;
            this.Width.Set(lng*size, 0);
            this.Height.Set(size*1.5f, 0);
            rect = new Rectangle((int)(HAlign * Main.screenWidth), (int)(VAlign * Main.screenHeight), (int)Width.Pixels, (int)Height.Pixels);
            //float vAdj = (.5f * (float)size / Main.screenHeight);
            for (int i = 0; i < lng; i++)
            {
                //Chip c = tItem.GetChip(i);
                //Main.NewText(c.Item.Name);
                ChipUIItem chip = new ChipUIItem(tItem, i, ref tItem.GetChip(i));
                //chip.HAlign = HAlign + ((float)i * (float)size / (float)Main.screenWidth);
                //chip.VAlign = VAlign + vAdj;
                chip.Width.Set(size, 0);
                chip.Height.Set(size, 0);
                Append(chip);
            }
            UpdateChildPositions();
        }

        public override bool ContainsPoint(Vector2 point)
        {
            foreach(UIElement ui in Elements)
            {
                if (ui.ContainsPoint(point)) return false;
            }
            return rect.Contains(point.ToPoint());
        }

        bool drag;
        Vector2 prevPt;
        Rectangle rect;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            void AdjustPos(Vector2 mPos, bool snap)
            {
                drag = snap;
                if (prevPt.Equals(Vector2.Zero) || prevPt.HasNaNs())
                    prevPt = mPos;
                rect.Location += (mPos - prevPt).ToPoint();
                prevPt = snap ? mPos : Vector2.Zero;
                this.Left.Set(rect.X, 0);
                VAlign = (float)rect.X / Main.screenWidth;
                this.Top.Set(rect.Y, 0);
                HAlign = (float)rect.Y / Main.screenHeight;
                UpdateChildPositions();
            }

            Vector2 mouseScreen = Main.MouseScreen;

            //AdjustPos(mouseScreen, Main.mouseLeft ? (ContainsPoint(mouseScreen) || drag) : !drag);
            if(Main.mouseLeft)
            {
                if (ContainsPoint(mouseScreen) || drag)
                    AdjustPos(mouseScreen, true);
            }
            else
            {
                if (drag)
                    AdjustPos(mouseScreen, false);
            }

            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;
        }
        private void UpdateChildPositions()
        {
            float vAdj = (.5f * (float)size / Main.screenHeight);
            for (int i = 0; i < Elements.Count; i++)
            {
                //UIElement chip = Elements[i];
                Elements[i].Top.Set(rect.Y + (int)(size * .5f), 0);
                Elements[i].Left.Set(rect.X + (int)(i * size), 0);
                //Elements[i].HAlign = HAlign + ((float)i * (float)size / (float)Main.screenWidth);
                //Elements[i].VAlign = VAlign + vAdj;
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //Rectangle drawRect = rect;// new Rectangle((int)(this.HAlign * Main.screenWidth), (int)(this.VAlign * Main.screenHeight), (int)Width.Pixels, (int)Height.Pixels);
            spriteBatch.Draw(TextureAssets.ChatBack.Value, rect, Color.White*.7f);
            string title = "Tinkering Menu";
            DynamicSpriteFont dSpFn = FontAssets.MouseText.Value;
            Vector2 offset = dSpFn.MeasureString(title);
            float xMult = MathHelper.Clamp((float)rect.Width / offset.X, 0f, 1f);// * offset.X;
            offset.X *= xMult;
            Vector2 position = new Vector2(rect.Center.X - (offset.X * .5f), rect.Top + 4f);
            //Main.NewText(title + " : " + offset.ToPoint() + " : " + xMult + " : " + position.ToPoint());
            spriteBatch.DrawString(dSpFn, title, position, Main.OurFavoriteColor * .95f, 0, Vector2.Zero, new Vector2(xMult, 1), SpriteEffects.None, 0f);
        }
    }
}
