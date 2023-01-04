using System;
using System.Collections;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;

namespace ProtossMod.Scripts.Abilities
{
    public class PrismaticAlignmentAbility : ACustomAbilityBehaviour<PrismaticAlignmentAbility, AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;

		private PlayableCard lastAttackedCard;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return lastAttackedCard != null && Card.OpponentCard == !playerTurnEnd;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			CardModificationInfo info = Card.TemporaryMods.Find((a) => a.singletonId == "PrismaticAlignment");
			if (info != null)
			{
				Card.TemporaryMods.Remove(info);
			}

			lastAttackedCard = null;
			yield return null;
		}

		public override bool RespondsToDealDamage(int amount, PlayableCard target)
		{
			return target != null && !target.Dead;
		}

		public override IEnumerator OnDealDamage(int amount, PlayableCard target)
		{
			yield return base.OnDealDamage(amount, target);

			CardModificationInfo info = Card.TemporaryMods.Find((a) => a.singletonId == "PrismaticAlignment");
			if (info == null)
			{
				info = new CardModificationInfo();
				info.attackAdjustment = 1;
				Card.AddTemporaryMod(info);
			}
			else
			{
				info.attackAdjustment += 1;
			}

			lastAttackedCard = target;
		}
	}
}