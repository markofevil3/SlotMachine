using UnityEngine;
using System.Collections;

public class SlotMachine : MonoBehaviour {

  public SlotReel[] slotReels;
  public SlotCombination slotCombination;
  public UILabel scoreLabel;
  
  private bool isRunning = false;
  private int reelStopCount = 0;
  private int[] resultsData = new int[15];

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
      for (int i = 0; i < SlotCombination.MAX_DISPLAY_ITEMS; i++) {
        // resultsData[i] = UnityEngine.Random.Range(0, (int)SlotItem.Type.TOTAL);
        // resultsData[i] = rand.Next((int)SlotItem.Type.TOTAL);
        resultsData[i] = slotCombination.RandomItem();
        log += resultsData[i] + ",";
      }
      Debug.Log("StartMachine " + log);
      for (int i = 0; i < slotReels.Length; i++) {
        slotReels[i].Reset();
        slotReels[i].StartMachine();
        slotReels[i].SetResults(new int[3] { resultsData[i * 3], resultsData[i * 3 + 1], resultsData[i * 3 + 2]});
      }
    }
    // Send request to server to get result
  }
  
  public void EventReelStop() {
    reelStopCount++;
    if (reelStopCount >= slotReels.Length) {
      reelStopCount = 0;
      isRunning = false;
      int[] scoreArr = slotCombination.CalculateCombination(resultsData);
      int totalScore = 0;
      for (int i = 0; i < scoreArr.Length; i++) {
        totalScore += scoreArr[i];
      }
      UpdateScore(totalScore);
    }
  }
  
  public void UpdateScore(int score) {
    scoreLabel.text = score.ToString("N0");
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
