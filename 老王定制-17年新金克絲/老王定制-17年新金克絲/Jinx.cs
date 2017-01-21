namespace Jinx
{
    #region

    using LeagueSharp;
    using LeagueSharp.Common;
    using System;

    #endregion

    internal class Jinx
    {
        public static string ChampionName => "Jinx";
        public static void Init()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharData.BaseSkinName != ChampionName)
            {
                return;
            }

            Champion.PlayerSpells.Init();
            Modes.ModeConfig.Init();
            Common.CommonItems.Init();

            Game.PrintChat("<font color='#99FFCC'><font size='30'><b>鑰佺帇瀹氬埗-17骞存柊鐗堥噾鍏嬩笣</b></font><font color='#FF6633'><b>鑰佺帇VIP鑵虫湰浜ゆ祦缇や辅348902882</b></font>");
            Game.PrintChat("Also try <font color='#66FF33'><b>鑰佺帇瀹氬埗</b></font> for a gamebreaking experience!");
            Game.PrintChat("<font color='#FF99CC'><b> 鏂板勾蹇▊!</b></font>!");


            Console.Clear();
        }
    }
}