﻿using System;
using System.Drawing;
using System.Linq;
using LeagueSharp.Common;
using LeagueSharp;
using Color = System.Drawing.Color;

namespace Jhin___The_Virtuoso
{
    static class Program
    {

        public static readonly Obj_AI_Hero Jhin = ObjectManager.Player;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q, W, E, R;
        public static Menu Config;

        static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Jhin")
            {
                return;
            }


            Q = new Spell(SpellSlot.Q, 550);
            W = new Spell(SpellSlot.W, 2500);
            E = new Spell(SpellSlot.E, 2000);
            R = new Spell(SpellSlot.R, 3500);


            W.SetSkillshot(0.75f, 40, float.MaxValue, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.23f, 120, 1600, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.21f, 80, 5000, false, SkillshotType.SkillshotLine);

            VersionCheck.UpdateCheck();

            Config = new Menu(":: 戏命师-烬", ":: Jhin - The Virtuoso", true);
            {
                Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu(":: Orbwalker Settings"));
                var comboMenu = new Menu(":: Combo Settings", ":: Combo Settings");
                {
                    var qComboMenu = new Menu(":: Q", ":: Q");
                    {
                        qComboMenu.AddItem(new MenuItem("q.combo", "Use (Q)").SetValue(true));
                        comboMenu.AddSubMenu(qComboMenu);
                    }

                    var wComboMenu = new Menu(":: W", ":: W");
                    {
                        wComboMenu.AddItem(new MenuItem("w.combo", "Use (W)").SetValue(true));
                        wComboMenu.AddItem(new MenuItem("w.combo.min.distance", "Min. Distance").SetValue(new Slider(400, 1, 2500)));
                        wComboMenu.AddItem(new MenuItem("w.combo.max.distance", "Max. Distance").SetValue(new Slider(1000, 1, 2500)));
                        wComboMenu.AddItem(new MenuItem("w.passive.combo", "Use (W) If Enemy Is Marked").SetValue(true));
                        wComboMenu.AddItem(new MenuItem("w.hit.chance", "(W) Hit Chance").SetValue(new StringList(Helper.HitchanceNameArray, 2)));
                        comboMenu.AddSubMenu(wComboMenu);
                    }

                    var eComboMenu = new Menu(":: E", ":: E");
                    {
                        eComboMenu.AddItem(new MenuItem("e.combo", "Use (E)").SetValue(true));
                        eComboMenu.AddItem(new MenuItem("e.combo.teleport", "Auto (E) Teleport").SetValue(true));
                        eComboMenu.AddItem(new MenuItem("e.hit.chance", "(E) Hit Chance").SetValue(new StringList(Helper.HitchanceNameArray, 2)));
                        comboMenu.AddSubMenu(eComboMenu);
                    }

                    Config.AddSubMenu(comboMenu);
                }
                var harassMenu = new Menu(":: Harass Settings", ":: Harass Settings");
                {
                    var wHarassMenu = new Menu(":: W", ":: W");
                    {
                        wHarassMenu.AddItem(new MenuItem("w.harass", "Use (W)").SetValue(true));
                        harassMenu.AddSubMenu(wHarassMenu);
                    }
                    harassMenu.AddItem(new MenuItem("harass.mana", "Min. Mana Percentage").SetValue(new Slider(50, 1, 99)));
                    Config.AddSubMenu(harassMenu);
                }
                var clearMenu = new Menu(":: Clear Settings", ":: Clear Settings");
                {
                    var laneclearMenu = new Menu(":: Wave Clear", ":: Wave Clear");
                    {
                        laneclearMenu.AddItem(new MenuItem("keysinfo1", "                  (Q) Settings").SetTooltip("Q Settings"));
                        laneclearMenu.AddItem(new MenuItem("q.clear", "Use (Q)").SetValue(true));
                        laneclearMenu.AddItem(new MenuItem("keysinfo2", "                  (W) Settings").SetTooltip("W Settings"));
                        laneclearMenu.AddItem(new MenuItem("w.clear", "Use (W)").SetValue(true));
                        laneclearMenu.AddItem(new MenuItem("w.hit.x.minion", "Min. Minion").SetValue(new Slider(4, 1, 5)));
                        clearMenu.AddSubMenu(laneclearMenu);
                    }

                    var jungleClear = new Menu(":: Jungle Clear", ":: Jungle Clear");
                    {
                        jungleClear.AddItem(new MenuItem("keysinfo1X", "                  (Q) Settings").SetTooltip("Q Settings"));
                        jungleClear.AddItem(new MenuItem("q.jungle", "Use (Q)").SetValue(true));
                        jungleClear.AddItem(new MenuItem("keysinfo2X", "                  (W) Settings").SetTooltip("W Settings"));
                        jungleClear.AddItem(new MenuItem("w.jungle", "Use (W)").SetValue(true));
                        clearMenu.AddSubMenu(jungleClear);
                    }
                    clearMenu.AddItem(new MenuItem("clear.mana", "LaneClear Min. Mana Percentage").SetValue(new Slider(50, 1, 99)));
                    clearMenu.AddItem(new MenuItem("jungle.mana", "Jungle Min. Mana Percentage").SetValue(new Slider(50, 1, 99)));
                    Config.AddSubMenu(clearMenu);
                }

