using UnityEngine;
using System.Collections;

public class MachineHandler : MonoBehaviour {
  
  public UISlider bodySlider;
  public UIEventListener btnDrag;
  public SlotMachine slotMachine;
  
  void Start() {
    btnDrag.onDragEnd = FinishDrag;
  }
  
  void FinishDrag(GameObject go) {
    if (bodySlider.value < 1) {
  		LeanTween.value(bodySlider.gameObject, DoUpdateSliderValue, bodySlider.value, 1, Mathf.Max(0.05f, (1 - bodySlider.value) * 0.3f));
    }
    if (bodySlider.value < 0.6f) {
      // Start slider
      slotMachine.StartMachine();
    }
  }
  
  void DoUpdateSliderValue(float updateVal) {
		bodySlider.value = updateVal;
	}
  
}