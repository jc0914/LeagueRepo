namespace LeagueSharp.Common
{
    /// <summary>
    ///     The LeagueSharp.Common official menu.
    /// </summary>
    internal class CommonMenu
    {
        #region Static Fields

        /// <summary>
        ///     The menu instance.
        /// </summary>
        internal static Menu Instance = new Menu("LeagueSharp.Common", "LeagueSharp.Common", true);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="CommonMenu" /> class.
        /// </summary>
        static CommonMenu()
        {
            Initialize();
        }

        public static void Initialize()
        {
			//修改的内容：增加老王定制菜单。
			老王定制Menu.Initialize();
			TargetSelector.Initialize();
            Prediction.Initialize();
            Hacks.Initialize();
            FakeClicks.Initialize();

            Instance.AddToMainMenu();
        }

        public static void Shutdown()
        {
			//修改的内容：增加老王定制菜单。
			老王定制Menu.Shutdown();
			TargetSelector.Shutdown();
            Prediction.Shutdown();
            Hacks.Shutdown();
            FakeClicks.Shutdown();

            Menu.Remove(Instance);
        }

        #endregion
    }
}