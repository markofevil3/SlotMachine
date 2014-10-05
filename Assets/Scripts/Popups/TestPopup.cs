using UnityEngine;
using System.Collections;

public class TestPopup : Popup {
	
	public UIButton btnClosePopup;
	
	public override void Init() {
		base.Init();
		EventDelegate.Set(btnClosePopup.onClick, Close);
	}
	
}
