﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;
using ItemData = LeagueSharp.Common.Data.ItemData;
using SharpDX;


namespace HoolaLucian
{
    public class Program
    {
        private static Menu Menu;
        private static Orbwalking.Orbwalker Orbwalker;
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static HpBarIndicator Indicator = new HpBarIndicator();
        private static Spell Q, Q1, W, E, R;
        private static bool rstate;
        private static bool AAPassive;
        private static bool HEXQ { get { return Menu.Item("HEXQ").GetValue<bool>(); } }
        private static bool KillstealQ { get { return Menu.Item("KillstealQ").GetValue<bool>(); } }
        private static bool CQ { get { return Menu.Item("CQ").GetValue<bool>(); } }
        private static bool CW { get { return Menu.Item("CW").GetValue<bool>(); } }
        private static int CE { get { return Menu.Item("CE").GetValue<StringList>().SelectedIndex; } }
        private static bool HQ { get { return Menu.Item("HQ").GetValue<bool>(); } }
        private static bool HW { get { return Menu.Item("HW").GetValue<bool>(); } }
        private static int HE { get { return Menu.Item("HE").GetValue<StringList>().SelectedIndex; } }
        private static int HMinMana { get { return Menu.Item("HMinMana").GetValue<Slider>().Value; } }
        private static bool JQ { get { return Menu.Item("JQ").GetValue<bool>(); } }
        private static bool JW { get { return Menu.Item("JW").GetValue<bool>(); } }
        private static bool JE { get { return Menu.Item("JE").GetValue<bool>(); } }
        private static bool LHQ { get { return Menu.Item("LHQ").GetValue<bool>(); } }
        private static bool LQ { get { return Menu.Item("LQ").GetValue<bool>(); } }
        private static bool LW { get { return Menu.Item("LW").GetValue<bool>(); } }
        private static bool LE { get { return Menu.Item("LE").GetValue<bool>(); } }
        private static int LMinMana { get { return Menu.Item("LMinMana").GetValue<Slider>().Value; } }
        private static bool Dind { get { return Menu.Item("Dind").GetValue<bool>(); } }
        private static bool DEQ { get { return Menu.Item("DEQ").GetValue<bool>(); } }
        private static bool DQ { get { return Menu.Item("DQ").GetValue<bool>(); } }
        private static bool DW { get { return Menu.Item("DW").GetValue<bool>(); } }
        private static bool DE { get { return Menu.Item("DE").GetValue<bool>(); } }
        static bool AutoQ { get { return Menu.Item("AutoQ").GetValue<KeyBind>().Active; } }
        private static int MinMana { get { return Menu.Item("MinMana").GetValue<Slider>().Value; } }
        private static int HHMinMana { get { return Menu.Item("HHMinMana").GetValue<Slider>().Value; } }
        private static int Humanizer { get { return Menu.Item("Humanizer").GetValue<Slider>().Value; } }
        static bool ForceR { get { return Menu.Item("ForceR").GetValue<KeyBind>().Active; } }
        static bool LT { get { return Menu.Item("LT").GetValue<KeyBind>().Active; } }

        static void Main()
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        static void OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Lucian") return;
            Game.PrintChat("<font color='#33FF66'><b>鑰佺帇瀹氬埗-鏈€寮疯矾瑗挎仼   娉ㄥ叆鎴愬姛</b>");
            Q = new Spell(SpellSlot.Q, 675);
            Q1 = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 1200, TargetSelector.DamageType.Magical);
            E = new Spell(SpellSlot.E, 475f);
            R = new Spell(SpellSlot.R, 1400);

            OnMenuLoad();

