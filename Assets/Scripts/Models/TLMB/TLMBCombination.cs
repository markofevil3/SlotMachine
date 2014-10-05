using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TLMBCombination : Combination {
	public enum Type {
		NOTHING,
		TWO,
		COUPLE,
		TRIPLE,
		QUAD,
		STRAIGHT,
		THREE_COUPLES,
		FOUR_COUPLES,
		DOUBLE_QUAD,
		FIVE_COUPLES,
		SIX_COUPLES,
		TRIPLE_QUAD
	};

	public bool hasAceAdvantage;
	public bool hasTwoAdvantage;

	public TLMBCombination() : base() {
	}

	public TLMBCombination(List<Card> cards) : base(cards) {
	}

	public TLMBCombination(string cardsString) : base(cardsString) {
	}

	public override object CreateInstance(string cardString) {
		return new TLMBCard(cardString);
	}

	protected override void FindType() {
		Sort(Order.ASC);
		type = (int)Type.NOTHING;
		hasTwoAdvantage = false;
		hasAceAdvantage = false;

		switch (numCards) {
			case 1:
				if (At(0).rank == (int)TLMBCard.Rank.TWO) {
					SetType(Type.TWO);
				}
			break;
			case 2:
				if (IsCouple(At(0), At(1))) {
					SetType(Type.COUPLE);
					hasTwoAdvantage = At(0).IsTwo();
				}
			break;
			case 3:
				if (At(0).HasSameRank(At(2))) {
					SetType(Type.TRIPLE);
					hasTwoAdvantage = At(0).IsTwo();
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 4:
				if (At(0).HasSameRank(At(3))) {
					SetType(Type.QUAD);
					hasAceAdvantage = At(0).IsAce();
					hasTwoAdvantage = At(0).IsTwo();
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 5:
			case 7:
			case 9:
			case 11:
			case 13:
				if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 6:
				if (IsCouple(At(0), At(1)) && IsCouple(At(2), At(3)) && IsCouple(At(4), At(5)) &&
					At(0).HasSameColor(At(2)) && At(0).HasSameColor(At(4)) && At(4).rank - At(0).rank == 2) {
					SetType(Type.THREE_COUPLES);
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 8:
				if (IsCouple(At(0), At(1)) && IsCouple(At(2), At(3)) && IsCouple(At(4), At(5)) && IsCouple(At(6), At(7)) &&
					At(0).HasSameColor(At(2)) && At(0).HasSameColor(At(4)) && At(0).HasSameColor(At(6)) && At(6).rank - At(0).rank == 3) {
					SetType(Type.FOUR_COUPLES);
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 10:
				if (IsCouple(At(0), At(1)) && IsCouple(At(2), At(3)) && IsCouple(At(4), At(5)) && IsCouple(At(6), At(7)) &&
					IsCouple(At(8), At(9)) && At(0).HasSameColor(At(2)) && At(0).HasSameColor(At(4)) && 
					At(0).HasSameColor(At(6)) && At(0).HasSameColor(At(8)) && At(8).rank - At(0).rank == 4) {
					SetType(Type.FIVE_COUPLES);
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
			case 12:
				if (IsCouple(At(0), At(1)) && IsCouple(At(2), At(3)) && IsCouple(At(4), At(5)) && IsCouple(At(6), At(7)) &&
					IsCouple(At(8), At(9)) && IsCouple(At(10), At(11)) && At(0).HasSameColor(At(2)) && At(0).HasSameColor(At(4)) && 
					At(0).HasSameColor(At(6)) && At(0).HasSameColor(At(8)) && At(0).HasSameColor(At(10)) && At(10).rank - At(0).rank == 5) {
					SetType(Type.FIVE_COUPLES);
				} else if (IsStraight()) {
					SetType(Type.STRAIGHT);
				}
			break;
		}
	}

	public bool CanDefeat(TLMBCombination another) {
		switch ((Type)another.type) {
			case Type.NOTHING:
				switch ((Type)type) {
					case Type.NOTHING:
						return numCards == 1 && At(0).HasSameSuit(another.At(0)) && At(0).GetRank() > another.At(0).GetRank();
					default: return type == (int)Type.TWO;
				}
			break;
			case Type.TWO:
				switch ((Type)type) {
					case Type.TWO: return At(0).GetSuit() > another.At(0).GetSuit();
					case Type.QUAD:
					case Type.THREE_COUPLES:
					case Type.FOUR_COUPLES:
					case Type.FIVE_COUPLES:
					case Type.SIX_COUPLES:
						return true;
					default: return false;
				}
			break;
			case Type.COUPLE:
				switch ((Type)type) {
					case Type.COUPLE:
						if (another.hasTwoAdvantage) {
							return hasTwoAdvantage && At(1).GetSuit() == (int)Card.Suit.HEART;
						} else {
							return hasTwoAdvantage || (At(0).HasSameColor(another.At(0)) && At(0).GetRank() > another.At(0).GetRank());
						}
					break;
					case Type.QUAD: return another.hasTwoAdvantage && hasAceAdvantage;
					case Type.FOUR_COUPLES:
					case Type.FIVE_COUPLES:
					case Type.SIX_COUPLES:
					case Type.DOUBLE_QUAD:
						return another.hasTwoAdvantage;
					default: return false;
				}
			break;
			case Type.TRIPLE:
				return type == (int)Type.TRIPLE && At(0).GetRank() > another.At(0).GetRank() &&
						At(0).GetSuit() == another.At(0).GetSuit() && At(1).GetSuit() == another.At(1).GetSuit() && 
						At(2).GetSuit() == another.At(2).GetSuit();
			case Type.QUAD:
				return type == (int)Type.QUAD && At(0).GetRank() > another.At(0).GetRank();
			case Type.STRAIGHT:
				return type == (int)Type.STRAIGHT && numCards >= another.numCards && 
						At(0).GetSuit() == another.At(0).GetSuit() && At(0).GetRank() > another.At(another.numCards - 1).GetRank();
			case Type.THREE_COUPLES:
				return type == (int)Type.THREE_COUPLES && At(0).HasSameColor(another.At(0)) && At(0).GetRank() > another.At(0).GetRank();
			case Type.DOUBLE_QUAD:
				return type == (int)Type.DOUBLE_QUAD && At(0).GetRank() > another.At(0).GetRank() && At(4).GetRank() > another.At(4).GetRank();
			default:
				return false;
		}

	}

	public TLMBCard At(int index) {
		return (TLMBCard)base.At(index);
	}

	public override string GetTypeName() {
		string name = "Nothing";

		switch ((Type)type) {
			case Type.COUPLE: name = "Couple"; break;
			case Type.TRIPLE: name = "Triple"; break;
			case Type.QUAD: name = "Quad"; break;
			case Type.STRAIGHT: name = "Straight"; break;
			case Type.THREE_COUPLES: name = "Three couples"; break;
			case Type.FOUR_COUPLES: name = "Four couples"; break;
			case Type.FIVE_COUPLES: name = "Five couples"; break;
			case Type.SIX_COUPLES: name = "Six couples"; break;
		}

		return name;
	}

	public bool IsCouple(TLMBCard card1, TLMBCard card2) {
		return card1.HasSameColor(card2) && card1.HasSameRank(card2);
	}

	public bool IsStraight() {
		if (At(numCards - 1).rank - At(0).rank != numCards - 1) {
			return false;
		}

		for (int i = 0; i < numCards - 1; i++) {
			if (At(i).suit != At(i + 1).suit) {
				return false;
			}
		}

		return true;
	}

	public void SetType(Type aType) {
		type = (int)aType;
	}

	public static bool CanDefeat(TLMBCombination current, TLMBCombination previous) {
		return current.CanDefeat(previous);
	}
}