using System;
using System.Collections;
using DiskCardGame;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;
using UnityEngine;

namespace ProtossMod.Scripts.Abilities
{
    public class BuildInterceptorAbility : ACustomCreateCardsAdjacent<BuildInterceptorAbility, CreateCardsAbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}
        
		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return !this.Card.Dead && this.Card.OpponentCard != playerUpkeep;
		}

		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			bool toLeftValid = toLeft != null && toLeft.Card == null;
			bool toRightValid = toRight != null && toRight.Card == null;
			yield return base.PreSuccessfulTriggerSequence();
			if (toLeftValid)
			{
				yield return new WaitForSeconds(0.1f);
				yield return this.SpawnCardOnSlot(toLeft);
			}
			if (toRightValid)
			{
				yield return new WaitForSeconds(0.1f);
				yield return this.SpawnCardOnSlot(toRight);
			}
			if (toLeftValid || toRightValid)
			{
				yield return base.LearnAbility(0f);
			}
		}
	}
}