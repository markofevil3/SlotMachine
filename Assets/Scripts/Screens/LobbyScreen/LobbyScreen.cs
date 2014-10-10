using UnityEngine;
using System.Collections;

public class LobbyScreen : BaseScreen {

  public static LobbyScreen Instance;

  public UIButton btnLogin;
  public UIButton btnRegister;
  public UIButton btnPlayAsGuest;
  public UIButton btnSelectGame;
  public UIButton btnLogout;
  public UIGridExtent buttonGrid;
  public GameObject loginRegisterPanel;
	public UIButton[] tabBars = new UIButton[3];
  
  private LoginTab loginTab = null;
  private RegisterTab registerTab = null;
  private Tab currentTab = null;
  private UserBottomBar userBottomBar;
  
  // Test Data
  // public int testIndex = 1;
  // public string testString = "sadsad";

  void Awake() {
    Instance = this;
  }

  public override void Init(object[] data) {
    EventDelegate.Set(btnLogin.onClick, delegate() { OpenTab(Tab.TabType.LOGIN_TAB); });
    EventDelegate.Set(btnRegister.onClick, delegate() { OpenTab(Tab.TabType.REGISTER_TAB); });
    EventDelegate.Set(btnPlayAsGuest.onClick, EventPlayAsGuest);
    // EventDelegate.Set(btnSelectGame.onClick, EventMoveToSelectGameScreen);
    // TEST - for test slot game
    EventDelegate.Set(btnSelectGame.onClick, EventMoveToSlotScreen);
    EventDelegate.Set(btnLogout.onClick, EventLogout);

    GameObject tempGameObject = NGUITools.AddChild(gameObject, Resources.Load(Global.USER_BOTTOM_BAR_PREFAB, typeof(GameObject)) as GameObject);
   	tempGameObject.name = "UserBottomBar";
   	userBottomBar = tempGameObject.GetComponent<UserBottomBar>();
   	userBottomBar.Init(this);
   	
    EventDelegate.Set(tabBars[0].onClick, delegate() { OpenTab(Tab.TabType.LOGIN_TAB); });
   	EventDelegate.Set(tabBars[1].onClick, delegate() { OpenTab(Tab.TabType.REGISTER_TAB); });
   	EventDelegate.Set(tabBars[2].onClick, delegate() { OpenTab(Tab.TabType.POLICY_TAB); });
    
    if (SmartfoxClient.Instance.IsLoggedIn) {
      EventLoggedIn();
    }
    
    Invoke("TestDropdown", 2.0f);
  }

  void TestDropdown() {
    HUDManager.Instance.AddFlyText("AAAAAA", Vector3.zero, 40, Color.red);
    PopupManager.Instance.ShowNotification(string.Empty, "toi dang test");
  }
  
  public override void Open() {}

  public override void Close() {
		ScreenManager.Instance.LobbyScreen = null;
		base.Close();
	}

	private void OpenTab(Tab.TabType tabType) {
		if (!loginRegisterPanel.activeSelf) {
      loginRegisterPanel.SetActive(true);
    }
		if (currentTab != null && currentTab.type == tabType) {
			return;
		}
		if (currentTab != null) {
		  currentTab.Close(false);
		}
		for (int i = 0; i < tabBars.Length; i++) {
			if (i == (int)tabType) {
				tabBars[i].isEnabled = false;
			} else {
				tabBars[i].isEnabled = true;
			}
		}
		switch(tabType) {
			case Tab.TabType.LOGIN_TAB:
				if (loginTab == null) {
		    	GameObject tempGameObject = NGUITools.AddChild(loginRegisterPanel, Resources.Load(Global.SCREEN_PATH + "/LobbyScreen/LoginTab", typeof(GameObject)) as GameObject);
		     	tempGameObject.name = "LoginTab";
		     	loginTab = tempGameObject.GetComponent<LoginTab>();
		     	loginTab.Init();
		     	loginTab.Open();
		   	} else if (currentTab == null || currentTab.type != Tab.TabType.LOGIN_TAB) {
		   	  loginTab.Open();
		   	}
		   	currentTab = loginTab as Tab;
			break;
			case Tab.TabType.REGISTER_TAB:
				if (registerTab == null) {
		    	GameObject tempGameObject = NGUITools.AddChild(loginRegisterPanel, Resources.Load(Global.SCREEN_PATH + "/LobbyScreen/RegisterTab", typeof(GameObject)) as GameObject);
		     	tempGameObject.name = "RegisterTab";
		     	registerTab = tempGameObject.GetComponent<RegisterTab>();
		     	registerTab.Init();
		     	registerTab.Open();
		   	} else if (currentTab == null || currentTab.type != Tab.TabType.REGISTER_TAB) {
		   	  registerTab.Open();
		   	}
		   	currentTab = registerTab as Tab;
			break;
		}
	}
  
  private void EventPlayAsGuest() {
    PopupManager.Instance.ShowLoadingPopup();
    AccountManager.Instance.RegisterAsGuest();
  }
  
  // TEST
  private void EventMoveToSlotScreen() {
    SlotMachineClient.Instance.JoinRoom();
  }
  
  private void EventMoveToSelectGameScreen() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_GAME, null, true);
  }
  
  private void EventLogout() {
    PopupManager.Instance.ShowLoadingPopup();
    SmartfoxClient.Instance.LogoutUser();
  }
  
  public void EventLogoutSuccess() {
    btnLogin.gameObject.SetActive(true);
    btnRegister.gameObject.SetActive(true);
    btnSelectGame.gameObject.SetActive(false);
    btnLogout.gameObject.SetActive(false);
    btnPlayAsGuest.gameObject.SetActive(true);
    buttonGrid.Reposition();
    userBottomBar.EventUserLoggedOut();
  }
  
  public void EventLoggedIn() {
    if (loginTab != null) {
      loginTab.Close();
    }
    btnLogin.gameObject.SetActive(false);
    btnRegister.gameObject.SetActive(false);
    btnPlayAsGuest.gameObject.SetActive(false);
    btnSelectGame.gameObject.SetActive(true);
    btnLogout.gameObject.SetActive(true);
    buttonGrid.Reposition();
    loginRegisterPanel.SetActive(false);
    userBottomBar.EventUserLoggedIn();
  }

  public void EventLoginFail() {
    if (loginTab != null) {
      loginTab.ClearInput();
    }
  }
}
