using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;
using UnityEngine;

namespace ProtossMod.Scripts.Abilities
{
    public class SoulAbsorptionAbility : ACustomAbilityBehaviour<SoulAbsorptionAbility, AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			return BoardHasEnoughCardsToRevive();
		}

		private bool BoardHasEnoughCardsToRevive()
		{
			int currentHealth = this.Card.Health;
			if (currentHealth > 0)
			{
				return false;
			}

			List<CardSlot> targetCard = GetSortedTargetSlots();
			if (targetCard.Count == 0)
			{
				return false;
			}

			// Get all health of cards on the board
			int totalHealth = targetCard.Sum((a) => a.Card.Health);

			// Make sure their health is higher than the damage taken
			return totalHealth > -currentHealth;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			yield return PreSuccessfulTriggerSequence();

			// Keep going until we revive
			while (BoardHasEnoughCardsToRevive())
			{
				// Get the next weakest card
				List<CardSlot> allSlots = GetSortedTargetSlots();
				PlayableCard targetCard = allSlots[0].Card;
				
				// Show card is about to take damage
				targetCard.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.3f);

				int currentHealth = this.Card.Health;
				int requiredHealth = Mathf.Min(targetCard.Health, currentHealth + 1); // +1 to make the card have more than 1 health

				// Heal this card
				this.Card.HealDamage(requiredHealth);
				yield return new WaitForSeconds(0.1f);
				
				// Team the card card to take damage
				yield return targetCard.TakeDamage(requiredHealth, Card);
				yield return LearnAbility(0f);
			}
		}

		private List<CardSlot> GetSortedTargetSlots()
		{
			List<CardSlot> allSlots = Singleton<BoardManager>.Instance.playerSlots.FindAll(IsValidSacrifice);
			
			// Sort by weakest cards
			allSlots.Sort((a, b) =>
			{
				int aStats = a.Card.Attack + (a.Card.Health + a.Card.Status.damageTaken);
				int bStats = b.Card.Attack + (b.Card.Health + b.Card.Status.damageTaken);
				return aStats - bStats;
			});
			
			return allSlots;
		}

		private bool IsValidSacrifice(CardSlot a)
		{
			if (a.Card == null) 
				return false;

			if (a.Card == this.Card)
				return false;

			// Do not take health from terrain
			if (a.Card.Info.traits.Contains(Trait.Terrain))
				return false;
			
			return true;
		}
	}
}