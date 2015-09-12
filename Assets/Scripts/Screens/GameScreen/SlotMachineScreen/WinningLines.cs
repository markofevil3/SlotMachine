using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinningLines : MonoBehaviour {

	public GameObject[] lines;
	
	private List<int> winningLineIndexs;
	private int lastBlinkIndex = 0;
	
	public void Show(List<int> winningLineIndexs) {
		this.winningLineIndexs = winningLineIndexs;
		lastBlinkIndex = 0;
		InvokeRepeating("Blink", 0, 1f);
	}

	void Blink() {
		NGUITools.SetActiveSelf(lines[winningLineIndexs[lastBlinkIndex]], false);
		lastBlinkIndex = (lastBlinkIndex + 1) % winningLineIndexs.Count;
		StartCoroutine("Blink2");
	}

	IEnumerator Blink2() {
		yield return new WaitForSeconds(0.5f);
		NGUITools.SetActiveSelf(lines[winningLineIndexs[lastBlinkIndex]], true);
	}

	public void Hide() {
		CancelInvoke("Blink");
		StopCoroutine("Blink2");
		for (int i = 0; i < lines.Length; i++) {
			NGUITools.SetActiveSelf(lines[i], false);
		}
	}
}
