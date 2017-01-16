using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace MoonDraven
{
    internal class MoonDraven
    {
        public Spell E;
        public Spell Q;
        public List<QRecticle> QReticles = new List<QRecticle>();
        public Spell R;
        public Spell W;
        public Orbwalking.Orbwalker Orbwalker { get; set; }
        public Menu Menu { get; set; }

        public Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        public int QCount
        {
            get
            {
                return (Player.HasBuff("dravenspinningattack")
                    ? Player.Buffs.First(x => x.Name == "dravenspinningattack").Count
                    : 0) + QReticles.Count;
            }
        }

        // Jodus pls
        public float ManaPercent
        {
            get { return Player.Mana/Player.MaxMana*100; }
        }

        public void Load()
        {
            // Create spells
            Q = new Spell(SpellSlot.Q, Orbwalking.GetRealAutoAttackRange(Player));
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 1100);
            R = new Spell(SpellSlot.R);

            E.SetSkillshot(0.25f, 130, 1400, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.4f, 160, 2000, true, SkillshotType.SkillshotLine);

            CreateMenu();

            Game.PrintChat("<font color='#FF99CC'><b>鑰佺帇瀹氬埗浜旀寰疯悐鏂囦辅 娉ㄥ叆鎴愬姛</b></font><font color='#FF3300'><b>鑰佺帇VIP鑵虫湰浜ゆ祦缇や辅348902882</b></font>");
            Game.PrintChat("Also try <font color='#66FF33'><b>鑰佺帇瀹氬埗</b></font> for a gamebreaking experience!");

            GameObject.OnCreate += GameObjectOnOnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloserOnOnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += Interrupter2OnOnInterruptableTarget;
            Drawing.OnDraw += DrawingOnOnDraw;
            Game.OnUpdate += GameOnOnUpdate;
        }

        private void DrawingOnOnDraw(EventArgs args)
        {
            var drawE = Menu.Item("DrawE").IsActive();
            var drawAxeLocation = Menu.Item("DrawAxeLocation").IsActive();
            var drawAxeRange = Menu.Item("DrawAxeRange").IsActive();

            if (drawE)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, E.IsReady() ? Color.Aqua : Color.Red);
            }

            if (drawAxeLocation)
            {
                var bestAxe =
                    QReticles
                        .Where(
                            x =>
                                x.Position.Distance(Game.CursorPos) <
                                Menu.Item("CatchAxeRange").GetValue<Slider>().Value)
                        .OrderBy(x => x.Position.Distance(Player.ServerPosition))
                        .ThenBy(x => x.Position.Distance(Game.CursorPos))
                        .FirstOrDefault();

                if (bestAxe != null)
                {
                    Render.Circle.DrawCircle(bestAxe.Position, 120, Color.LimeGreen);
                }

                foreach (
                    var axe in
                        QReticles.Where(x => x.Object.NetworkId != (bestAxe == null ? 0 : bestAxe.Object.NetworkId)))
                {
                    Render.Circle.DrawCircle(axe.Position, 120, Color.Yellow);
                }
            }

            if (drawAxeRange)
            {
                Render.Circle.DrawCircle(Game.CursorPos, Menu.Item("CatchAxeRange").GetValue<Slider>().Value,
                    Color.DodgerBlue);
            }
        }

        private void Interrupter2OnOnInterruptableTarget(Obj_AI_Hero sender,
            Interrupter2.InterruptableTargetEventArgs args)
        {
            if (!Menu.Item("UseEInterrupt").IsActive() || !E.IsReady() || !sender.IsValidTarget(E.Range))
            {
                return;
            }

            if (args.DangerLevel == Interrupter2.DangerLevel.Medium || args.DangerLevel == Interrupter2.DangerLevel.High)
            {
                E.Cast(sender);
            }
        }

        private void AntiGapcloserOnOnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("UseEGapcloser").IsActive() || !E.IsReady() || !gapcloser.Sender.IsValidTarget(E.Range))
            {
                return;
            }

            E.Cast(gapcloser.Sender);
        }

        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }

            QReticles.RemoveAll(x => x.Object.NetworkId == sender.NetworkId);
        }

        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }

            QReticles.Add(new QRecticle(sender, Environment.TickCount + 1800));
            Utility.DelayAction.Add(1800, () => QReticles.RemoveAll(x => x.Object.NetworkId == sender.NetworkId));
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            var catchOption = Menu.Item("AxeMode").GetValue<StringList>().SelectedIndex;

            if ((catchOption == 0 && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (catchOption == 1 && Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) || catchOption == 2)
            {
                var bestReticle =
                    QReticles
                        .Where(
                            x =>
                                x.Object.Position.Distance(Game.CursorPos) <
                                Menu.Item("CatchAxeRange").GetValue<Slider>().Value)
                        .OrderBy(x => x.Position.Distance(Player.ServerPosition))
                        .ThenBy(x => x.Position.Distance(Game.CursorPos))
                        .FirstOrDefault();

                if (bestReticle != null && bestReticle.Object.Position.Distance(Player.ServerPosition) > 110)
                {
                    var eta = 1000*(Player.Distance(bestReticle.Position)/Player.MoveSpeed);
                    var expireTime = bestReticle.ExpireTime - Environment.TickCount;

                    if (eta >= expireTime && Menu.Item("UseWForQ").IsActive())
                    {
                        W.Cast();
                    }
                    
                    if (Menu.Item("DontCatchUnderTurret").IsActive())
                    {
                        // If we're under the turret as well as the axe, catch the axe
                        if (Player.UnderTurret(true) && bestReticle.Object.Position.UnderTurret(true))
                        {
                            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                            }
                            else
                            {
                                Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                            }
                        }
                        // Catch axe if not under turret
                        else if (!bestReticle.Position.UnderTurret(true))
                        {
                            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                            }
                            else
                            {
                                Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                            }      
                        }
                    }
                    else
                    {
                        if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                        }
                        else
                        {
                            Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                        } 
                    }            
                }
                else
                {
                    Orbwalker.SetOrbwalkingPoint(Game.CursorPos);
                }
            }
            else
            {
                Orbwalker.SetOrbwalkingPoint(Game.CursorPos);
            }

            if (W.IsReady() && Menu.Item("UseWSlow").IsActive() && Player.HasBuffOfType(BuffType.Slow))
            {
                W.Cast();
            }

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
            }

            if (Menu.Item("UseHarassToggle").IsActive())
            {
                Harass();
            }
        }

        private void Combo()
        {
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            if (!target.IsValidTarget())
            {
                return;
            }

            var useQ = Menu.Item("UseQCombo").IsActive();
            var useW = Menu.Item("UseWCombo").IsActive();
            var useE = Menu.Item("UseECombo").IsActive();
            var useR = Menu.Item("UseRCombo").IsActive();

            if (useQ && QCount < Menu.Item("MaxAxes").GetValue<Slider>().Value - 1 && Q.IsReady() &&
                Orbwalker.InAutoAttackRange(target) &&
                !Player.Spellbook.IsAutoAttacking)
            {
                Q.Cast();
            }

            if (useW && W.IsReady() && ManaPercent > Menu.Item("UseWManaPercent").GetValue<Slider>().Value)
            {
                if (Menu.Item("UseWSetting").IsActive())
                {
                    W.Cast();
                }
                else
                {
                    if (!Player.HasBuff("dravenfurybuff"))
                    {
                        W.Cast();
                    }
                }
            }

            if (useE && E.IsReady())
            {
                E.Cast(target);
            }

            if (!useR || !R.IsReady())
            {
                return;
            }

            // Patented Advanced Algorithms D321987
            var killableTarget =
                HeroManager.Enemies.Where(x => x.IsValidTarget(2000))
                    .FirstOrDefault(
                        x =>
                            Player.GetSpellDamage(x, SpellSlot.R)*2 > x.Health &&
                            (!Orbwalker.InAutoAttackRange(x) || Player.CountEnemiesInRange(E.Range) > 2));

            if (killableTarget != null)
            {
                R.Cast(killableTarget);
            }
        }

        private void LaneClear()
        {
            var useQ = Menu.Item("UseQWaveClear").IsActive();
            var useW = Menu.Item("UseWWaveClear").IsActive();
            var useE = Menu.Item("UseEWaveClear").IsActive();

            if (ManaPercent < Menu.Item("WaveClearManaPercent").GetValue<Slider>().Value)
            {
                return;
            }

            if (useQ && QCount < Menu.Item("MaxAxes").GetValue<Slider>().Value - 1 && Q.IsReady() &&
                Orbwalker.GetTarget() is Obj_AI_Minion &&
                !Player.Spellbook.IsAutoAttacking)
            {
                Q.Cast();
            }

            if (useW && W.IsReady() && ManaPercent > Menu.Item("UseWManaPercent").GetValue<Slider>().Value)
            {
                if (Menu.Item("UseWSetting").IsActive())
                {
                    W.Cast();
                }
                else
                {
                    if (!Player.HasBuff("dravenfurybuff"))
                    {
                        W.Cast();
                    }
                }
            }

            if (!useE || !E.IsReady())
            {
                return;
            }

            var bestLocation = E.GetLineFarmLocation(MinionManager.GetMinions(E.Range));

            if (bestLocation.MinionsHit > 1)
            {
                E.Cast(bestLocation.Position);
            }
        }

        private void Harass()
        {
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            if (!target.IsValidTarget())
            {
                return;
            }

            if (Menu.Item("UseEHarass").IsActive() && E.IsReady())
            {
                E.Cast(target);
            }
        }

        private void CreateMenu()
        {
            Menu = new Menu("大魔王老王定制系列暴走德莱文", "cmMoonDraven", true);

            // Target Selector
            var tsMenu = new Menu("目标选择", "ts");
            TargetSelector.AddToMenu(tsMenu);
            Menu.AddSubMenu(tsMenu);

            // Orbwalker
            var orbwalkMenu = new Menu("走砍", "orbwalker");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkMenu);
            Menu.AddSubMenu(orbwalkMenu);

            // Combo
            var comboMenu = new Menu("连招", "combo");
            comboMenu.AddItem(new MenuItem("UseQCombo", "使用Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseWCombo", "使用W").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseECombo", "使用E").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseRCombo", "使用R").SetValue(true));
            Menu.AddSubMenu(comboMenu);

            // Harass
            var harassMenu = new Menu("骚扰", "harass");
            harassMenu.AddItem(new MenuItem("UseEHarass", "使用E").SetValue(true));
            harassMenu.AddItem(
                new MenuItem("UseHarassToggle", "骚扰(切换)").SetValue(new KeyBind(84, KeyBindType.Toggle)));
            Menu.AddSubMenu(harassMenu);

            // Lane Clear
            var laneClearMenu = new Menu("清线", "waveclear");
            laneClearMenu.AddItem(new MenuItem("UseQWaveClear", "使用Q").SetValue(true));
            laneClearMenu.AddItem(new MenuItem("UseWWaveClear", "使用W").SetValue(true));
            laneClearMenu.AddItem(new MenuItem("UseEWaveClear", "使用E").SetValue(false));
            laneClearMenu.AddItem(new MenuItem("WaveClearManaPercent", "蓝量保护").SetValue(new Slider(50)));
            Menu.AddSubMenu(laneClearMenu);

            // Axe Menu
            var axeMenu = new Menu("斧子设置", "axeSetting");
            axeMenu.AddItem(
                new MenuItem("AxeMode", "接斧子模式:").SetValue(new StringList(new[] {"Combo", "Any", "Always"},
                    2)));
            axeMenu.AddItem(new MenuItem("CatchAxeRange", "接斧子半径").SetValue(new Slider(800, 120, 1500)));
            axeMenu.AddItem(new MenuItem("MaxAxes", "最大区域").SetValue(new Slider(2, 1, 3)));
            axeMenu.AddItem(new MenuItem("UseWForQ", "如果斧子太远用W").SetValue(true));
            axeMenu.AddItem(new MenuItem("DontCatchUnderTurret", "不在塔下接斧头").SetValue(true));
            Menu.AddSubMenu(axeMenu);

            // Drawing
            var drawMenu = new Menu("标示", "draw");
            drawMenu.AddItem(new MenuItem("DrawE", "E范围").SetValue(true));
            drawMenu.AddItem(new MenuItem("DrawAxeLocation", "标示斧子位置").SetValue(true));
            drawMenu.AddItem(new MenuItem("DrawAxeRange", "标识斧子捕捉范围").SetValue(true));
            Menu.AddSubMenu(drawMenu);

            // Misc Menu
            var miscMenu = new Menu("杂项", "misc");
            miscMenu.AddItem(new MenuItem("UseWSetting", "立即启用W(可用时)").SetValue(false));
            miscMenu.AddItem(new MenuItem("UseEGapcloser", "自动E接近者").SetValue(true));
            miscMenu.AddItem(new MenuItem("UseEInterrupt", "使用E打断").SetValue(true));
            miscMenu.AddItem(new MenuItem("UseWManaPercent", "W使用蓝量保护").SetValue(new Slider(50)));
            miscMenu.AddItem(new MenuItem("UseWSlow", "如果减速使用W").SetValue(true));
            Menu.AddSubMenu(miscMenu);

            Menu.AddToMainMenu();
        }

        internal class QRecticle
        {
            public QRecticle(GameObject rectice, int expireTime)
            {
                Object = rectice;
                ExpireTime = expireTime;
            }

            public GameObject Object { get; set; }
            public int ExpireTime { get; set; }

            public Vector3 Position
            {
                get { return Object.Position; }
            }
        }
    }
}