                var ksMenu = new Menu(":: Kill Steal", ":: Kill Steal");
                {
                    ksMenu.AddItem(new MenuItem("q.ks", "Use (Q)").SetValue(true));
                    ksMenu.AddItem(new MenuItem("w.ks", "Use (W)").SetValue(true));
                    ksMenu.AddItem(new MenuItem("r.ks", "Use (R)").SetValue(true));
                    Config.AddSubMenu(ksMenu);
                }

                var miscMenu = new Menu(":: Miscellaneous", ":: Miscellaneous");
                {
                    miscMenu.AddItem(new MenuItem("auto.e.immobile", "Auto Cast (E) Immobile Target").SetValue(true));
                    //miscMenu.AddItem(new MenuItem("ezevade.hijacker", "ezEvade Hijacker").SetValue(true)).SetTooltip("When Jhin using (R) Disabling ezEvade for max. damage ");
                    //miscMenu.AddItem(new MenuItem("evadesharp.hijacker", "Evade# Hijacker").SetValue(true)).SetTooltip("When Jhin using (R) Disabling Evade# for max. damage ");
                    Config.AddSubMenu(miscMenu);
                }
                var rComboMenu = new Menu(":: Ultimate Settings", ":: Ultimate Settings").SetFontStyle(FontStyle.Bold, SharpDX.Color.Yellow);
                {
                    var rComboWhiteMenu = new Menu(":: R - Whitelist", ":: R - Whitelist");
                    {
                        foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValid))
                        {
                            rComboWhiteMenu.AddItem(new MenuItem("r.combo." + enemy.ChampionName, "(R): " + enemy.ChampionName).SetValue(true));
                        }
                        rComboMenu.AddSubMenu(rComboWhiteMenu);
                    }
                    rComboMenu.AddItem(new MenuItem("r.combo", "Use (R)").SetValue(true));
                    rComboMenu.AddItem(new MenuItem("auto.shoot.bullets", "If Jhin Casting (R) Auto Cast Bullets").SetValue(true));
                    rComboMenu.AddItem(new MenuItem("r.hit.chance", "(R) Hit Chance").SetValue(new StringList(Helper.HitchanceNameArray, 1)));
                    Config.AddSubMenu(rComboMenu);
                }

                var drawMenu = new Menu(":: Drawings", ":: Drawings");
                {
                    var damageDraw = new Menu(":: Damage Draw", ":: Damage Draw");
                    {
                        damageDraw.AddItem(new MenuItem("aa.indicator", "(AA) Indicator").SetValue(new Circle(true, Color.Gold)));
                        drawMenu.AddSubMenu(damageDraw);
                    }
                    drawMenu.AddItem(new MenuItem("q.draw", "(Q) Range").SetValue(new Circle(false, Color.White)));
                    drawMenu.AddItem(new MenuItem("w.draw", "(W) Range").SetValue(new Circle(false, Color.Gold)));
                    drawMenu.AddItem(new MenuItem("e.draw", "(E) Range").SetValue(new Circle(false, Color.DodgerBlue)));
                    drawMenu.AddItem(new MenuItem("r.draw", "(R) Range").SetValue(new Circle(false, Color.GreenYellow)));
                    Config.AddSubMenu(drawMenu);
                }
                Config.AddItem(new MenuItem("semi.manual.ult", "Semi-Manual (R)!").SetValue(new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));
                Config.AddItem(new MenuItem("use.combo", "Combo (Active)").SetValue(new KeyBind(32, KeyBindType.Press)));
                Config.AddItem(new MenuItem("credits.x1", "                          Developed by Hikigaya").SetFontStyle(FontStyle.Bold, SharpDX.Color.DodgerBlue));
                Config.AddItem(new MenuItem("credits.x2", "       Dont forget to Upvote on Assembly Database").SetFontStyle(FontStyle.Bold, SharpDX.Color.YellowGreen));
                var drawDamageMenu = new MenuItem("RushDrawEDamage", "Combo Damage").SetValue(true);
                var drawFill = new MenuItem("RushDrawEDamageFill", "Combo Damage Fill").SetValue(new Circle(true, Color.Gold));

                drawMenu.SubMenu("Damage Draws").AddItem(drawDamageMenu);
                drawMenu.SubMenu("Damage Draws").AddItem(drawFill);

                DamageIndicator.DamageToUnit = Helper.ComboDamage;
                DamageIndicator.Enabled = drawDamageMenu.GetValue<bool>();
                DamageIndicator.Fill = drawFill.GetValue<Circle>().Active;
                DamageIndicator.FillColor = drawFill.GetValue<Circle>().Color;

                drawDamageMenu.ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    DamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                };

                drawFill.ValueChanged +=
                delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    DamageIndicator.Fill = eventArgs.GetNewValue<Circle>().Active;
                    DamageIndicator.FillColor = eventArgs.GetNewValue<Circle>().Color;
                };
                Config.AddToMainMenu();
            }
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Item("q.draw").GetValue<Circle>().Active && Q.IsReady())
            {
                Render.Circle.DrawCircle(Jhin.Position, Q.Range, Config.Item("q.draw").GetValue<Circle>().Color);
            }
            if (Config.Item("w.draw").GetValue<Circle>().Active && W.IsReady())
            {
                Render.Circle.DrawCircle(Jhin.Position, W.Range, Config.Item("w.draw").GetValue<Circle>().Color);
            }
            if (Config.Item("e.draw").GetValue<Circle>().Active && E.IsReady())
            {
                Render.Circle.DrawCircle(Jhin.Position, E.Range, Config.Item("e.draw").GetValue<Circle>().Color);
            }
            if (Config.Item("r.draw").GetValue<Circle>().Active && R.IsReady())
            {
                Render.Circle.DrawCircle(Jhin.Position, R.Range, Config.Item("r.draw").GetValue<Circle>().Color);
            }
            if (Config.Item("aa.indicator").GetValue<Circle>().Active)
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(1500) && x.IsValid && x.IsVisible && !x.IsDead && !x.IsZombie))
                {
                    Drawing.DrawText(enemy.HPBarPosition.X, enemy.HPBarPosition.Y, Config.Item("aa.indicator").GetValue<Circle>().Color, string.Format("{0} Basic Attack = Kill", Helper.AaIndicator(enemy)));
                }
            }

        } // done working
        private static void OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    JungleClear();
                    LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.None:
                    ImmobileE();
                    /* KillSteal();*/
                    break;
            }
            if (Config.Item("semi.manual.ult").GetValue<KeyBind>().Active)
            {
                SemiManual();
            }
            if (!Config.Item("semi.manual.ult").GetValue<KeyBind>().Active && Config.Item("auto.shoot.bullets").GetValue<bool>())
            {
                AutoShoot();
            }

            if (!Helper.IsRActive)
            {
                Orbwalker.SetAttack(true);
                Orbwalker.SetMovement(true);
            }
            else
            {
                Orbwalker.SetAttack(false);
                Orbwalker.SetMovement(false);
            }
        }
        private static void AutoShoot()
        {
            if (Helper.IsRActive)
            {
                var blocked = HeroManager.Enemies.Where(x => !Config.Item("r.combo." + x.ChampionName).GetValue<bool>());
                var tstarget = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical, false, blocked);
                if (tstarget != null)
                {
                    var pred = R.GetPrediction(tstarget);
                    if (pred.Hitchance >= Helper.HikiChance("r.hit.chance"))
                    {
                        R.Cast(pred.CastPosition);
                        return;
                    }
                }
            }
        }

        private static void SemiManual()
        {
            if (R.IsReady())
            {
                var blocked = HeroManager.Enemies.Where(x => !Config.Item("r.combo." + x.ChampionName).GetValue<bool>());
                var tstarget = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical, false, blocked);
                if (tstarget != null)
                {
                    if (!Helper.IsRActive)
                    {
                        R.CastOnUnit(ObjectManager.Player);
                    }
                    else
                    {
                        AutoShoot();
                    }
                }
            }
        }

        private static void Combo()
        {
            if (Q.IsReady() && Config.Item("q.combo").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Q.Range) && ObjectManager.Player.HasBuff("JhinPassiveReload")))
                {
                    Q.CastOnUnit(enemy);
                }
            }
            if (W.IsReady() && Config.Item("w.combo").GetValue<bool>())
            {
                if (Config.Item("w.passive.combo").GetValue<bool>())
                {
                    foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValid && x.Distance(Jhin) < Config.Item("w.combo.max.distance").GetValue<Slider>().Value
                    && x.Distance(Jhin) > Config.Item("w.combo.min.distance").GetValue<Slider>().Value && W.GetPrediction(x).Hitchance >= Helper.HikiChance("w.hit.chance")
                    && x.HasBuff("jhinespotteddebuff")))
                    {
                        W.Cast(enemy);
                    }
                }
                else
                {
                    foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValid && x.Distance(Jhin) < Config.Item("w.combo.max.distance").GetValue<Slider>().Value
                    && x.Distance(Jhin) > Config.Item("w.combo.min.distance").GetValue<Slider>().Value && W.GetPrediction(x).Hitchance >= Helper.HikiChance("w.hit.chance")))
                    {
                        W.Cast(enemy);
                    }
                }

            }
            if (E.IsReady() && Config.Item("e.combo").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && E.GetPrediction(x).Hitchance >= Helper.HikiChance("e.hit.chance")))
                {
                    if (Helper.IsEnemyImmobile(enemy))
                    {
                        E.Cast(enemy.Position);
                    }
                }
            }
            if (E.IsReady() && Config.Item("e.combo").GetValue<bool>() && Config.Item("e.combo.teleport").GetValue<bool>())
            {
                foreach (var obj in ObjectManager.Get<Obj_AI_Base>().Where(x => x.Team != Jhin.Team && x.Distance(Jhin) < E.Range
                    && x.HasBuff("teleport_target")))
                {
                    E.Cast(obj.Position);
                }
            }

            if (R.IsReady() && Config.Item("r.combo").GetValue<bool>() && !Helper.IsRActive)
            {
                var enemy = HeroManager.Enemies.Find(x => x.IsValidTarget(R.Range) && Config.Item("r.combo." + x.ChampionName).GetValue<bool>() && x.Health <= R.GetDamage(x));
                if (enemy != null)
                {
                    R.CastOnUnit(ObjectManager.Player);
                }
            }
        }
    

        private static void Harass()
        {
            if (Jhin.ManaPercent < Config.Item("harass.mana").GetValue<Slider>().Value)
            {
                return;
            }
            if (W.IsReady() && Config.Item("w.harass").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(W.Range) && W.GetPrediction(x).Hitchance >= Helper.HikiChance("w.hit.chance")))
                {
                    W.Cast(enemy);
                }
            }
        }
        private static void LaneClear()
        {
            if (Jhin.ManaPercent < Config.Item("clear.mana").GetValue<Slider>().Value)
            {
                return;
            }
            if (Q.IsReady() && Config.Item("q.clear").GetValue<bool>()) // done working
            {
                var min = MinionManager.GetMinions(Jhin.ServerPosition, Q.Range).MinOrDefault(x => x.Health);
                Q.CastOnUnit(min);
            }
            if (W.IsReady() && Config.Item("w.clear").GetValue<bool>()) // done working
            {
                var min = MinionManager.GetMinions(Jhin.ServerPosition, W.Range);
                if (W.GetLineFarmLocation(min).MinionsHit >= Config.Item("w.hit.x.minion").GetValue<Slider>().Value)
                {
                    W.Cast(W.GetLineFarmLocation(min).Position);
                }
            }
        }
        private static void JungleClear()
        {
            if (Jhin.ManaPercent < Config.Item("jungle.mana").GetValue<Slider>().Value)
            {
                return;
            }
            var mobs = MinionManager.GetMinions(Jhin.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
            if (mobs == null || (mobs.Count == 0))
            {
                return;
            }
            if (Q.IsReady() && Config.Item("q.clear").GetValue<bool>())
            {
                Q.Cast(mobs[0]);
            }
            if (W.IsReady() && Config.Item("w.clear").GetValue<bool>())
            {
                W.Cast(mobs[0]);
            }
        }
        private static void ImmobileE()
        {
            if (E.IsReady() && Config.Item("auto.e.immobile").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && Helper.IsEnemyImmobile(x)))
                {
                    E.Cast(enemy);
                }
            }
        }
        private static void KillSteal()
        {
            if (Q.IsReady() && Config.Item("q.ks").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Q.Range) &&
                    x.Health < Q.GetDamage(x)))
                {
                    Q.CastOnUnit(enemy);
                }
            }
            if (W.IsReady() && Config.Item("w.ks").GetValue<bool>())
            {
                foreach (var enemy in HeroManager.Enemies.Where(x => x.Distance(Jhin) < Config.Item("w.combo.max.distance").GetValue<Slider>().Value
                        && x.Distance(Jhin) > Config.Item("w.combo.min.distance").GetValue<Slider>().Value
                        && x.IsValid && W.GetPrediction(x).Hitchance >= Helper.HikiChance("w.hit.chance")
                        && x.Health < W.GetDamage(x)))
                {
                    W.Cast(enemy);
                }
            }

            if (R.IsReady() && Config.Item("r.ks").GetValue<bool>() && Helper.IsRActive) {
                var killable = HeroManager.Enemies.Find(x => Config.Item("r.combo." + x.ChampionName).GetValue<bool>() && x.IsValidTarget(R.Range) && x.Health < R.GetDamage(x));
                if (killable != null)
                {
                    var pred = R.GetPrediction(killable);
                    if (pred.Hitchance >= Helper.HikiChance("r.hit.chance"))
                    {
                        R.Cast(pred.CastPosition);
                        return;
                    }
                }
            }
        }
    }
}