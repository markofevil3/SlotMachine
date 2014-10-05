using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

  public static AudioManager Instance { get; private set; }
  
  public bool soundOn = true;
  
	// Use this for initialization
	void Awake () {
	  Instance = this;
    if (PlayerPrefs.HasKey("sound")) {
      soundOn = bool.Parse(PlayerPrefs.GetString("sound"));
    } else {
      PlayerPrefs.SetString("sound", soundOn.ToString());
    }
    AudioListener.volume = soundOn ? 1.0f : 0;
	}
  
  public bool SwitchSound() {
    if (soundOn) {
      AudioListener.volume  = 0;
      soundOn = false;
    } else {
      AudioListener.volume  = 1.0f;
      soundOn = true;
    }
    PlayerPrefs.SetString("sound", soundOn.ToString());
    return soundOn;
  }
  
  public bool IsSoundOn() {
    return soundOn;
  }
  
}
