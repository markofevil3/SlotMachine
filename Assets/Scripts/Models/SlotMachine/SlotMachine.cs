using UnityEngine;
using System.Collections;
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
  private JSONObject winResults;
  private bool isJackpot = false;
	private int freeSpinLeft = 0;

  public void Init() {
    slotCombination.Init();
    for (int i = 0; i < slotReels.Length; i++) {
      slotReels[i].onFinished = EventReelStop;
    }
  }

  public void StartMachine() {
    if (!isRunning) {
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
    winningGold = winResults.GetArray("winningGold");
    isJackpot = winResults.GetBoolean("isJackpot");
		freeSpinLeft = jsonData.GetInt("freeSpinLeft");
    for (int i = 0; i < slotReels.Length; i++) {
      slotReels[i].SetResults(new int[3] { (int)resultsData[i * 3].Number, (int)resultsData[i * 3 + 1].Number, (int)resultsData[i * 3 + 2].Number });
    }
  }
  
  public void EventReelStop() {
    reelStopCount++;
    if (reelStopCount >= slotReels.Length) {
      reelStopCount = 0;
      isRunning = false;
      // int[] scoreArr = slotCombination.CalculateCombination(resultsData);
      int totalScore = 0;
      for (int i = 0; i < winningGold.Length; i++) {
        totalScore += (int)winningGold[i].Number;
      }
      UpdateScore(totalScore);
			if (freeSpinLeft > 0) {
				machineHandler.DisableHandler();
				Invoke("StartMachine", 0.5f);
			} else {
				machineHandler.EnableHandler();
			}
    }
  }
  
  public void UpdateJackpot(int score) {
    jackpotLabel.text = score.ToString("N0");
  }
  
  public void UpdateScore(int score) {
    scoreLabel.text = score.ToString("N0");
    AccountManager.Instance.UpdateUserCash(score);
    ScreenManager.Instance.CurrentSlotScreen.UpdateUserCashLabel();
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
