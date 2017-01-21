using Jinx.Common;

namespace Jinx.Modes
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Color = SharpDX.Color;

    internal class ModeConfig
    {
        public static Orbwalking.Orbwalker Orbwalker;
        public static Menu MenuConfig { get; private set; }
        public static Menu MenuKeys { get; private set; }
        public static Menu MenuHarass { get; private set; }
        public static Menu MenuFarm { get; private set; }
        public static Menu MenuFlee { get; private set; }
        public static Menu MenuMisc { get; private set; }
        public static Menu MenuTools { get; private set; } 
        public static Menu Menumessgage { get; private set; } 
        public static void Init()
        {
            MenuConfig = new Menu("老王定制-17年新金克丝", "CjShu Jinx Back", true).SetFontStyle(FontStyle.Regular, Color.Pink);

            MenuTools = new Menu("杂项", "Tools");
            MenuConfig.AddSubMenu(MenuTools);

            MenuTools.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(MenuTools.SubMenu("Orbwalking"));
            Orbwalker.SetAttack(true);

            Modes.ModeSettings.Init(MenuConfig);
            {
                Common.CommonAutoLevel.Init(MenuTools);
                Common.CommonAutoBush.Init(MenuTools);
                Common.CommonSkins.Init(MenuTools);
            }

            // EvadeMain.Init();
            CommonHelper.Init();

            MenuKeys = new Menu("Keys", "Keys").SetFontStyle(FontStyle.Bold, Color.Coral);
            {
                MenuKeys.AddItem(new MenuItem("Key.Combo", "Combo!").SetValue(new KeyBind(MenuConfig.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press))).SetFontStyle(FontStyle.Regular, Color.GreenYellow);
                MenuKeys.AddItem(new MenuItem("Key.Farm", "Farm").SetValue(new KeyBind(MenuConfig.Item("LaneClear").GetValue<KeyBind>().Key, KeyBindType.Press))).SetFontStyle(FontStyle.Regular, Color.DarkKhaki);
                
                MenuConfig.AddSubMenu(MenuKeys);
            }

            ModeCombo.Init();

            MenuFarm = new Menu("Farm", "Farm");
            {
                Modes.ModeLane.Init(MenuFarm);
                Modes.ModeJungle.Init(MenuFarm);

                MenuFarm.AddItem(new MenuItem("Farm.Enable", ":: Lane / Jungle Clear Active!").SetValue(new KeyBind("J".ToCharArray()[0], KeyBindType.Toggle, true))).Permashow(true, ObjectManager.Player.ChampionName + " | " + "Lane/Jungle Farm", Colors.ColorPermaShow);
                MenuFarm.AddItem(new MenuItem("Farm.MinMana.Enable", "Min. Mana Control Active!").SetValue(new KeyBind("M".ToCharArray()[0], KeyBindType.Toggle, true)).SetFontStyle(FontStyle.Regular, Color.Aqua)).Permashow(true, ObjectManager.Player.ChampionName + " | " + "Min. Mana Control Active", Colors.ColorPermaShow);

                MenuConfig.AddSubMenu(MenuFarm);
            }

            Menumessgage = new Menu("脚本 信息", "messgage");
            Menumessgage.AddItem(new MenuItem("Sprite", "辉煌游戏"));
            Menumessgage.AddItem(new MenuItem("Version", "版本 : V 8.8.8.8"));
            Menumessgage.AddItem(new MenuItem("Hanhua", "老王汉化"));
            Menumessgage.AddItem(new MenuItem("wearethebest", "老王定制Q-228124423"));
            Menumessgage.AddItem(new MenuItem("duiwaiqqqun", "老王内部群:348902882"));

            
            new ModeDraw().Init();
            ModePerma.Init();
            MenuConfig.AddToMainMenu();
            
            foreach (var i in MenuConfig.Children.Cast<Menu>().SelectMany(GetSubMenu))
            {
                i.DisplayName = ":: " + i.DisplayName;
            }
        }

        private static IEnumerable<Menu> GetSubMenu(Menu menu)
        {
            yield return menu;

            foreach (var childChild in menu.Children.SelectMany(GetSubMenu))
                yield return childChild;
        }

    }
}