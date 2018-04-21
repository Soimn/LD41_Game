using System;

namespace LudumDare41_Game.UI {
    struct HeldCardInfo {
        public bool isHeld;
        public Card heldCard;

        public HeldCardInfo (bool _isHeld, Card _heldCard) {
            isHeld = _isHeld;
            heldCard = _heldCard;
        }

        public Type GetCardType () {
            return heldCard.GetType();
        }
    }
}
