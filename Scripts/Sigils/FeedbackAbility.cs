using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using Pixelplacement;
using StarCraftCore.Scripts.Abilities;
using UnityEngine;
using StarCraftCore.Scripts.Data.Sigils;

namespace ProtossMod.Scripts.Abilities
{
    public class FeedbackAbility : ACustomAbilityBehaviour<FeedbackAbility, AbilityData>
    {
	    public override Ability Ability => ability;
	    public static Ability ability = Ability.None;
	    
        private CardSlot m_targetedCardSlot = null;

        public static void Initialize(Type declaringType)
        {
	        ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
        }

        public override bool RespondsToResolveOnBoard()
        {
	        return Card.Slot.IsPlayerSlot && CanFeedback();
        }
        
        public override IEnumerator OnResolveOnBoard()
        {
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.ChoosingSlotViewMode, false);
	        
	        // Find who we want to abduct
	        IEnumerator onResolveOnBoard = ChooseTarget();
	        yield return onResolveOnBoard;
	        
	        // Make sure its a valid target
	        CardSlot cardSlot = m_targetedCardSlot;
	        CardSlot slot = StarCraftCore.Utils.GetSlot(this.Card);
	        if (cardSlot != null && (cardSlot.Card != null && cardSlot.Card != this.Card && cardSlot.Index != slot.Index))
	        {
		        // Pull them to the correct slot
		        yield return Feedback(cardSlot);
	        }
	        else
	        {
		        Card.Anim.StrongNegationEffect();
		        yield return new WaitForSeconds(0.3f);
	        }

	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.DefaultViewMode, false);
	        Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
	        Singleton<CombatPhaseManager>.Instance.VisualizeClearSniperAbility();
        }

        private IEnumerator ChooseTarget()
        {
	        CombatPhaseManager combatPhaseManager = Singleton<CombatPhaseManager>.Instance;
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        List<CardSlot> allSlots = new List<CardSlot>(boardManager.AllSlots);

	        Action<CardSlot> callback1 = null;
	        Action<CardSlot> callback2 = null;
	        
	        combatPhaseManager.VisualizeStartSniperAbility(Card.slot);
	        
	        CardSlot cardSlot = Singleton<InteractionCursor>.Instance.CurrentInteractable as CardSlot;
	        if (cardSlot != null && allSlots.Contains(cardSlot))
	        {
		        combatPhaseManager.VisualizeAimSniperAbility(Card.slot, cardSlot);
	        }

	        List<CardSlot> allTargetSlots = allSlots;
	        List<CardSlot> validTargetSlots = ValidSlots();

	        m_targetedCardSlot = null;
	        Action<CardSlot> targetSelectedCallback;
	        if ((targetSelectedCallback = callback1) == null)
	        {
		        targetSelectedCallback = (callback1 = delegate(CardSlot s)
		        {
			        m_targetedCardSlot = s;
			        combatPhaseManager.VisualizeConfirmSniperAbility(s);
		        });
	        }
	        
	        Action<CardSlot> invalidTargetCallback = null;
	        Action<CardSlot> slotCursorEnterCallback;
	        if ((slotCursorEnterCallback = callback2) == null)
	        {
		        slotCursorEnterCallback = (callback2 = delegate(CardSlot s)
		        {
			        combatPhaseManager.VisualizeAimSniperAbility(Card.slot, s);
		        });
	        }

	        yield return boardManager.ChooseTarget(allTargetSlots, validTargetSlots, targetSelectedCallback, invalidTargetCallback, slotCursorEnterCallback, () => false, CursorType.Target);
        }

        private IEnumerator WiggleEffect()
        {
	        Card.Anim.StrongNegationEffect();
	        yield return new WaitForSeconds(0.3f);
        }
        
        private IEnumerator Feedback(CardSlot targetSlot)
        {
	        yield return base.PreSuccessfulTriggerSequence();

	        PlayableCard targetCard = targetSlot.Card;
	        yield return targetCard.TakeDamage(targetCard.Attack, this.Card);
	        
	        yield return new WaitForSeconds(0.3f);
	        yield return base.LearnAbility();
        }

        private bool CanFeedback()
        {
	        return ValidSlots().Count > 0;
        }

        private List<CardSlot> ValidSlots()
        {
	        List<CardSlot> validSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.opponentSlots);
	        validSlots.RemoveAll((a) => a.Card == null || a.Card.Dead);
	        return validSlots;
        }

    }
}