using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class SlotMachine : MonoBehaviour {

  public SlotReel[] slotReels;
  public SlotCombination slotCombination;
  public UILabel scoreLabel;
  public UILabel jackpotLabel;
	public MachineHandler machineHandler;
  
  private bool isRunning = false;
  private int reelStopCount = 0;
  private JSONArray resultsData;
  private JSONArray winningGold;
  private JSONArray winningCount;
  private JSONArray winningType;
  private JSONObject winResults;
  private bool isJackpot = false;
	public int freeSpinLeft = 0;
	private bool autoStart = false;
	// private bool canStart = true;
	private bool isBigWin = false;
	public bool gotFreeSpin = false;
	private List<SlotItem> mSlotItems = new List<SlotItem>();
	private List<SlotItem> slotItems {
		get {
			if (mSlotItems.Count == 0) {
		    for (int i = 0; i < slotReels.Length; i++) {
					for (int j = 0; j < slotReels[i].GetSlotItems().Count; j++) {
						mSlotItems.Add(slotReels[i].GetSlotItems()[j]);
					}
		    }
			}
			return mSlotItems; 
		}
	}
	
	private int pauseCount = 0;
	
  public void Init() {
    slotCombination.Init();
		// slotItems.Clear();
    for (int i = 0; i < slotReels.Length; i++) {
			// for (int j = 0; j < slotReels[i].GetSlotItems().Count; i++) {
			// 	slotItems.Add(slotReels[i].GetSlotItems()[j]);
			// }
      slotReels[i].onFinished = EventReelStop;
    }
  }

  public void StartMachine() {
    if (!isRunning && pauseCount == 0) {
      isRunning = true;
      string log = "";
      // for (int i = 0; i < SlotCombination.MAX_DISPLAY_ITEMS; i++) {
      //   // resultsData[i] = UnityEngine.Random.Range(0, (int)SlotItem.Type.TOTAL);
      //   // resultsData[i] = rand.Next((int)SlotItem.Type.TOTAL);
      //   resultsData[i] = slotCombination.RandomItem();
      //   log += resultsData[i] + ",";
      // }
      // Debug.Log("StartMachine " + log);
      for (int i = 0; i < slotReels.Length; i++) {
        slotReels[i].Reset();
        slotReels[i].StartMachine();
        // slotReels[i].SetResults(new int[3] { resultsData[i * 3], resultsData[i * 3 + 1], resultsData[i * 3 + 2]});
      }
      SlotMachineClient.Instance.Play(GetBetPerLine(), GetNumLine());
    }
    // Send request to server to get result
  }
  
  public void SetResults(JSONObject jsonData) {
    resultsData = jsonData.GetArray("items");
    winResults = jsonData.GetObject("winResults");
    winningGold = winResults.GetArray("wGold");
		// Calculate extra data (winning type, winning count from list result items)
		JSONObject extraData = SlotCombination.CalculateCombination(resultsData);
    winningCount = extraData.GetArray("wCount");
    winningType = extraData.GetArray("wType");
		//
    isJackpot = winResults.GetBoolean("isJP");
		freeSpinLeft = jsonData.GetInt("frLeft");
		isBigWin = jsonData.GetBoolean("bWin");
		gotFreeSpin = winResults.GetInt("frCount") > 0;
		// bool[] winingItems = new bool[15];
		// for (int i = 0; i < winningGold.Length; i++) {
		// 	if (winningGold[i].Number > 0) {
		// 		for (int j = 0; j < SlotCombination.NUM_REELS; j++) {
		// 			winingItems[SlotCombination.COMBINATION[i, j]] = true;
		// 		}
		// 	}
		// }
    for (int i = 0; i < slotReels.Length; i++) {
      slotReels[i].SetResults(new int[3] { (int)resultsData[i * 3].Number, (int)resultsData[i * 3 + 1].Number, (int)resultsData[i * 3 + 2].Number });
    }
  }
  
  private void EventReelStop() {
    reelStopCount++;
    if (reelStopCount >= slotReels.Length) {
      reelStopCount = 0;
      isRunning = false;
			
      int totalScore = 0;
      for (int i = 0; i < winningGold.Length; i++) {
        totalScore += (int)winningGold[i].Number;
      }
			if (isBigWin) {
				ScreenManager.Instance.CurrentSlotScreen.FadeInBigWin(totalScore);
			} else if (gotFreeSpin) {
				ScreenManager.Instance.CurrentSlotScreen.FadeInFreeSpin(freeSpinLeft);
			} else {
	      UpdateScore(totalScore);
			}
			
			// Glow winning item
			for (int i = 0; i < winningCount.Length; i++) {
				if (winningCount[i].Number >= 3) {
					for (int j = 0; j < SlotCombination.NUM_REELS; j++) {
						slotItems[SlotCombination.COMBINATION[i, j]].Glow();
					}
					ScreenManager.Instance.CurrentSlotScreen.AddSkillToQueue(new SpawnableSkill((int)winningType[i].Number, (int)winningCount[i].Number, (int)winningGold[i].Number, true));
				}
			}
			if (freeSpinLeft > 0) {
				machineHandler.DisableHandler();
				Invoke("EnableAutoStart", 0.5f);
			} else {
				DisableAutoStart();
				machineHandler.EnableHandler();
			}
    }
  }
  
	public void Wait() {
		// canStart = false;
		pauseCount++;
	}
	
	public void Resume() {		
		// canStart = true;
		pauseCount = Mathf.Max(0, pauseCount - 1);
	}
	
	void EnableAutoStart() {
		autoStart = true;
	}
	
	void DisableAutoStart() {
		autoStart = false;
	}
	
	void Update() {
		// TEST CODE -- should refined - auto start in free spin
		if (autoStart) {
			// if (autoStart) {
				StartMachine();
			// }
		}
	}
	
  public void UpdateJackpot(int score) {
    jackpotLabel.text = score.ToString("N0");
  }
  
  public void UpdateScore(int score) {
    scoreLabel.text = score.ToString("N0");
    // AccountManager.Instance.UpdateUserCash(score);
    ScreenManager.Instance.CurrentSlotScreen.UpdateUserCashLabel(score);
    // ScreenManager.Instance.CurrentSlotScreen.EventFinishSpin(isBigWin, score);
  }
  
  public void SetBetPerLine(int betVal) {
    slotCombination.SetBetPerLine(betVal);
  }
  
  public void SetNumLine(int lineVal) {
    slotCombination.SetNumLines(lineVal);
  }
  
  public int GetNumLine() {
    return slotCombination.numLines;
  }
  
  public int GetBetPerLine() {
    return slotCombination.betPerLine;
  }
}
