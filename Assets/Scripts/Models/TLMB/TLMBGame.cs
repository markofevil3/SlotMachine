using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TLMBGame {
	public enum State {
		WAITING,
		PLAYING
	}

	private string userId;
	private int seatIndex;
	private State state;
	private bool isHost;
	private bool isHandFinished;
	private TLMBHand hand;

	public TLMBGame(JSONObject jsonData) {
		Init(jsonData);
	}

	public void Init(JSONObject jsonData) {
		userId = jsonData.GetString("userId");
		seatIndex = jsonData.GetInt("seatIndex");
		isHost = jsonData.GetBoolean("isHost");
		state = (State)jsonData.GetInt("state");
		hand = new TLMBHand();

		if (SmartfoxClient.Instance.IsYou(userId)) {
			AddCards(jsonData.GetString("cards"));
		} else {
			int numCards = jsonData.GetInt("numCards");

			for (int i = 0; i < numCards; i++) {
				hand.AddCard(new TLMBCard(-1));
			}
		}
	}

	public void AddCards(string cardsString) {
		hand.AddCards(cardsString);
	}

	public int Drop(TLMBCombination lastDroppedCards, string cardsString) {
		if (!hand.Contains(cardsString)) {
			return (int)ErrorCode.TLMB.CARDS_NOT_EXIST;
		} else if (!hand.CanDrop(cardsString)) {
			return (int)ErrorCode.TLMB.CANNOT_DROP;
		}

		if (lastDroppedCards == null || lastDroppedCards.IsEmpty() || 
			TLMBCombination.CanDefeat(new TLMBCombination(cardsString), lastDroppedCards)) {
			return (int)ErrorCode.TLMB.NULL;
		}

		return (int)ErrorCode.TLMB.CANNOT_DEFEAT;
	}
}