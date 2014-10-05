using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TLMBGameConfig {
	public enum Type {
		NORMAL,
		COUNT
	};

	public Type type;
	public int coinPerCard;
	public int firstRankRewardCoin;
	public int secondRankRewardCoin;

	public TLMBGameConfig() {
	}

	public TLMBGameConfig(JSONObject jsonData) {
		type = (Type)jsonData.GetInt("type");
		coinPerCard = jsonData.GetInt("coinPerCard");
		firstRankRewardCoin = jsonData.GetInt("firstRankRewardCoin");
		secondRankRewardCoin = jsonData.GetInt("secondRankRewardCoin");
	}

	public static TLMBGameConfig CreateNormalGame(int firstRankRewardCoin, int secondRankRewardCoin) {
		TLMBGameConfig gameConfig = new TLMBGameConfig();
		gameConfig.firstRankRewardCoin = firstRankRewardCoin;
		gameConfig.secondRankRewardCoin = secondRankRewardCoin;
		return gameConfig;
	}

	public static TLMBGameConfig CreateCountGame(int coinPerCard) {
		TLMBGameConfig gameConfig = new TLMBGameConfig();
		gameConfig.coinPerCard = coinPerCard;
		return gameConfig;	
	}

	public JSONObject ToJsonObject() {
		JSONObject jsonData = new JSONObject();
		jsonData.Add("type", (int)type);
		jsonData.Add("coinPerCard", coinPerCard);
		jsonData.Add("firstRankRewardCoin", firstRankRewardCoin);
		jsonData.Add("secondRankRewardCoin", secondRankRewardCoin);
		return jsonData;
	}
}