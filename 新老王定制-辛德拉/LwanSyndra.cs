namespace LwanSyndra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;
    using Common = SebbyLib.OktwCommon;
    using Cache = SebbyLib.Cache;

    class Syndra
    {
        private Menu Config = Program.Config;
        public static Orbwalking.Orbwalker Orbwalker = Program.Orbwalker;
        public Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private Spell E, Q, R, W, EQ, Eany;
        private float QMANA = 0, WMANA = 0, EMANA = 0, RMANA = 0;
        private static List<Obj_AI_Minion> BallsList = new List<Obj_AI_Minion>();
        private bool EQcastNow = false;
        public void Load()
        {
            Q = new Spell(SpellSlot.Q, 790);
            W = new Spell(SpellSlot.W, 950);
            E = new Spell(SpellSlot.E, 700);
            EQ = new Spell(SpellSlot.Q, Q.Range + 400);
            Eany = new Spell(SpellSlot.Q, Q.Range + 400);
            R = new Spell(SpellSlot.R, 675);

            Q.SetSkillshot(0.6f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 140f, 1600f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, 100, 2500f, false, SkillshotType.SkillshotLine);
            EQ.SetSkillshot(0.6f, 100f, 2500f, false, SkillshotType.SkillshotLine);
            Eany.SetSkillshot(0.30f, 50f, 2500f, false, SkillshotType.SkillshotLine);

            Config.SubMenu("Q技能设置").AddItem(new MenuItem("autoQ", "自动Q", true).SetValue(true));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("harrasQ", "骚扰Q", true).SetValue(true));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("QHarassMana", "骚扰最低蓝量", true).SetValue(new Slider(30, 100, 0)));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("farmQ", "清线Q", true).SetValue(true));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("farmQout", "Q不在攻击范围内的且为残血的小兵", true).SetValue(true));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("ManaQ", "清线蓝量", true).SetValue(new Slider(50, 100, 0)));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("LCminionsQ", "清线小兵个数", true).SetValue(new Slider(3, 10, 0)));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("jungleQ", "清野Q", true).SetValue(true));
            Config.SubMenu("Q技能设置").AddItem(new MenuItem("ManaQJC", "清野最低蓝量", true).SetValue(new Slider(20, 100, 0)));

            Config.SubMenu("W技能设置").AddItem(new MenuItem("autoW", "自动W", true).SetValue(true));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("harrasW", "骚扰W", true).SetValue(true));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("farmW", "清线W", true).SetValue(true));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("ManaW", "清线最新蓝量", true).SetValue(new Slider(50, 100, 0)));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("LCminionsW", "清线小兵个数", true).SetValue(new Slider(3, 10, 0)));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("jungleW", "清野W", true).SetValue(true));
            Config.SubMenu("W技能设置").AddItem(new MenuItem("ManaWJC", "清野最低蓝量", true).SetValue(new Slider(60, 100, 0)));

            Config.SubMenu("E技能设置").AddItem(new MenuItem("autoE", "自动E", true).SetValue(true));
            Config.SubMenu("E技能设置").AddItem(new MenuItem("gapcloserE", "反突进E", true).SetValue(true));
            Config.SubMenu("E技能设置").AddItem(new MenuItem("gapcloserEtarget", "反突进 E 目标", true));
            foreach (var enemy in HeroManager.Enemies)
                Config.SubMenu("E技能设置").AddItem(new MenuItem("Egapcloser" + enemy.ChampionName, enemy.ChampionName, true).SetValue(true));

            Config.SubMenu("QE技能设置").AddItem(new MenuItem("autoE", "自动QE连招", true).SetValue(true));
            Config.SubMenu("QE技能设置").AddItem(new MenuItem("harrasE", "骚扰QE", true).SetValue(false));
            Config.SubMenu("QE技能设置").AddItem(new MenuItem("EInterrupter", "自动QE打断技能", true).SetValue(true));
            Config.SubMenu("QE技能设置").AddItem(new MenuItem("useQE", "智能QE按键", true).SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press))); //32 == space
            Config.SubMenu("QE技能设置").AddItem(new MenuItem("QEtarget", "QE目标", true));
            foreach (var enemy in HeroManager.Enemies)
                Config.SubMenu("QE技能设置").AddItem(new MenuItem("Eon" + enemy.ChampionName, enemy.ChampionName, true).SetValue(true));

            Config.SubMenu("R技能设置").AddItem(new MenuItem("autoR", "自动R击杀", true).SetValue(true));
            Config.SubMenu("R技能设置").AddItem(new MenuItem("Rcombo", "计算连招+R的伤害先手R", true).SetValue(true));
            foreach (var enemy in HeroManager.Enemies)
                Config.SubMenu("R技能设置").AddItem(new MenuItem("Rmode" + enemy.ChampionName, enemy.ChampionName, true).SetValue(new StringList(new[] { "KS ", "总是", "关闭" }, 0)));

            Config.SubMenu("自动眼位").AddItem(new MenuItem("AutoWard", "启动", true).SetValue(true));
            Config.SubMenu("自动眼位").AddItem(new MenuItem("AutoBuy", "lv9自动买灯泡", true).SetValue(true));
            Config.SubMenu("自动眼位").AddItem(new MenuItem("AutoPink", "自动真眼扫描", true).SetValue(true));
            Config.SubMenu("自动眼位").AddItem(new MenuItem("AutoWardCombo", "仅连招模式启动 ", true).SetValue(true));
            new AutoWard().Load();
            new Tracker().Load();

            var SkinMenu = Config.AddSubMenu(new Menu("换肤设置", "换肤设置"));
            {
                SkinMenu.AddItem(new MenuItem("EnableSkin", "启动换肤").SetValue(false));
                SkinMenu.AddItem(new MenuItem("SkinSelect", "选择皮肤").SetValue(new StringList(new[] { "经典", "仲裁圣女", "亚特兰蒂斯", "方块王后", "冰雪女王" })));
            }

            Config.SubMenu("显示设置").AddItem(new MenuItem("qRange", "Q 范围", true).SetValue(false));
            Config.SubMenu("显示设置").AddItem(new MenuItem("wRange", "W 范围", true).SetValue(false));
            Config.SubMenu("显示设置").AddItem(new MenuItem("eRange", "E 范围", true).SetValue(false));
            Config.SubMenu("显示设置").AddItem(new MenuItem("rRange", "R 范围", true).SetValue(false));

            Game.OnUpdate += Game_OnGameUpdate;
            GameObject.OnCreate += Obj_AI_Base_OnCreate;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
        }

        private void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.Slot == SpellSlot.Q && EQcastNow && E.IsReady())
            {
                var customeDelay = Q.Delay - (E.Delay + ((Player.Distance(args.End)) / E.Speed));
                Utility.DelayAction.Add((int)(customeDelay * 1000), () => E.Cast(args.End));
            }
        }

        private void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (E.IsReady() && Config.Item("EInterrupter", true).GetValue<bool>())
            {
                if(sender.IsValidTarget(E.Range))
                {
                    E.Cast(sender.Position);
                }
                else if (Q.IsReady() && sender.IsValidTarget(EQ.Range))
                {
                    TryBallE(sender);
                }
            }
        }

        private void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && Config.Item("Egapcloser" + gapcloser.Sender.ChampionName, true).GetValue<bool>())
            {
                if (Q.IsReady())
                {
                    EQcastNow = true;
                    Q.Cast(gapcloser.End);
                }
                else if(gapcloser.Sender.IsValidTarget(E.Range))
                {
                    E.Cast(gapcloser.Sender);
                }
            }
        }

        private void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.IsAlly && sender.Type == GameObjectType.obj_AI_Minion && sender.Name == "Seed")
            {
                var ball = sender as Obj_AI_Minion;
                BallsList.Add(ball);
            }
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!E.IsReady())
                EQcastNow = false;

            if (Program.LagFree(1))
            { 
                SetMana();
                BallCleaner();
                Jungle();
            }

            if (Program.LagFree(1) && E.IsReady() && Config.Item("autoE", true).GetValue<bool>())
                LogicE();

            if (Program.LagFree(2) && Q.IsReady() && Config.Item("autoQ", true).GetValue<bool>())
                LogicQ();

            if (Program.LagFree(3) && W.IsReady() && Config.Item("autoW", true).GetValue<bool>())
                LogicW();

            if (Program.LagFree(4) && R.IsReady() && Config.Item("autoR", true).GetValue<bool>())
                LogicR();
        }

        private void TryBallE(Obj_AI_Hero t)
        {
            if (Q.IsReady())
            {
                CastQE(t);
            }
            if(!EQcastNow)
            {
                var ePred = Eany.GetPrediction(t);
                if (ePred.Hitchance >= HitChance.VeryHigh)
                {
                    var playerToCP = Player.Distance(ePred.CastPosition);
                    foreach (var ball in BallsList.Where(ball => Player.Distance(ball.Position) < E.Range))
                    {
                        var ballFinalPos = Player.ServerPosition.Extend(ball.Position, playerToCP);
                        if (ballFinalPos.Distance(ePred.CastPosition) < 50)
                            E.Cast(ball.Position);
                    }
                }
            }
        }

        private void LogicE()
        {
            if(Config.Item("useQE", true).GetValue<KeyBind>().Active)
            {
                var mouseTarget = HeroManager.Enemies.Where(enemy => 
                    enemy.IsValidTarget(Eany.Range)).OrderBy(enemy => enemy.Distance(Game.CursorPos)).FirstOrDefault();

                if (mouseTarget != null)
                {
                    TryBallE(mouseTarget);
                    return;
                }
            }

            var t = TargetSelector.GetTarget(Eany.Range, TargetSelector.DamageType.Magical);
            if (t.IsValidTarget())
            {
                if (Common.GetKsDamage(t, E) + Q.GetDamage(t)> t.Health)
                    TryBallE(t);
                if (Program.Combo && Player.Mana > RMANA + EMANA + QMANA && Config.Item("Eon" + t.ChampionName, true).GetValue<bool>())
                    TryBallE(t);
                if (Program.Farm && Player.Mana > RMANA + EMANA + QMANA + WMANA && Config.Item("harrasE", true).GetValue<bool>())
                    TryBallE(t);
            }
        }

        private void LogicR()
        {
            R.Range = R.Level == 3 ? 750 : 675;

            bool Rcombo = Config.Item("Rcombo", true).GetValue<bool>();

            foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(R.Range) && Common.ValidUlt(enemy)))
            {
                int Rmode = Config.Item("Rmode" + enemy.ChampionName, true).GetValue<StringList>().SelectedIndex;

                if (Rmode == 2)
                    continue;
                else if (Rmode == 1)
                    R.Cast(enemy);

                var comboDMG = Common.GetKsDamage(enemy, R) ;
                comboDMG += (R.GetDamage(enemy, 1) * (R.Instance.Ammo - 3));
                comboDMG += Common.GetEchoLudenDamage(enemy);

                if (Rcombo)
                {
                    if (Q.IsReady() && enemy.IsValidTarget(600))
                        comboDMG += Q.GetDamage(enemy);

                    if (E.IsReady())
                        comboDMG += E.GetDamage(enemy);

                    if (W.IsReady())
                        comboDMG += W.GetDamage(enemy);
                }

                if (enemy.Health < comboDMG)
                {
                    R.Cast(enemy);
                }
            }
        }

        private void LogicW()
        {
            if (W.Instance.ToggleState == 1)
            {
                var t = TargetSelector.GetTarget(W.Range - 150, TargetSelector.DamageType.Magical);
                if (t.IsValidTarget())
                {
                    if (Program.Combo && Player.Mana > RMANA + QMANA + WMANA)
                        CatchW(t);
                    else if (Program.Farm && Config.Item("harrasW", true).GetValue<bool>()
                        && Player.ManaPercent > Config.Item("QHarassMana", true).GetValue<Slider>().Value)
                    {
                        CatchW(t);
                    }
                    else if (Common.GetKsDamage(t, W) > t.Health)
                        CatchW(t);
                    else if (Player.Mana > RMANA + WMANA)
                    {
                        foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(W.Range) && !Common.CanMove(enemy)))
                            CatchW(t);
                    }
                }
                else if (Program.LaneClear && !Q.IsReady() && Player.ManaPercent > Config.Item("ManaW", true).GetValue<Slider>().Value && Config.Item("farmW", true).GetValue<bool>())
                {
                    var allMinions = Cache.GetMinions(Player.ServerPosition, W.Range);
                    var farmPos = W.GetCircularFarmLocation(allMinions, W.Width);

                    if (farmPos.MinionsHit >= Config.Item("LCminionsW", true).GetValue<Slider>().Value)
                        CatchW(allMinions.FirstOrDefault());
                }
            }
            else
            {
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (t.IsValidTarget())
                {
                    Program.CastSpell(W, t);
                }
                else if (Program.LaneClear && Config.Item("farmW", true).GetValue<bool>())
                {
                    var allMinions = Cache.GetMinions(Player.ServerPosition, W.Range);
                    var farmPos = W.GetCircularFarmLocation(allMinions, W.Width);

                    if (farmPos.MinionsHit > 1)
                        W.Cast(farmPos.Position);
                }
            }
        }   

        private void LogicQ()
        {
            var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (t.IsValidTarget())
            {
                if (Program.Combo && Player.Mana > RMANA + QMANA + EMANA && !E.IsReady())
                    Program.CastSpell(Q, t);
                else if (Program.Farm && Config.Item("harrasQ", true).GetValue<bool>() && Player.ManaPercent > Config.Item("QHarassMana", true).GetValue<Slider>().Value)
                    Program.CastSpell(Q, t);
                else if (Common.GetKsDamage(t, Q) > t.Health)
                    Program.CastSpell(Q, t);
                else if (Player.Mana > RMANA + QMANA)
                {
                    foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(Q.Range) && !Common.CanMove(enemy)))
                        Program.CastSpell(Q, t);
                }
            }

            if (Player.IsWindingUp)
                return;

            if (!Program.None && !Program.Combo && Player.ManaPercent > Config.Item("ManaQ", true).GetValue<Slider>().Value)
            {
                var allMinions = Cache.GetMinions(Player.ServerPosition, Q.Range);

                if (Config.Item("farmQout", true).GetValue<bool>())
                {
                    foreach (var minion in allMinions.Where(minion => minion.IsValidTarget(Q.Range) && (!Orbwalker.InAutoAttackRange(minion) || (!minion.UnderTurret(true) && minion.UnderTurret()))))
                    {
                        var hpPred = HealthPrediction.GetHealthPrediction(minion, 600);
                        if (hpPred < Q.GetDamage(minion)  && hpPred > minion.Health - hpPred * 2)
                        {
                            Q.Cast(minion);
                            return;
                        }
                    }
                }
                if (Program.LaneClear && Config.Item("farmQ", true).GetValue<bool>())
                {
                    var farmPos = Q.GetCircularFarmLocation(allMinions, Q.Width);
                    if (farmPos.MinionsHit >= Config.Item("LCminionsQ", true).GetValue<Slider>().Value)
                        Q.Cast(farmPos.Position);
                }
            }
        }

        private void Jungle()
        {
            if (Program.LaneClear && Player.Mana > RMANA + QMANA)
            {
                var mobs = Cache.GetMinions(Player.ServerPosition, Q.Range, MinionTeam.Neutral);
                if (mobs.Count > 0)
                {
                    var mob = mobs[0];
                    if (Q.IsReady() && Config.Item("jungleQ", true).GetValue<bool>() && Player.ManaPercent > Config.Item("ManaQJC", true).GetValue<Slider>().Value)
                    {
                        Q.Cast(mob.ServerPosition);
                        return;
                    }
                    else if (W.IsReady() && Config.Item("jungleW", true).GetValue<bool>() && Player.ManaPercent > Config.Item("ManaWJC", true).GetValue<Slider>().Value && Utils.TickCount - Q.LastCastAttemptT > 900)
                    {
                        W.Cast(mob.ServerPosition);
                        return;
                    }
                }
            }
        }

        private void CastQE(Obj_AI_Base target)
        {
            SebbyLib.Prediction.SkillshotType CoreType2 = SebbyLib.Prediction.SkillshotType.SkillshotLine;

            var predInput2 = new SebbyLib.Prediction.PredictionInput
            {
                Aoe = false,
                Collision = EQ.Collision,
                Speed = EQ.Speed,
                Delay = EQ.Delay,
                Range = EQ.Range,
                From = Player.ServerPosition,
                Radius = EQ.Width,
                Unit = target,
                Type = CoreType2
            };

            var poutput2 = SebbyLib.Prediction.Prediction.GetPrediction(predInput2);

            if (Common.CollisionYasuo(Player.ServerPosition, poutput2.CastPosition))
                return;

            Vector3 castQpos = poutput2.CastPosition;

            if (Player.Distance(castQpos) > Q.Range)
                castQpos = Player.Position.Extend(castQpos, Q.Range);

            if (poutput2.Hitchance >= SebbyLib.Prediction.HitChance.VeryHigh)
            {
                EQcastNow = true;
                Q.Cast(castQpos);
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (Config.Item("qRange", true).GetValue<bool>())
            {
                if (Q.IsReady())
                    Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.Cyan, 1, 1);
            }
            if (Config.Item("wRange", true).GetValue<bool>())
            {
                if (W.IsReady())
                    Utility.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.Orange, 1, 1);
            }
            if (Config.Item("eRange", true).GetValue<bool>())
            {
                if (E.IsReady())
                    Utility.DrawCircle(ObjectManager.Player.Position, EQ.Range, System.Drawing.Color.Yellow, 1, 1);
            }
            if (Config.Item("rRange", true).GetValue<bool>())
            {
                if (R.IsReady())
                    Utility.DrawCircle(ObjectManager.Player.Position, R.Range, System.Drawing.Color.Gray, 1, 1);
            }
        }

        private void SetMana()
        {
            QMANA = Q.Instance.ManaCost;
            WMANA = W.Instance.ManaCost;
            EMANA = E.Instance.ManaCost;

            if (!R.IsReady())
                RMANA = QMANA - Player.PARRegenRate * Q.Instance.Cooldown;
            else
                RMANA = R.Instance.ManaCost;
        }

        private void BallCleaner()
        {
            if (BallsList.Count > 0)
            {
                BallsList.RemoveAll(ball => !ball.IsValid || ball.Mana == 19);
            }
        }

        private void CatchW(Obj_AI_Base t, bool onlyMinin = false)
        {

            if (Utils.TickCount - W.LastCastAttemptT < 150)
                return;

            var catchRange = 925;
            Obj_AI_Base obj = null;
            if (BallsList.Count > 0 && !onlyMinin)
            {
                obj = BallsList.Find(ball => ball.Distance(Player) < catchRange);
            }
            if (obj == null)
            {
                obj = MinionManager.GetMinions(Player.ServerPosition, catchRange, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.MaxHealth).FirstOrDefault();
            }

            if (obj != null)
            {
                foreach (var minion in MinionManager.GetMinions(Player.ServerPosition, catchRange, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.MaxHealth))
                {
                    if (t.Distance(minion) < t.Distance(obj))
                        obj = minion;
                }
                
                W.Cast(obj.Position);
            }
        }
    }
}
