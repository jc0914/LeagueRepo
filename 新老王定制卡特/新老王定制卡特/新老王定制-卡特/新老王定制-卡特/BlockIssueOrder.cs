﻿using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace e.Motion_Katarina
{
    public static class BlockIssueOrder
    {
        private static int whenToCancelR = 0;
        public static void InitializeBlockIssueOrder()
        {
            Obj_AI_Base.OnIssueOrder += OnIssueOrder;
            Obj_AI_Base.OnBuffAdd += OnBuffGain;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;

        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && Logic.Player.HasBuff("katarinarsound"))
            {
                //Accidental Cancel
                if (Utils.TickCount <= whenToCancelR && Config.GetBoolValue("misc.noRCancel"))
                {
                    args.Process = false;
                }
                if(args.Slot != SpellSlot.Q && args.Slot != SpellSlot.W && args.Slot != SpellSlot.E)
                {
                    args.Process = false;
                }
            }
        }


        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            if(sender.IsMe && args.Buff.Name == "katarinarsound")
            {
                whenToCancelR = Utils.TickCount + 400;
            }
        }

        private static void OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            //Accidental Cancel
            if (sender.IsMe && Logic.Player.HasBuff("katarinarsound") && Utils.TickCount <= whenToCancelR && Config.GetBoolValue("misc.noRCancel"))
            {
                args.Process = false;
            }            
        }
    }
}
