using UnityEngine;
using System.Collections;

//
// 3S =  0    3C = 13    3D = 26    3H = 39
// 4S =  1    4C = 14    4D = 27    4H = 40
// 5S =  2    5C = 15    5D = 28    5H = 41
// 6S =  3    6C = 16    6D = 29    6H = 42
// 7S =  4    7C = 17    7D = 30    7H = 43
// 8S =  5    8C = 18    8D = 31    8H = 44
// 9S =  6    9C = 19    9D = 32    9H = 45
// TS =  7    TC = 20    TD = 33    TH = 46
// JS =  8    JC = 21    JD = 34    JH = 47
// QS =  9    QC = 22    QD = 35    QH = 48
// KS = 10    KC = 23    KD = 36    KH = 49
// AS = 11    AC = 24    AD = 37    AH = 50
// 2S = 12    2C = 25    2D = 38    2H = 51
//

public class TLMBCard : Card {
	public enum Rank {
		THREE,
		FOUR,
		FIVE,
		SIX,
		SEVEN,
		EIGHT,
		NINE,
		TEN,
		JACK,
		QUEEN,
		KING,
		ACE,
		TWO
	};

	public TLMBCard(int index) : base(index) {
	}

	public TLMBCard(string cardsString) : base(cardsString) {
	}

	public override int GetRankOffset() {
		return 3;
	}

	public override string GetRankChar(int rank, int offset) {
		string rankChar = "0";

		switch (rank) {
			case 12: rankChar = "2"; break;
			case 11: rankChar = "A"; break;
			case 10: rankChar = "K"; break;
			case 9: rankChar = "Q"; break;
			case 8: rankChar = "J"; break;
			case 7: rankChar = "T"; break;
			default: rankChar = (rank + GetRankOffset()).ToString(); break;
		}

		return rankChar;
	}

	public override int CharsToIndex(char rank, char suit) {
		int r = INVALID;

		switch (rank) {
			case '2': r = (int)Rank.TWO; break;
			case 'T': r = (int)Rank.TEN; break;
			case 'J': r = (int)Rank.JACK; break;
			case 'Q': r = (int)Rank.QUEEN; break;
			case 'K': r = (int)Rank.KING; break;
			case 'A': r = (int)Rank.ACE; break;
			default: r = (int)rank - 51; break;
		}

		switch (suit) {
			case 'S': r = ToIndex(r, (int)Suit.SPADE); break;
			case 'C': r = ToIndex(r, (int)Suit.CLUB); break;
			case 'D': r = ToIndex(r, (int)Suit.DIAMOND); break;
			case 'H': r = ToIndex(r, (int)Suit.HEART); break;
		}

		return r;
	}

	public bool HasSameColor(TLMBCard another) {
		return base.HasSameColor((Card)another) || (rank == (int)Rank.TWO && another.rank == (int)Rank.TWO);
	}

	public bool IsTwo() {
		return rank == (int)Rank.TWO;
	}

	public bool IsAce() {
		return rank == (int)Rank.ACE;
	}
}