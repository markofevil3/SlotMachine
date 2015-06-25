using UnityEngine;
using System;
using System.Collections;

public class SelectGameScreen : BaseScreen {
	
  public UIButton btnLeaderboard;
  public UIButton btnDailyBonus;
  public UIButton btnZombieGame;
  public UIButton btnDragonGame;
  public UIButton btnPirateGame;
	
	public UILabel usernameLabel;
	public GameObject defaultAvatar;
	public UITexture avatarSprite;
	public UILabel coinLabel;
	public UILabel killLabel;
	public UILabel gemLabel;

  public UIButton btnFriend;
	public UIButton btnMail;
	public UIButton btnSetting;
	public UIButton btnBack;
	public UILabel dailyRewardCounterLabel;
	public GameObject mailNotice;

	private bool shouldUpdateDailyRewardTime = false;
	private int timeCounter = 0;
	private int currentCashDisplay = 0;
	private int currentGemDisplay = 0;
	private bool updatingCash = false;
	private bool updatingGem = false;
	// TO DO: update lastest user data from server

  public override void Init(object[] data) {
    EventDelegate.Set(btnZombieGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_ZOMBIE); });
    EventDelegate.Set(btnDragonGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_DRAGON); });
    EventDelegate.Set(btnPirateGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_PIRATE); });
    EventDelegate.Set(btnLeaderboard.onClick, EventOpenLeaderboard);
    
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    EventDelegate.Set(btnFriend.onClick, EventOpenFriendPopup);
    EventDelegate.Set(btnMail.onClick, EventOpenMailPopup);
    EventDelegate.Set(btnSetting.onClick, EventOpenSetting);
    EventDelegate.Set(btnDailyBonus.onClick, EventClaimDaily);
		
		usernameLabel.text = AccountManager.Instance.displayName;
		if (AccountManager.Instance.avatarLink != string.Empty) {
			NGUITools.SetActive(avatarSprite.gameObject, true);
			NGUITools.SetActive(defaultAvatar, false);
			StartCoroutine(DisplayAvatar());
		} else {
			NGUITools.SetActive(avatarSprite.gameObject, false);
			NGUITools.SetActive(defaultAvatar, true);
		}
		UpdateUserCashLabelFinished();
		UpdateUserGemLabelFinished();
		killLabel.text = AccountManager.Instance.bossKilled.ToString("N0");
		
		if (AccountManager.Instance.lastClaimedDaily == 0 || (long)(Utils.UTCNowMiliseconds() - AccountManager.Instance.lastClaimedDaily) >= Global.DAILY_REWARD_MILI) {
			EnableClaimDailyReward();
		} else {
			DisableClaimDailyReward();
		}
		ShowOrHideMailNotice();
  }

	public void ShowOrHideMailNotice() {
		if (AccountManager.Instance.lastInboxTime > AccountManager.Instance.lastReadInboxTime) {
			Utils.SetActive(mailNotice, true);
		} else {
			Utils.SetActive(mailNotice, false);
		}
	}

	IEnumerator DisplayAvatar() {
		WWW www = new WWW(AccountManager.Instance.avatarLink);
		yield return www;
		if (www.texture != null) {
			avatarSprite.mainTexture = www.texture;
		} else {
			NGUITools.SetActive(avatarSprite.gameObject, false);
			NGUITools.SetActive(defaultAvatar, true);
		}
		www.Dispose();
	}

	void Update() {
		if (shouldUpdateDailyRewardTime) {
			// TO DO: can be hack by change device time
			timeCounter = (int)((Global.DAILY_REWARD_MILI - (long)((DateTime.UtcNow - Utils.time1970).TotalMilliseconds - AccountManager.Instance.lastClaimedDaily)) / 1000);
			if (timeCounter <= 0) {
				EnableClaimDailyReward();
			} else {
				dailyRewardCounterLabel.text = Utils.GetTimeString(timeCounter);
			}
		}
		if (!SmartfoxClient.Instance.isRestarting && currentCashDisplay != AccountManager.Instance.cash && !updatingCash) {
			UpdateUserCashLabel();
		}
		if (!SmartfoxClient.Instance.isRestarting && currentGemDisplay != AccountManager.Instance.gem && !updatingGem) {
			UpdateUserGemLabel();
		}
	}

	void EnableClaimDailyReward() {
		btnDailyBonus.isEnabled = true;
		dailyRewardCounterLabel.text = Localization.Get("DailyReward_Claim_Now");
		dailyRewardCounterLabel.color = Color.green;
		shouldUpdateDailyRewardTime = false;
	}

	public void DisableClaimDailyReward() {
		btnDailyBonus.isEnabled = false;
		shouldUpdateDailyRewardTime = true;
		dailyRewardCounterLabel.color = Color.white;
	}

	public void ClaimedDailyRewardCallback() {
		// TO DO - display collect reward animation
		// UpdateUserCashLabel();
		DisableClaimDailyReward();
	}

  private void UpdateUserCashLabel() {
		if (currentCashDisplay == AccountManager.Instance.cash) {
			UpdateUserCashLabelFinished();
			return;
		}
		updatingCash = true;
		LeanTween.value(gameObject, UpdateUserCashLabelCallback, currentCashDisplay, AccountManager.Instance.cash, 1f).setOnComplete(UpdateUserCashLabelFinished);
  }
  
	void UpdateUserCashLabelCallback(float val) {
    coinLabel.text = Mathf.Floor(val).ToString("N0");
	}
	
	void UpdateUserCashLabelFinished() {
    coinLabel.text = AccountManager.Instance.cash.ToString("N0");
		currentCashDisplay = AccountManager.Instance.cash;
		updatingCash = false;
	}

  private void UpdateUserGemLabel() {
		if (currentGemDisplay == AccountManager.Instance.gem) {
			UpdateUserGemLabelFinished();
			return;
		}
		updatingGem = true;
		LeanTween.value(gameObject, UpdateUserGemLabelCallback, currentGemDisplay, AccountManager.Instance.gem, 1f).setOnComplete(UpdateUserGemLabelFinished);
  }
  
	void UpdateUserGemLabelCallback(float val) {
    gemLabel.text = Mathf.Floor(val).ToString("N0");
	}
	
	void UpdateUserGemLabelFinished() {
    gemLabel.text = AccountManager.Instance.gem.ToString("N0");
		currentGemDisplay = AccountManager.Instance.gem;
		updatingGem = false;
	}

  private void EventOpenLeaderboard() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LEADERBOARD, null, true, true);
  }

  private void EventOpenFriendPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_FRIENDS);
  }

  private void EventOpenMailPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_INBOX);
  }

  private void EventOpenSetting() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_SETTING);
  }

  private void EventOpenSlotGame(BaseSlotMachineScreen.GameType type) {
    SlotMachineClient.Instance.JoinRoom(type);
  }

	private void EventClaimDaily() {
    UserExtensionRequest.Instance.ClaimDailyReward();
	}

  void BackToLobbyScreen() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LOBBY);
  }

  public override void Close() {
		ScreenManager.Instance.SelectGameScreen = null;
		base.Close();
	}
}
