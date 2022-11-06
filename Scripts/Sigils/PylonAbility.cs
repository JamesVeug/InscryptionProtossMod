using System;
using System.Collections;
using DiskCardGame;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;

namespace ProtossMod.Scripts.Abilities
{
    public class PylonAbility : ACustomAbilityBehaviour<PylonAbility, AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep;
		}

		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return Singleton<ResourcesManager>.Instance.AddEnergy(1);
			yield return base.LearnAbility();
		}
	}
}