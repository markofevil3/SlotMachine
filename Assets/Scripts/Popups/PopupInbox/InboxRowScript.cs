using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class InboxRowScript : MonoBehaviour {
	
  [HideInInspector]
  public JSONObject rowData;
  
  public UILabel titleLabel;
	public UISprite iconSprite;
  public UIDragScrollView dragScrollView;

  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateRowData(JSONObject data) {
    rowData = data;
  }
}
