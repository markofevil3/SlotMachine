using UnityEngine;
using System.Collections;

public class AtlasManager : MonoBehaviour {
  
  public static AtlasManager Instance { get; private set; }
  
  public UIAtlas slotMachinePirateReference;
  public UIAtlas slotMachineZombieReference;
  public UIAtlas slotMachineDragonReference;
  
  private string uiAtlasName;
  private string theme1UI;
  private string theme2UI;
  private string theme3UI;
  private string theme4UI;

  void Awake() {
    Instance = this;
  }

  public void Init() {
			//     if (Utils.IsUHD()) {
			//       uiAtlasName = "UHD";
			// theme1UI = "ThemeUHD";
			// theme2UI = "Theme2UHD";
			// theme3UI = "Theme3UHD";
			// theme4UI = "Theme4UHD";
			//     } else {
			//       if (Utils.IsSD()) {
			//         uiAtlasName = "HD";
			//   			theme1UI = "ThemeSD";
			//   			theme2UI = "Theme2SD";
			//   			theme3UI = "Theme3SD";
			//   			theme4UI = "Theme4SD";
			//       } else {
			//         uiAtlasName = "HD";
			//   			theme1UI = "ThemeHD";
			//   			theme2UI = "Theme2HD";
			//   			theme3UI = "Theme3HD";
			//   			theme4UI = "Theme4HD";
			//       }
			//     }
			//     uiRef.replacement = (Resources.Load("Atlas/" + uiAtlasName + "/" + uiAtlasName, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
			//     theme1UIRef.replacement = (Resources.Load("Atlas/" + theme1UI + "/" + theme1UI, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
			//     theme2UIRef.replacement = (Resources.Load("Atlas/" + theme2UI + "/" + theme2UI, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
			//     theme3UIRef.replacement = (Resources.Load("Atlas/" + theme3UI + "/" + theme3UI, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
    // theme4UIRef.replacement = (Resources.Load("Atlas/" + theme4UI + "/" + theme4UI, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
  }
	
	public UIAtlas GetAtlasReferenceBySlotType(BaseSlotMachineScreen.GameType slotType) {
		switch(slotType) {
			case BaseSlotMachineScreen.GameType.SLOT_DRAGON:
				return slotMachineDragonReference;
			break;
			case BaseSlotMachineScreen.GameType.SLOT_PIRATE:
				return slotMachinePirateReference;
			break;
			case BaseSlotMachineScreen.GameType.SLOT_ZOMBIE:
				return slotMachineZombieReference;
			break;
		}
		return null;
	}
}
