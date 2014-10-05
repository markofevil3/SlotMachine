using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class TLMBRoom {
	public enum State {
		WAITING,
		STARTING,
		PLAYING,
		FINISHING,
		FINISHED
	};

	private string roomId;
	private List<TLMBGame> userGames;
	private TLMBCombination droppedCards;
	private TLMBCombination roundDroppedCards;
	private State state;
	private TLMBGameConfig gameConfig;
	private int activeUserSeatIndex;
	private int remainingSeconds;

	public TLMBRoom() {
		userGames = new List<TLMBGame>();
	}

	public void Init(JSONObject jsonData) {
		roomId = jsonData.GetString("roomId");
		userGames.Clear();
		JSONArray userGameArray = jsonData.GetArray("userGames");

		for (int i = 0; i < userGameArray.Length; i++) {
			userGames.Add(new TLMBGame(userGameArray[i].Obj));
		}

		droppedCards = new TLMBCombination(jsonData.GetString("droppedCards"));
		roundDroppedCards = new TLMBCombination(jsonData.GetString("roundDroppedCards"));
		activeUserSeatIndex = jsonData.GetInt("activeUserSeatIndex");
		state = (State)jsonData.GetInt("state");
		gameConfig = new TLMBGameConfig(jsonData.GetObject("gameConfig"));
		remainingSeconds = jsonData.GetInt("remainingSeconds");
	}
}