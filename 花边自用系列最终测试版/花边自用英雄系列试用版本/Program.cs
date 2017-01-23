namespace 花边自用英雄系列试用版本
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using myChampionsAIO_Test;

    internal class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                if (!string.IsNullOrEmpty(ObjectManager.Player.ChampionName))
                {
                    myChampionInject.作者QQ("1076751236");
                }
            };
        }
    }
}
