using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CardSet {
	public enum Order {
		ASC = 1,
		DESC = -1
	}

	public List<Card> cards;
	public int numCards;

	public CardSet() {
		cards = new List<Card>();
		numCards = 0;
	}

	public CardSet(string cardsString) : this() {
		AddCards(cardsString);
	}

	public CardSet(List<Card> cards) {
		this.cards = cards;
		numCards = cards.Count;
	}

	public void AddCards(string cardsString) {
		cardsString = cardsString.Replace(" ", "");
		int count = cardsString.Length / 2;
		
		for (int i = 0; i < count; i++) {
			AddCard(CreateInstance(cardsString.Substring(i * 2, 2)));
		}
	}

	public void AddCard(object card) {
		cards.Add((Card)card);
		numCards++;
	}

	public virtual object CreateInstance(string cardString) {
		return new Card(cardString);
	}

	public void RemoveCard(int cardIndex) {
		for (int i = 0; i < numCards; i++) {
			if (At(i).index == cardIndex) {
				cards.RemoveAt(i);
				numCards--;
				break;
			}
		}
	}

	public void RemoveCard(Card card) {
		for (int i = 0; i < numCards; i++) {
			if (At(i).IsEqual(card)) {
				cards.RemoveAt(i);
				numCards--;
				break;
			}
		}
	}

	public void RemoveCard(string cardString) {
		RemoveCard(new Card(cardString));
	}

	public bool Contains(string cardsString) {
		cardsString = cardsString.Replace(" ", "");
		int count = cardsString.Length / 2;
		
		for (int i = 0; i < count; i++) {
			if (!Contains(new Card(cardsString.Substring(i * 2, i * 2 + 2)))) {
				return false;
			}
		}
		
		return true;
	}

	public bool Contains(Card card) {
		for (int i = 0; i < numCards; i++) {
			if (At(i).IsEqual(card)) {
				return true;
			}
		}

		return false;
	}

	public bool IsEmpty() {
		return numCards == 0;
	}

	public Card At(int index) {
		return index < numCards ? cards[index] : null;
	}

	public string ToCardString() {
		if (numCards == 0) {
			return "";
		}
		
		string st = "";
		
		for (int i = 0; i < numCards - 1; i++) {
			st += At(i).ToString() + " ";
		}
		
		st += At(numCards - 1).ToString();
		return st;
	}

	public void Sort(Order sortOrder) {
		cards.Sort(delegate(Card card1, Card card2) {
			return card1.CompareTo(card2) * (int)sortOrder;
		});
	}

	public void Reset() {
		cards.Clear();
		numCards = 0;
	}
}