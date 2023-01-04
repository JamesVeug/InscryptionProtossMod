using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using StarCraftCore.Scripts.Regions;

namespace ProtossMod.Scripts.Encounters
{
    public class ZealotRushEncounter
    {
        public static EncounterBlueprintData Instance = null;

        public static void Initialize()
        {
            EncounterBlueprintData data = EncounterManager.New("Protoss Zealot Rush");
            data.SetDifficulty(5, 15);
            data.AddDominantTribes(Tribe.Bird);
            data.AddRandomReplacementCards(new string[]
            {
                "Protoss_JSON_Adept",
            });
            data.AddTurnMods(new []
            {
                new EncounterBlueprintData.TurnModBlueprint()
                {
                    turn = 1,
                    applyAtDifficulty = 1,
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                    minDifficulty = 1,
                    maxDifficulty = 10
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Adept"),
                    minDifficulty = 11,
                    maxDifficulty = 0
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Adept"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Adept"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Protoss_JSON_Zealot"),
                }
            });

            RegionData region = AuirRegion.regionData;
            if (region != null)
            {
                data.regionSpecific = true;
                region.AddEncounters(data);
            }
            Instance = data;
        }
    }
}