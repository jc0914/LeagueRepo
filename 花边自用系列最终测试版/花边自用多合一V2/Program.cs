namespace 花边自用多合一
{
    using System;
    using LeagueSharp.Common;
    using myUtility;

    internal class Program
    {
        private static void Main(string[] Args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs Args)
        {
            myAllInOne.Init();
        }
    }
}
