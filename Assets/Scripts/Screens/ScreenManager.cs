using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

	public static ScreenManager Instance { get; private set; }
	public GameObject root;
	public Camera camera;
	
	public BaseScreen currentScreen;
	private BaseScreen previousScreen;
	private LobbyScreen lobbyScreen;
	private SelectGameScreen selectGameScreen;
	private SelectRoomScreen selectRoomScreen;
	private LeaderboardScreen leaderboardScreen;
	private BaseGameScreen gameScreen;
	private BaseSlotMachineScreen slotScreen;
	
	public LobbyScreen LobbyScreen {
		get { return lobbyScreen; }
		set { lobbyScreen = value; }
	}
	
	public SelectGameScreen SelectGameScreen {
		get { return selectGameScreen; }
		set { selectGameScreen = value; }
	}
	
	public SelectRoomScreen SelectRoomScreen {
		get { return selectRoomScreen; }
		set { selectRoomScreen = value; }
	}
	
	public LeaderboardScreen LeaderboardScreen {
	  get { return leaderboardScreen; }
	  set { leaderboardScreen = value; }
	}
	
	public BaseGameScreen CurrentGameScreen {
		get { return gameScreen; }
		set { gameScreen = value; }
	}
	
	public BaseSlotMachineScreen CurrentSlotScreen {
		get { return slotScreen; }
		set { slotScreen = value; }
	}
	
	void Start() {
	  Application.targetFrameRate = 60;
		Application.runInBackground = true;
		Global.Init();
		Global.isTablet = Utils.IsTablet();
	  Instance = this;
	  SetScreen(BaseScreen.Type.LOBBY, null, true);
	}
	
	public void Restart() {
		if (currentScreen != null) {
			currentScreen.FadeOutAndDestroy(gameObject, "RestartCallback");
			currentScreen = null;
		} else {
			RestartCallback();
		}
		if (previousScreen != null) {
			previousScreen.Close();
			previousScreen = null;
		}
	}
	
	void RestartCallback() {
		Invoke("Start", 0.2f);
	}
	
	public void BackToPrevScreen() {
	  if (previousScreen != null) {
	    currentScreen.Close();
	    previousScreen.Show();
	    currentScreen = previousScreen;
	  }
	}
	
  public void SetScreen(BaseScreen.Type type, object[] data = null, bool isFadeIn = false, bool keepPrevScreen = false) {
		if (currentScreen != null) {
		  if (currentScreen.type == type) {
		    return;
		  }
		  if (keepPrevScreen) {
		    previousScreen = currentScreen;
		    previousScreen.Hide();
		  } else {
		    currentScreen.Close();
		  }
		}
		GameObject tempGameObject;
		switch(type) {
			case BaseScreen.Type.LOBBY:
      	tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/LobbyScreen/LobbyScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "LobbyScreen";
       	LobbyScreen = tempGameObject.GetComponent<LobbyScreen>();
				currentScreen = LobbyScreen;
       	LobbyScreen.Init(data);
	     	LobbyScreen.Open();
			break;
			case BaseScreen.Type.SELECT_GAME:
      	tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/SelectGameScreen/SelectGameScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SelectGameScreen";
       	SelectGameScreen = tempGameObject.GetComponent<SelectGameScreen>();
				currentScreen = SelectGameScreen;
       	SelectGameScreen.Init(data);
	     	SelectGameScreen.Open();
			break;
			case BaseScreen.Type.SELECT_ROOM:
      	tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/SelectRoomScreen/SelectRoomScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SelectRoomScreen";
       	SelectRoomScreen = tempGameObject.GetComponent<SelectRoomScreen>();
				currentScreen = SelectRoomScreen;
       	SelectRoomScreen.Init(data);
	     	SelectRoomScreen.Open();
			break;
			case BaseScreen.Type.LEADERBOARD:
      	tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/LeaderboardScreen/LeaderboardScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "LeaderboardScreen";
       	LeaderboardScreen = tempGameObject.GetComponent<LeaderboardScreen>();
				currentScreen = LeaderboardScreen;
       	LeaderboardScreen.Init(data);
	     	LeaderboardScreen.Open();
			break;
			// when open gamescreen, first element in data will be GameType
			case BaseScreen.Type.GAME_SCREEN:
      	currentScreen = SetGameScreen((BaseGameScreen.GameType)data[0], data);
			break;
			case BaseScreen.Type.SLOT_GAME_SCREEN:
      	currentScreen = SetSlotGameScreen((BaseSlotMachineScreen.GameType)data[0], data);
			break;
		}
		if (isFadeIn) {
      // currentScreen.FadeIn();
      PopupManager.Instance.FadeInScreen();
		}
  }
  
  private BaseScreen SetSlotGameScreen(BaseSlotMachineScreen.GameType gameType, object[] data = null) {
    GameObject tempGameObject;
    BaseSlotMachineScreen tempScreen;
    switch(gameType) {
      case BaseSlotMachineScreen.GameType.SLOT_FRUITS:
        tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotFruitScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SlotFruitScreen";
       	SlotFruitsScreen slotFruitsScreen = tempGameObject.GetComponent<SlotFruitsScreen>();
       	slotFruitsScreen.Init(data);
	     	slotFruitsScreen.Open();
	     	tempScreen = slotFruitsScreen as BaseSlotMachineScreen;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN:
        tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotHalloweenScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SlotHalloweenScreen";
       	SlotHalloweenScreen slotHalloweenScreen = tempGameObject.GetComponent<SlotHalloweenScreen>();
       	slotHalloweenScreen.Init(data);
	     	slotHalloweenScreen.Open();
	     	tempScreen = slotHalloweenScreen as BaseSlotMachineScreen;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_DRAGON:
        tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotDragonScreen/SlotDragonScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SlotDragonScreen";
       	SlotDragonScreen slotDragonScreen = tempGameObject.GetComponent<SlotDragonScreen>();
       	slotDragonScreen.Init(data);
	     	slotDragonScreen.Open();
	     	tempScreen = slotDragonScreen as BaseSlotMachineScreen;
      break;
      case BaseSlotMachineScreen.GameType.SLOT_PIRATE:
        tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/GameScreen/SlotMachine/SlotPirateScreen/SlotPirateScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "SlotPirateScreen";
       	SlotPirateScreen slotPirateScreen = tempGameObject.GetComponent<SlotPirateScreen>();
       	slotPirateScreen.Init(data);
	     	slotPirateScreen.Open();
	     	tempScreen = slotPirateScreen as BaseSlotMachineScreen;
      break;
      default:
        tempScreen = null;
      break;
    }
    CurrentSlotScreen = tempScreen as BaseSlotMachineScreen;
    return tempScreen;
  }
  
  private BaseScreen SetGameScreen(BaseGameScreen.GameType gameType, object[] data = null) {
    GameObject tempGameObject;
    BaseScreen tempScreen;
    switch(gameType) {
      case BaseGameScreen.GameType.TIEN_LEN_MB:
        tempGameObject = NGUITools.AddChild(root, Resources.Load(Global.SCREEN_PATH + "/GameScreen/TienLenMN/TienLenMNScreen", typeof(GameObject)) as GameObject);
       	tempGameObject.name = "TienLenMNScreen";
       	TienLenMNScreen tienLenMNScreen = tempGameObject.GetComponent<TienLenMNScreen>();
       	tienLenMNScreen.Init(data);
	     	tienLenMNScreen.Open();
	     	tempScreen = tienLenMNScreen as BaseGameScreen;
      break;
      default:
        tempScreen = null;
      break;
    }
    CurrentGameScreen = tempScreen as BaseGameScreen;
    return tempScreen;
  }
}
