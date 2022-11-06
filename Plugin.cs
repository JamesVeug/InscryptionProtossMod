using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using ProtossMod.Scripts.Abilities;
using ProtossMod.Scripts.Cards;
using StarCraftCore.Scripts.Abilities;

namespace ProtossMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("jamesgames.inscryption.starcraftcore", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.protossmod";
	    public const string PluginName = "Protoss Mod";
	    public const string PluginVersion = "0.1.0.0";
	    public const string DecalPath = "Artwork/watermark.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ProtossMod.dll", "");
            new Harmony(PluginGuid).PatchAll();

            // Sigils
            BuildInterceptorAbility.Initialize(typeof(BuildInterceptorAbility));
            SoulAbsorptionAbility.Initialize(typeof(SoulAbsorptionAbility));
            PylonAbility.Initialize(typeof(PylonAbility));
            PrismaticAlignmentAbility.Initialize(typeof(PrismaticAlignmentAbility));

            // Cards
            XelNagaArtifact.Initialize();

            //ChangeSquirrelToLarva();

            Logger.LogInfo($"Loaded {PluginName}!");
        }
        
        public void ChangeSquirrelToLarva()
        {
	        List<Ability> abilities = new List<Ability> { SplashDamageAbility.ability };

	        CardInfo card = CardManager.BaseGameCards.CardByName("Squirrel");
	        card.baseAttack = 1;
	        card.baseHealth = 10;
	        card.abilities = abilities;
	        card.decals = Utils.GetDecals();
        }
    }
}
