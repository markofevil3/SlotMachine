using UnityEngine;
using System;
using System.Collections;
using Boomlagoon.JSON;

public class LeaderboardScreen : BaseScreen {

  private const int RELOAD_DATA_SECONDS = 300;
  private const int STOP_DRAG_NUMB_ROW = 7;

  public enum Tab {
    NULL = -1,
		TOP_RICHER,
		TOP_WINNER
  }

  private static JSONArray topRicherList;
  private static JSONArray topWinnerList;
  private static DateTime? topRicherLoadTime = null;
  private static DateTime? topWinnerLoadTime = null;

  public UIButton btnBack;
  public UIWrapContent wrapContent;
  public UIScrollView scrollview;
  public UIButton[] tabBars = new UIButton[2];
  public UIDragScrollView backgroundDragScrollView;
	public UIWidget wrapperAnchor;
	
  private Tab currentTab = Tab.NULL;
  private bool isInitingRow = false;
  private bool isLoading = false;

  
  public override void Init(object[] data) {
    base.Init(data);
    EventDelegate.Set(btnBack.onClick, EventBackToPrevScreen);
    EventDelegate.Set(tabBars[0].onClick, delegate() { ShowTopPlayer(Tab.TOP_RICHER); });
   	EventDelegate.Set(tabBars[1].onClick, delegate() { ShowTopPlayer(Tab.TOP_WINNER); });
    ShowTopPlayer(Tab.TOP_RICHER);
  }

  private bool ShouldLoadData(Tab selectedTab) {
    JSONArray checkData = new JSONArray();
    DateTime? checkTime = null;
    switch(selectedTab) {
      case Tab.TOP_RICHER:
        checkData = topRicherList;
        checkTime = topRicherLoadTime;
      break;
      case Tab.TOP_WINNER:
        checkData = topWinnerList;
        checkTime = topWinnerLoadTime;
      break;
      default:
      return false;
    }
    
    if (checkData == null || checkData.Length == 0) {
      return true;
    } else {
      if (checkTime.HasValue) {
        if (Utils.CurrentTime().Subtract((DateTime)checkTime).TotalSeconds >= RELOAD_DATA_SECONDS) {
          return true;
        } else {
          return false;
        }
      } else {
        return true;
      }
    }
  }

  private void LoadDataFromServer(Tab selectedTab) {
    PopupManager.Instance.ShowLoadingPopup();
    isLoading = true;
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
			Utils.SetActive(wrapContent.transform.GetChild(i).gameObject, false);
    }
    UserExtensionRequest.Instance.LoadLeaderboardData(selectedTab);
  }

  public static void SetData(JSONArray users, Tab selectedTab) {
    switch(selectedTab) {
      case Tab.TOP_RICHER:
        topRicherList = users;
        topRicherLoadTime = Utils.CurrentTime();
      break;
      case Tab.TOP_WINNER:
        topWinnerList = users;
        topWinnerLoadTime = Utils.CurrentTime();
      break;
    }
  }

  public void ShowTopPlayer(Tab selectedTab) {
    if (currentTab == selectedTab && !isLoading) {
			return;
		}
		currentTab = selectedTab;
		for (int i = 0; i < tabBars.Length; i++) {
			if (i == (int)selectedTab) {
				tabBars[i].isEnabled = false;
			} else {
				tabBars[i].isEnabled = true;
			}
		}
		
		if (ShouldLoadData(selectedTab)) {
		  LoadDataFromServer(selectedTab);
		} else {
		  InitScrollViewData(selectedTab);
		}
  }

  private void InitScrollViewData(Tab selectedTab) {
    isLoading = false;
    wrapContent.ResetChildPositions();
    scrollview.currentMomentum = Vector3.zero;
    scrollview.ResetPosition();
    Transform tempGameObject;
    JSONArray targetList = null;
    
    switch(selectedTab) {
      case Tab.TOP_RICHER:
        targetList = topRicherList;
      break;
      case Tab.TOP_WINNER:
        targetList = topWinnerList;
      break;
    }
    
    wrapContent.minIndex = -(targetList.Length - 1);
    wrapContent.onInitializeItem = UpdateRowDataOnScroll;
    bool canDrag = true;
    if (targetList.Length <= STOP_DRAG_NUMB_ROW) {
      canDrag = false;
      backgroundDragScrollView.enabled = false;
    } else {
      backgroundDragScrollView.enabled = true;
    }
    for (int i = 0; i < wrapContent.transform.childCount; i++) {
      tempGameObject = wrapContent.transform.GetChild(i);
      if (!tempGameObject.gameObject.activeSelf) {
				Utils.SetActive(tempGameObject.gameObject, true);
      }
      TopPlayerRowScript tempRowScript = tempGameObject.GetComponent<TopPlayerRowScript>();
      tempRowScript.Init(scrollview);
      
      if (canDrag) {
        tempRowScript.dragScrollView.enabled = true;
      } else {
        tempRowScript.dragScrollView.enabled = false;
      }
      if (i < targetList.Length) {
				Utils.SetActive(tempGameObject.gameObject, true);
        tempRowScript.UpdateRowData(targetList[i].Obj, currentTab);
      } else {
				Utils.SetActive(tempGameObject.gameObject, false);
      }
    }
		wrapperAnchor.ResetAndUpdateAnchors();
  }

  float lastValue = 0;
  float finalOffset = 0;

  void Test() {
    // Can hard-code: get offset of scroll view at the last item
    // 14 is the gap of top and bottom are trim
    finalOffset = topRicherList.Length * wrapContent.itemSize - scrollview.panel.height - 14;
    LeanTween.value(gameObject, UpdateValue, 0, 13000f, 0.5f);
  }

  void UpdateValue(float momentum) {
    if (momentum > finalOffset) {
      scrollview.MoveRelative(new Vector3(0, finalOffset - lastValue, 0));
      LeanTween.cancel(gameObject);
      return;
    }

	  scrollview.MoveRelative(new Vector3(0, momentum - lastValue, 0));
    lastValue = momentum;
  }

  public void UpdateRowDataOnScroll(GameObject go, int wrapIndex, int realIndex) {
    realIndex = (int)Mathf.Abs((float)realIndex);
    if (currentTab == Tab.TOP_RICHER) {
      if (realIndex < topRicherList.Length) {
        go.GetComponent<TopPlayerRowScript>().UpdateRowData(topRicherList[realIndex].Obj, currentTab);
      }
    } else {
      if (realIndex < topWinnerList.Length) {
        go.GetComponent<TopPlayerRowScript>().UpdateRowData(topWinnerList[realIndex].Obj, currentTab);
      }
    }
  }

  private void EventBackToPrevScreen() {
    ScreenManager.Instance.BackToPrevScreen();
  }

	public override void DestroyCallBack() {
		topRicherLoadTime = null;
		topWinnerLoadTime = null;
	}

  public override void Close() {
		ScreenManager.Instance.LeaderboardScreen = null;
		base.Close();
	}
}
