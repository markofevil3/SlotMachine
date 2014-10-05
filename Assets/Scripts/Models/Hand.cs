using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : CardSet {
	public List<Combination> combinations;

	public Hand() : base() {
		combinations = new List<Combination>();
	}

	public Hand(List<Card> cards) : base(cards) {
		combinations = new List<Combination>();
	}

	public virtual List<Combination> FindCombinations() {
		return combinations;
	}
}