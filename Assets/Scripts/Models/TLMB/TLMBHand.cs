using UnityEngine;
using System.Collections;

public class TLMBHand : Hand {
	public override object CreateInstance(string cardString) {
		return new TLMBCard(cardString);
	}

	public bool CanDrop(string cardsString) {
		TLMBCombination combination = new TLMBCombination(cardsString);

		if (combination.numCards != 1 && combination.type == (int)TLMBCombination.Type.NOTHING) {
			return false;
		}

		return true;
	}
}