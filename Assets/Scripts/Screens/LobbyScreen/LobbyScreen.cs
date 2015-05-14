using UnityEngine;
using System.Collections;

public class LobbyScreen : BaseScreen {

  public static LobbyScreen Instance;
  public UIButton btnLoginFB;
  public UIButton btnLogin;
  public UIButton btnRegister;
  public UIButton btnPlayAsGuest;
  public UIButton btnSelectGame;
  public UIButton btnLogout;
  public UIGridExtent buttonGrid;
  public GameObject loginRegisterPanel;
  
  private LoginTab loginTab = null;
  private RegisterTab registerTab = null;
  private Tab currentTab = null;
  
  // Test Data
  // public int testIndex = 1;
  // public string testString = "sadsad";

  void Awake() {
    Instance = this;
  }

  public override void Init(object[] data) {
    EventDelegate.Set(btnLogin.onClick, OpenLoginPanel);
    EventDelegate.Set(btnRegister.onClick, delegate() { OpenTab(Tab.TabType.REGISTER_TAB); });
    EventDelegate.Set(btnPlayAsGuest.onClick, EventPlayAsGuest);
    EventDelegate.Set(btnSelectGame.onClick, EventMoveToSelectGameScreen);
    EventDelegate.Set(btnLoginFB.onClick, EventLoginFB);
		NGUITools.SetActive(btnLoginFB.gameObject, !MyFacebook.Instance.IsLoggedIn);
    // TEST - for test slot game
    // EventDelegate.Set(btnSelectGame.onClick, EventMoveToSlotScreen);
    EventDelegate.Set(btnLogout.onClick, EventLogout);
		Utils.SetActive(buttonGrid.gameObject, true);
		Utils.SetActive(loginRegisterPanel, false);		
    if (SmartfoxClient.Instance.IsLoggedIn) {
      EventLoggedIn();
    }
		
		MyFacebook.Instance.Init();
    // Invoke("TestDropdown", 2.0f);
  }

  // void TestDropdown() {
  //   HUDManager.Instance.AddFlyText("AAAAAA", Vector3.zero, 40, Color.red);
  //   PopupManager.Instance.ShowNotification(string.Empty, "toi dang test");
  // }
  
  public override void Open() {
  }

  public override void Close() {
		ScreenManager.Instance.LobbyScreen = null;
		base.Close();
	}

	public void EventCloseSubPanel() {
		if (NGUITools.GetActive(loginRegisterPanel)) {
			Utils.SetActive(loginRegisterPanel, false);
    }
		Utils.SetActive(buttonGrid.gameObject, true);
	}

	private void OpenLoginPanel() {
		if (!NGUITools.GetActive(loginRegisterPanel)) {
			Utils.SetActive(loginRegisterPanel, true);
    }
		Utils.SetActive(buttonGrid.gameObject, false);
		if (loginTab == null) {
    	GameObject tempGameObject = NGUITools.AddChild(loginRegisterPanel, Resources.Load(Global.SCREEN_PATH + "/LobbyScreen/LoginTab", typeof(GameObject)) as GameObject);
     	tempGameObject.name = "LoginTab";
     	loginTab = tempGameObject.GetComponent<LoginTab>();
     	loginTab.Init();
     	loginTab.Open();
   	} else {
   	  loginTab.Open();
   	}
	}

	private void OpenTab(Tab.TabType tabType) {
		if (!loginRegisterPanel.activeSelf) {
			Utils.SetActive(loginRegisterPanel, true);
    }
		if (currentTab != null && currentTab.type == tabType) {
			return;
		}
		if (currentTab != null) {
		  currentTab.Close(false);
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
  
	private void EventLoginFB() {
		MyFacebook.Instance.Login();
	}
	
  private void EventPlayAsGuest() {
    AccountManager.Instance.RegisterAsGuest();
  }
  
  private void EventMoveToSelectGameScreen() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.SELECT_GAME, null, true);
  }
  
  private void EventLogout() {
    PopupManager.Instance.ShowLoadingPopup("LoadingText_Logout");
    SmartfoxClient.Instance.LogoutUser();
  }
  
  public void EventLogoutSuccess() {
		Utils.SetActive(btnLogin.gameObject, true);
		Utils.SetActive(btnRegister.gameObject, true);
		Utils.SetActive(btnSelectGame.gameObject, false);
		Utils.SetActive(btnLogout.gameObject, false);
		Utils.SetActive(btnPlayAsGuest.gameObject, true);
		NGUITools.SetActive(btnLoginFB.gameObject, AccountManager.Instance.fbId == string.Empty);
    buttonGrid.Reposition();
    // userBottomBar.EventUserLoggedOut();
  }
  
  public void EventLoggedIn() {
    if (loginTab != null) {
      loginTab.Close();
    }
		Utils.SetActive(btnLogin.gameObject, false);
		Utils.SetActive(btnRegister.gameObject, false);
		Utils.SetActive(btnPlayAsGuest.gameObject, false);
		Utils.SetActive(btnSelectGame.gameObject, true);
		Utils.SetActive(btnLogout.gameObject, true);
		NGUITools.SetActive(btnLoginFB.gameObject, AccountManager.Instance.fbId == string.Empty);
    buttonGrid.Reposition();
		Utils.SetActive(loginRegisterPanel, false);
    // userBottomBar.EventUserLoggedIn();
  }

  public void EventLoginFail() {
    if (loginTab != null) {
      loginTab.ClearInput();
    }
  }
}
