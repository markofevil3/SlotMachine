using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TLMBGameData {
	public static TLMBRoom currentRoom;

	public static void Init() {
		currentRoom = new TLMBRoom();
	}

	public static void LoadRoom(JSONObject jsonData) {
		currentRoom.Init(jsonData);
	}

	public static void UpdateRoom() {

	}

	public static void Destroy() {
	}
}