            Q.SetTargetted(0.25f, 1400f);
            Q1.SetSkillshot(0.5f, 65, float.MaxValue, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.30f, 80f, 1600f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.2f, 110f, 2500, true, SkillshotType.SkillshotLine);

            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnDoCast += OnDoCast;
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnDoCast += OnDoCastLC;
        }
        private static void OnMenuLoad()
        {
            Menu = new Menu("老王定制-最强卢西恩", "lucian", true);

            Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Menu.AddSubMenu(targetSelectorMenu);

            var Combo = new Menu("Combo", "Combo");
            Combo.AddItem(new MenuItem("CQ", "Use Q").SetValue(true));
            Combo.AddItem(new MenuItem("CW", "Use W").SetValue(true));
            Combo.AddItem(new MenuItem("CE", "Use E Mode").SetValue(new StringList(new[] { "Side", "Cursor", "Enemy", "Never" })));
            Combo.AddItem(new MenuItem("ForceR", "Force R On Target Selector").SetValue(new KeyBind('T', KeyBindType.Press)));
            Menu.AddSubMenu(Combo);

            var Misc = new Menu("Misc", "Misc");
            Misc.AddItem(new MenuItem("Humanizer", "Humanizer Delay").SetValue(new Slider(300,5,300)));
            Misc.AddItem(new MenuItem("Nocolision", "Nocolision W").SetValue(true));
            Menu.AddSubMenu(Misc);


            var Harass = new Menu("Harass", "Harass");
            Harass.AddItem(new MenuItem("HEXQ", "Use Extended Q").SetValue(true));
            Harass.AddItem(new MenuItem("HMinMana", "Extended Q Min Mana (%)").SetValue(new Slider(80)));
            Harass.AddItem(new MenuItem("HQ", "Use Q").SetValue(true));
            Harass.AddItem(new MenuItem("HW", "Use W").SetValue(true));
            Harass.AddItem(new MenuItem("HE", "Use E Mode").SetValue(new StringList(new [] {"Side","Cursor","Enemy","Never"})));
            Harass.AddItem(new MenuItem("HHMinMana", "Harass Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(Harass);

            var LC = new Menu("LaneClear", "LaneClear");
            LC.AddItem(new MenuItem("LT", "Use Spell LaneClear (Toggle)").SetValue(new KeyBind('J', KeyBindType.Toggle)));
            LC.AddItem(new MenuItem("LHQ", "Use Extended Q For Harass").SetValue(true));
            LC.AddItem(new MenuItem("LQ", "Use Q").SetValue(true));
            LC.AddItem(new MenuItem("LW", "Use W").SetValue(true));
            LC.AddItem(new MenuItem("LE", "Use E").SetValue(true));
            LC.AddItem(new MenuItem("LMinMana", "Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(LC);

            var JC = new Menu("JungleClear", "JungleClear");
            JC.AddItem(new MenuItem("JQ", "Use Q").SetValue(true));
            JC.AddItem(new MenuItem("JW", "Use W").SetValue(true));
            JC.AddItem(new MenuItem("JE", "Use E").SetValue(true));
            Menu.AddSubMenu(JC);

            var Auto = new Menu("Auto", "Auto");
            Auto.AddItem(new MenuItem("AutoQ", "Auto Extended Q (Toggle)").SetValue(new KeyBind('G', KeyBindType.Toggle)));
            Auto.AddItem(new MenuItem("MinMana", "Min Mana (%)").SetValue(new Slider(80)));
            Menu.AddSubMenu(Auto);

            var Draw = new Menu("Draw", "Draw");
            Draw.AddItem(new MenuItem("Dind", "Draw Damage Incidator").SetValue(true));
            Draw.AddItem(new MenuItem("DEQ", "Draw Extended Q").SetValue(true));
            Draw.AddItem(new MenuItem("DQ", "Draw Q").SetValue(true));
            Draw.AddItem(new MenuItem("DW", "Draw W").SetValue(true));
            Draw.AddItem(new MenuItem("DE", "Draw E").SetValue(true));
            Menu.AddSubMenu(Draw);

            var killsteal = new Menu("killsteal", "Killsteal");
            killsteal.AddItem(new MenuItem("KillstealQ", "Killsteal Q").SetValue(true));
            Menu.AddSubMenu(killsteal);

            Menu.AddToMainMenu();
        }

        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;

            if (args.Target is Obj_AI_Hero)
            {
                var target = (Obj_AI_Base)args.Target;
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target.IsValid)
                {
                    Utility.DelayAction.Add(Humanizer, () => OnDoCastDelayed(args));
                }
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed && target.IsValid)
                {
                    Utility.DelayAction.Add(Humanizer, () => OnDoCastDelayed(args));
                }
            }
            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && args.Target.IsValid)
                {
                    Utility.DelayAction.Add(Humanizer, () => OnDoCastDelayed(args));
                }
            }
        }
        private static void OnDoCastLC(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;

            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && args.Target.IsValid)
                {
                    Utility.DelayAction.Add(Humanizer, () => OnDoCastDelayedLC(args));
                }
            }
        }

        static void killsteal()
        {
            if (KillstealQ && Q.IsReady())
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValidTarget(Q.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Q.GetDamage2(target) && (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") && !target.HasBuff("JudicatorIntervention")))
                        Q.Cast(target);
                }
            }
        }
        private static void OnDoCastDelayedLC(GameObjectProcessSpellCastEventArgs args)
        {
            AAPassive = false;
            if (args.Target is Obj_AI_Minion && args.Target.IsValid)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && Player.ManaPercent > LMinMana)
                {
                    var Minions = MinionManager.GetMinions(Orbwalking.GetRealAutoAttackRange(Player), MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.Health);
                    if (Minions[0].IsValid && Minions.Count != 0)
                    {
                        if (!LT) return;

                        if (E.IsReady() && !AAPassive && LE) E.Cast(Player.Position.Extend(Game.CursorPos, 70));
                        if (Q.IsReady() && (!E.IsReady() || (E.IsReady() && !LE)) && LQ && !AAPassive) Q.Cast(Minions[0]);
                        if ((!E.IsReady() || (E.IsReady() && !LE)) && (!Q.IsReady() || (Q.IsReady() && !LQ)) && LW && W.IsReady() && !AAPassive) W.Cast(Minions[0].Position);
                    }
                }
            }
        }
        public static Vector2 Deviation(Vector2 point1, Vector2 point2, double angle)
        {
            angle *= Math.PI / 180.0;
            Vector2 temp = Vector2.Subtract(point2, point1);
            Vector2 result = new Vector2(0);
            result.X = (float)(temp.X * Math.Cos(angle) - temp.Y * Math.Sin(angle)) / 9;
            result.Y = (float)(temp.X * Math.Sin(angle) + temp.Y * Math.Cos(angle)) / 9;
            result = Vector2.Add(result, point1);
            return result;
        }
        private static void OnDoCastDelayed(GameObjectProcessSpellCastEventArgs args)
        {
            AAPassive = false;
            if (args.Target is Obj_AI_Hero)
            {
                var target = (Obj_AI_Base)args.Target;
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target.IsValid)
                {
                    if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
                    if (E.IsReady() && !AAPassive && CE == 0) E.Cast((Deviation(Player.Position.To2D(), target.Position.To2D(), 65).To3D()));
                    if (E.IsReady() && !AAPassive && CE == 1) E.Cast(Game.CursorPos);
                    if (E.IsReady() && !AAPassive && CE == 2) E.Cast(Player.Position.Extend(target.Position, 50));
                    if (Q.IsReady() && (!E.IsReady() || (E.IsReady() && CE == 3)) && CQ && !AAPassive) Q.Cast(target);
                    if ((!E.IsReady() || (E.IsReady() && CE == 3)) && (!Q.IsReady() || (Q.IsReady() && !CQ)) && CW && W.IsReady() && !AAPassive) W.Cast(target.Position);
                }
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed && target.IsValid)
                {
                    if (Player.ManaPercent < HHMinMana) return;

                    if (E.IsReady() && !AAPassive && HE == 0) E.Cast((Deviation(Player.Position.To2D(), target.Position.To2D(),65).To3D()));
                    if (E.IsReady() && !AAPassive && HE == 1) E.Cast(Player.Position.Extend(Game.CursorPos, 50));
                    if (E.IsReady() && !AAPassive && HE == 2) E.Cast(Player.Position.Extend(target.Position, 50));
                    if (Q.IsReady() && (!E.IsReady() || (E.IsReady() && HE == 3)) && HQ && !AAPassive) Q.Cast(target);
                    if ((!E.IsReady() || (E.IsReady() && HE == 3)) && (!Q.IsReady() || (Q.IsReady() && !HQ)) && HW && W.IsReady() && !AAPassive) W.Cast(target.Position);
                }
            }
            if (args.Target is Obj_AI_Minion && args.Target.IsValid)
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var Mobs = MinionManager.GetMinions(Orbwalking.GetRealAutoAttackRange(Player), MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);
                    if (Mobs[0].IsValid && Mobs.Count != 0)
                    {
                        if (E.IsReady() && !AAPassive && JE) E.Cast(Player.Position.Extend(Game.CursorPos, 70));
                        if (Q.IsReady() && (!E.IsReady() || (E.IsReady() && !JE)) && JQ && !AAPassive) Q.Cast(Mobs[0]);
                        if ((!E.IsReady() || (E.IsReady() && !JE)) && (!Q.IsReady() || (Q.IsReady() && !JQ)) && JW && W.IsReady() && !AAPassive) W.Cast(Mobs[0].Position);
                    }
                }
            }
        }

        static void Harass()
        {
            if (Player.ManaPercent < HMinMana) return;

            if (Q.IsReady() && HEXQ)
            {
                var t1 = TargetSelector.GetTarget(Q1.Range, TargetSelector.DamageType.Physical);
                if (t1.IsValidTarget(Q1.Range) && Player.Distance(t1.ServerPosition) > Q.Range + 100)
                {
                    var qpred = Q.GetPrediction(t1, true);
                    var distance = Player.Distance(qpred.CastPosition);
                    var minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                    foreach (var minion in minions.Where(minion => minion.IsValidTarget(Q.Range)))
                    {
                        if (qpred.CastPosition.Distance(Player.Position.Extend(minion.Position, distance)) < 25)
                        {
                            Q.Cast(minion);
                            return;
                        }
                    }
                }
            }
        }
        static void LaneClear()
        {
            if (Player.ManaPercent < LMinMana)

            if (Q.IsReady() && LHQ)
            {
                var t1 = TargetSelector.GetTarget(Q1.Range, TargetSelector.DamageType.Physical);
                if (t1.IsValidTarget(Q1.Range) && Player.Distance(t1.ServerPosition) > Q.Range + 100)
                {
                    var qpred = Q.GetPrediction(t1, true);
                    var distance = Player.Distance(qpred.CastPosition);
                    var minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                    foreach (var minion in minions.Where(minion => minion.IsValidTarget(Q.Range)))
                    {
                        if (qpred.CastPosition.Distance(Player.Position.Extend(minion.Position, distance)) < 25)
                        {
                            Q.Cast(minion);
                            return;
                        }
                    }
                }
            }
        }
        static void AutoUseQ()
        {
            if (Q.IsReady() && AutoQ && Player.ManaPercent > MinMana)
            {
                var t1 = TargetSelector.GetTarget(Q1.Range, TargetSelector.DamageType.Physical);
                if (t1.IsValidTarget(Q1.Range) && Player.Distance(t1.ServerPosition) > Q.Range + 100)
                {
                    var qpred = Q.GetPrediction(t1, true);
                    var distance = Player.Distance(qpred.CastPosition);
                    var minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                    foreach (var minion in minions.Where(minion => minion.IsValidTarget(Q.Range)))
                    {
                        if (qpred.CastPosition.Distance(Player.Position.Extend(minion.Position, distance)) < 25)
                        {
                            Q.Cast(minion);
                            return;
                        }
                    }
                }
            }
        }

        static void UseRTarget()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
            if (ForceR && !rstate && R.IsReady() && target.IsValid && target is Obj_AI_Hero) R.Cast(target.Position);
        }
        static void Game_OnUpdate(EventArgs args)
        {
            if (!R.IsReady(10000) && rstate) rstate = false;
            W.Collision = Menu.Item("Nocolision").GetValue<bool>();
            AutoUseQ();

            if (ForceR) UseRTarget();
            killsteal();
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed) Harass();
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) LaneClear();
        }
        static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E)
            {
                AAPassive = true;
            }
            if (args.Slot == SpellSlot.E)
            {
                Orbwalking.ResetAutoAttackTimer();
            }
            if (args.Slot == SpellSlot.R)
            {
                rstate = true;
                ItemData.Youmuus_Ghostblade.GetItem().Cast();
            }
        }

        static float getComboDamage2(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                if (E.IsReady()) damage = damage + (float)Player.GetAutoAttackDamage2(enemy) * 2;
                if (W.IsReady()) damage = damage + W.GetDamage2(enemy) + (float)Player.GetAutoAttackDamage2(enemy);
                if (Q.IsReady())
                {
                    damage = damage + Q.GetDamage2(enemy) + (float)Player.GetAutoAttackDamage2(enemy);
                }
                damage = damage + (float)Player.GetAutoAttackDamage2(enemy);

                return damage;
            }
            return 0;
        }

        static void OnDraw(EventArgs args)
        {
            if (DEQ) Render.Circle.DrawCircle(Player.Position, Q1.Range, Q.IsReady() ? Color.LimeGreen : Color.IndianRed);
            if (DQ) Render.Circle.DrawCircle(Player.Position, Q.Range, Q.IsReady() ? Color.LimeGreen : Color.IndianRed);
            if (DW) Render.Circle.DrawCircle(Player.Position, W.Range, W.IsReady() ? Color.LimeGreen : Color.IndianRed);
            if (DE) Render.Circle.DrawCircle(Player.Position, E.Range, E.IsReady() ? Color.LimeGreen : Color.IndianRed);
        }
        static void Drawing_OnEndScene(EventArgs args)
        {
            if (Dind)
            {
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(ene => ene.IsValidTarget() && !ene.IsZombie))
                {
                    Indicator.unit = enemy;
                    Indicator.drawDmg(getComboDamage2(enemy), new ColorBGRA(255, 204, 0, 160));

                }
            }
        }
    }
}