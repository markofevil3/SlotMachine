using UnityEngine;
using System.Collections;

public class Card {
	public const int TOTAL = 52;
	public const int NUM_SUITS = 4;
	public const int NUM_RANKS = 13;
	public const int INVALID = -1;

	public enum Suit {
		SPADE,
		CLUB,
		DIAMOND,
		HEART
	};

	public int suit;
	public int rank;

	// If index = -1, it's an invalid card or a face-down card.
	public int index;

	public Card() {
		index = INVALID;
		rank = INVALID;
		suit = INVALID;
	}

	public Card(int index) {
		this.index = index;
		this.rank = GetRank();
		this.suit = GetSuit();
	}

	public Card(string cardsString) {
		index = INVALID;

		if (cardsString != null && cardsString.Length == 2) {
			index = CharsToIndex(cardsString[0], cardsString[1]);
			rank = GetRank();
			suit = GetSuit();
		}
	}

	public int ToIndex() {
		return ToIndex(rank, suit);
	}

	public int ToIndex(int rank, int suit) {
		return suit * NUM_RANKS + rank;
	}

	public int GetRank() {
		return index % NUM_RANKS;
	}

	public int GetSuit() {
		return index / NUM_RANKS;
	}

	public bool IsEqual(Card another) {
		return index == another.index;
	}

	public string ToString() {
		return ToString(rank, suit);
	}

	public string ToString(int rank, int suit) {
		string cardString = "";
		string rankChar = GetRankChar(rank, GetRankOffset()).ToString();

		switch ((Suit)suit) {
			case Suit.SPADE: cardString = rankChar + "S"; break;
			case Suit.CLUB: cardString = rankChar + "C"; break;
			case Suit.DIAMOND: cardString = rankChar + "D"; break;
			case Suit.HEART: cardString = rankChar + "H"; break;
		}

		return cardString;
	}

	public bool HasSameColor(Card another) {
		return (suit < (int)Suit.DIAMOND && another.suit < (int)Suit.DIAMOND) ||
				(suit >= (int)Suit.DIAMOND && another.suit >= (int)Suit.DIAMOND);
	}

	public bool HasSameRank(Card another) {
		return rank == another.GetRank();
	}

	public bool HasSameSuit(Card another) {
		return suit == another.GetSuit();
	}

	public virtual int GetRankOffset() {
		return 0;
	}

	public virtual string GetRankChar(int rank, int offset) {
		return "";
	}

	public virtual int CharsToIndex(char rank, char suit) {
		return INVALID;
	}

	public int CompareTo(Card another) {
		if (rank < another.rank) {
			return -1;
		} else if (rank > another.rank) {
			return 1;
		}

		if (suit < another.suit) {
			return -1;
		} else if (suit > another.suit) {
			return 1;
		}

		return 0;
	}
}