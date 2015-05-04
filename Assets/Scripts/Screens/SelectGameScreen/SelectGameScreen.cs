using UnityEngine;
using System;
using System.Collections;

public class SelectGameScreen : BaseScreen {

  private string GAME_ITEM_PREFAB = Global.SCREEN_PATH + "/SelectGameScreen/GameItem";

  public UIButton btnLeaderboard;
  public UIButton btnDailyBonus;
  public UIButton btnZombieGame;
  public UIButton btnDragonGame;
  public UIButton btnPirateGame;
	
	public UILabel usernameLabel;
	public UILabel coinLabel;
	public UILabel goldLabel;

  public UIButton btnFriend;
	public UIButton btnMail;
	public UIButton btnSetting;
	public UIButton btnBack;
	public UILabel dailyRewardCounterLabel;

	private bool shouldUpdateDailyRewardTime = false;
	private int timeCounter = 0;

  public override void Init(object[] data) {
    EventDelegate.Set(btnZombieGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_HALLOWEEN); });
    EventDelegate.Set(btnDragonGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_DRAGON); });
    EventDelegate.Set(btnPirateGame.onClick, delegate() { EventOpenSlotGame(BaseSlotMachineScreen.GameType.SLOT_PIRATE); });
    EventDelegate.Set(btnLeaderboard.onClick, EventOpenLeaderboard);
    
    EventDelegate.Set(btnBack.onClick, BackToLobbyScreen);
    EventDelegate.Set(btnFriend.onClick, EventOpenFriendPopup);
    EventDelegate.Set(btnMail.onClick, EventOpenMailPopup);
    EventDelegate.Set(btnSetting.onClick, EventOpenSetting);
    EventDelegate.Set(btnDailyBonus.onClick, EventClaimDaily);
		
		usernameLabel.text = AccountManager.Instance.username;
		coinLabel.text = AccountManager.Instance.cash.ToString("N0");
		
		if (AccountManager.Instance.lastClaimedDaily == 0 || (long)((DateTime.UtcNow - Utils.time1970).TotalMilliseconds - AccountManager.Instance.lastClaimedDaily) >= Global.DAILY_REWARD_MILI) {
			Debug.Log("### " + AccountManager.Instance.lastClaimedDaily + " " + (long)((DateTime.UtcNow - Utils.time1970).TotalMilliseconds - AccountManager.Instance.lastClaimedDaily));
			EnableClaimDailyReward();
		} else {
			DisableClaimDailyReward();
		}
  }

	void Update() {
		if (shouldUpdateDailyRewardTime) {
			timeCounter = (int)((Global.DAILY_REWARD_MILI - (long)((DateTime.UtcNow - Utils.time1970).TotalMilliseconds - AccountManager.Instance.lastClaimedDaily)) / 1000);
			if (timeCounter <= 0) {
				EnableClaimDailyReward();
			} else {
				dailyRewardCounterLabel.text = Utils.GetTimeString(timeCounter);
			}
		}
	}

	void EnableClaimDailyReward() {
		dailyRewardCounterLabel.text = Localization.Get("DailyReward_Claim_Now");
		dailyRewardCounterLabel.color = Color.green;
		shouldUpdateDailyRewardTime = false;
	}

	public void DisableClaimDailyReward() {
		shouldUpdateDailyRewardTime = true;
		dailyRewardCounterLabel.color = Color.white;
	}

	public void ClaimedDailyRewardCallback(int userOldCash, int cashReward) {
		// TO DO - display collect reward animation
		ScreenManager.Instance.SelectGameScreen.UpdateUserCashLabel(userOldCash, cashReward);
		DisableClaimDailyReward();
	}

  public void UpdateUserCashLabel(int fromVal, int addValue) {
		if (addValue == 0) {
			UpdateUserCashLabelFinished();
			return;
		}
		LeanTween.value(gameObject, UpdateUserCashLabelCallback, fromVal, AccountManager.Instance.cash, 1f).setOnComplete(UpdateUserCashLabelFinished);
  }
  
	void UpdateUserCashLabelCallback(float val) {
    coinLabel.text = Mathf.Floor(val).ToString("N0");
	}
	
	void UpdateUserCashLabelFinished() {
    coinLabel.text = AccountManager.Instance.cash.ToString("N0");
	}

  private void EventOpenLeaderboard() {
    ScreenManager.Instance.SetScreen(BaseScreen.Type.LEADERBOARD, null, true, true);
  }

  private void EventOpenFriendPopup() {
    PopupManager.Instance.OpenPopup(Popup.Type.POPUP_FRIENDS);
  }

  private void EventOpenMailPopup() {
    // PopupManager.Instance.OpenPopup(Popup.Type.POPUP_FRIENDS);
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
