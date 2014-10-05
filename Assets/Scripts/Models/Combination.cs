using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combination : CardSet {
	public int type;

	public Combination() : base() {
		type = -1;
	}

	public Combination(List<Card> cards) : base(cards) {
		FindType();
	}

	public Combination(string cardsString) : base(cardsString) {
		FindType();
	}

	protected virtual void FindType() {
	}

	public virtual string GetTypeName() {
		return null;
	}
}