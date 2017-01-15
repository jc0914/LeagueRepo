﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace TrinketExploit
{
    class Program
    {
        public static Obj_AI_Hero myHero;
        public static Menu myMenu;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
            Game.OnUpdate += Update;
        }

        private static void OnLoad(EventArgs args)
        {
            myHero = ObjectManager.Player;
            Menu();
        }

        private static void Menu()
        {
            myMenu = new Menu("1级买蓝眼/扫描", "TrinketExploit", true);
            myMenu.AddItem(new MenuItem("buyBlue", "买蓝眼").SetValue(false)).ValueChanged += (s, ar) =>
            {
                if (ar.GetNewValue<bool>())
                {
                    Game.PrintChat("CHENG GONG !");
                    myHero.BuyItem(ItemId.Warding_Totem_Trinket);
                    myHero.BuyItem(ItemId.Farsight_Orb_Trinket);
                }
            };
            myMenu.AddItem(new MenuItem("buyRed", "买扫描").SetValue(false)).ValueChanged += (s, ar) =>
            {
                if (ar.GetNewValue<bool>())
                {
                    Game.PrintChat("CHENG GONG !");
                    myHero.BuyItem(ItemId.Sweeping_Lens_Trinket);
                    myHero.BuyItem(ItemId.Oracles_Lens_Trinket);
                }
            };
            myMenu.AddToMainMenu();

        }

        private static void Update(EventArgs args)
        {
            myMenu.Item("buyBlue").SetValue(false);
            myMenu.Item("buyRed").SetValue(false);
        }
    }
}
