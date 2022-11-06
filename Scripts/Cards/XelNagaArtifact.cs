using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;
using StarCraftCore.Scripts.Backgrounds;

namespace ProtossMod.Scripts.Cards
{
    public static class XelNagaArtifact
    {
        private const string ID = "Xel'naga Artifact 2";
        private const string DisplayName = "Strange Artifact";
        private const string Description = "What the hell is that?";
        private const string TextureFile = "Artwork/Cards/xelnaga_artifact_2.png";
        private const string EmitTextureFile = "Artwork/Cards/xelnaga_artifact_2_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 1;
        private const int energyCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(XelNagaBackground.CustomAppearance);
            
            List<Ability> abilities = new List<Ability>{ };
            abilities.Add(Ability.QuadrupleBones);
            abilities.Add(Ability.DrawCopyOnDeath);

            List<Trait> traits = new List<Trait> { Trait.Goat };
            List<Tribe> tribes = new List<Tribe> { Tribe.Insect, Tribe.Bird, Tribe.Canine };

            CardInfo card = CardManager.New(modPrefix: Plugin.PluginName, 
                name: ID,
                displayName: DisplayName,
                attack: BaseAttack,
                health: BaseHealth,
                description: Description);

            card.metaCategories = metaCategories;
            card.cardComplexity = CardComplexity.Simple;
            card.temple = CardTemple.Nature;
            card.SetCost(energyCost, BoneCost);
            card.abilities = abilities;
            card.appearanceBehaviour = appearanceBehaviour;
            card.SetPortrait(Utils.GetTextureFromPath(TextureFile));
            card.SetEmissivePortrait(Utils.GetTextureFromPath(EmitTextureFile));
            card.traits = traits;
            card.tribes = tribes;
            card.decals = Utils.GetDecals();
            card.onePerDeck = true;
        }
    }
}
