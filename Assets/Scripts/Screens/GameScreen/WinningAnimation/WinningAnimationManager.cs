using UnityEngine;
using System.Collections;

public class WinningAnimationManager : MonoBehaviour {

	[HideInInspector]
	public SlotMachine slotMachine;

	public virtual void Init(SlotMachine mSlotMachine) {
		slotMachine = mSlotMachine;
	}
	
	public virtual void Show() {}
}
