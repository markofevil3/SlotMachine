using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TopPlayerRowScript : MonoBehaviour {
  
  [HideInInspector]
  public JSONObject rowData;
  
  public UILabel playerNameLabel;
  public UILabel cashLabel;
  public UISprite rankIcon;
  public GameObject rankBackground;
  public UILabel rankLabel;
  public UIEventTriggerExtent eventTrigger;
  public UIDragScrollView dragScrollView;
  public UISprite background;
  
  private int rank;
  
  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateRowData(JSONObject data, LeaderboardScreen.Tab selectedTab) {
    rowData = data;
    
    if (AccountManager.Instance.IsYou(rowData.GetString("username"))) {
      background.spriteName = "MainPanel_Subpanel_dark";
    } else {
      background.spriteName = "MainPanel_Subpanel";
    }
    
    playerNameLabel.text = rowData.GetString("displayName");
    if (selectedTab == LeaderboardScreen.Tab.TOP_RICHER) {
      cashLabel.text = rowData.GetInt("cash").ToString("N0") + "$";
    } else {
      // cashLabel.text = Utils.Localize("Top_Winner_Match_Text", new string[1] {rowData.GetInt("winMatchNumb").ToString("N0")});
      cashLabel.text = Localization.Format("Top_Winner_Match_Text", rowData.GetInt("winMatchNumb").ToString("N0"));
    }
    rank = rowData.GetInt("rank");
    if (rank <= 3) {
      rankBackground.SetActive(false);
      rankIcon.gameObject.SetActive(true);
      rankIcon.spriteName = "Chat_RankIcon0" + rank;
    } else {
      rankBackground.SetActive(true);
      rankIcon.gameObject.SetActive(false);
      rankLabel.text = rank.ToString();
    }
    eventTrigger.inputParams = new object[] {rowData.GetString("username")};
    EventDelegate.Set(eventTrigger.onClick, delegate() { EventShowUserInfo((string)eventTrigger.inputParams[0]); });
  }

  void EventShowUserInfo(string username) {
    if (!AccountManager.Instance.IsYou(username)) {
      PopupManager.Instance.OpenPopup(Popup.Type.POPUP_USER_INFO, new object[] { username });
    }
  }
  
  // public void UpdateRicherData(JSONObject data) {
  //   rowData = data;
  //   playerNameLabel.text = data.GetString("displayName");
  //   cashLabel.text = data.GetInt("cash").ToString("N0") + "$";
  //   rank = data.GetInt("rank");
  //   if (rank <= 3) {
  //     rankBackground.SetActive(false);
  //     rankIcon.gameObject.SetActive(true);
  //     rankIcon.spriteName = "Chat_RankIcon0" + rank;
  //   } else {
  //     rankBackground.SetActive(true);
  //     rankIcon.gameObject.SetActive(false);
  //     rankLabel.text = rank.ToString();
  //   }
  // }
  // 
  // public void UpdateWinnerData(JSONObject data) {
  //   rowData = data;
  //   playerNameLabel.text = data.GetString("displayName");
  //   cashLabel.text = Utils.Localize("Top_Winner_Match_Text", new string[1] {data.GetInt("winMatchNumb").ToString("N0")});
  //   rank = data.GetInt("rank");
  //   if (rank <= 3) {
  //     rankBackground.SetActive(false);
  //     rankIcon.gameObject.SetActive(true);
  //     rankIcon.spriteName = "Chat_RankIcon0" + rank;
  //   } else {
  //     rankBackground.SetActive(true);
  //     rankIcon.gameObject.SetActive(false);
  //     rankLabel.text = rank.ToString();
  //   }
  // }
}
