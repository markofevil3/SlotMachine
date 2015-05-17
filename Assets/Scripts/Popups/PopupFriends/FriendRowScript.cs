using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class FriendRowScript : MonoBehaviour {

  [HideInInspector]
  public JSONObject rowData;
  
  public UILabel usernameLabel;
  public UILabel cashLabel;
  public UILabel currentGameLabel;
  public UIButton btnSendGift;
  public UITexture avatarTexture;
	public GameObject defaultAvatar;
  public UIDragScrollView dragScrollView;

	private string facebookID;

  public void Init(UIScrollView scrollview) {
    dragScrollView.scrollView = scrollview;
  }
  
  public void UpdateRowData(JSONObject data) {
		if (rowData != null && data.GetString("username") == rowData.GetString("username")) {
			return;
		}
    rowData = data;
		facebookID = rowData.GetString("facebookId");
    usernameLabel.text = data.GetString("displayName");
    cashLabel.text = data.GetInt("cash").ToString("N0");
		if (facebookID != null && facebookID != string.Empty) {
			NGUITools.SetActive(avatarTexture.gameObject, true);
			StartCoroutine(DisplayAvatar(facebookID));
		} else {
			NGUITools.SetActive(defaultAvatar, true);
			NGUITools.SetActive(avatarTexture.gameObject, false);
		}
  }

	IEnumerator DisplayAvatar(string fbId) {
		// TO DO: test when have multiple row
		WWW www = new WWW(rowData.GetString("avatar"));
		yield return www;
		if (www.texture != null) {
			if (fbId == rowData.GetString("facebookId")) {
				avatarTexture.mainTexture = www.texture;
				NGUITools.SetActive(defaultAvatar, false);
			}
		}
		www.Dispose();
	}
}
