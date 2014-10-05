using UnityEngine;
using System.Collections;

public class FramesPerSecond : MonoBehaviour {
	float updateInterval = 0.5f;	
	private float accum = 0.0f; // FPS accumulated over the interval
	private float frames = 0.0f; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private UILabel txt;
	
	void Start() {
		timeleft = updateInterval; 
		txt = gameObject.GetComponent<UILabel>();
  }
	
	void Update() {
		timeleft -= Time.smoothDeltaTime;
		accum += Time.timeScale/Time.smoothDeltaTime;
		++frames;		
		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0f) {
			// display two fractional digits (f2 format)
			txt.text = "" + (accum / frames).ToString("f2");
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0.0f;
		}
	}
}