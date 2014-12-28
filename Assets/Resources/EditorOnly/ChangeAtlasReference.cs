using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class ChangeAtlasReference : MonoBehaviour {

	public UIAtlas fromAtlas;
	public UIAtlas toAtlas;
	public bool startNow = false;
	
	void LateUpdate() {
		if (startNow) {
			ChageAtlas();
			startNow = false;
		}
	}
	
	void ChageAtlas() {
		UISprite[] sprites = gameObject.GetComponentsInChildren<UISprite>();
		foreach (UISprite sprite in sprites) {
			if (sprite.atlas.gameObject.name == fromAtlas.gameObject.name) {
				sprite.atlas = toAtlas;
			}
			// label.font = ((GameObject)Resources.Load("Fonts/FontReference")).GetComponent<UIFont>();
		}
	}
}