namespace 花边自用英雄系列
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using myChampionsAIO;

    internal class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                if (!string.IsNullOrEmpty(ObjectManager.Player.ChampionName))
                {
                    myChampionInject.LoadUserID("1076751236");
                }
            };
        }
    }
}
