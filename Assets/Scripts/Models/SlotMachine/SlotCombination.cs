using UnityEngine;
using System;
using System.Collections;
using Boomlagoon.JSON;

// #### Slot item index
// 0 3 6 9 12
// 1 4 7 10 13
// 2 5 8 11 14

public class SlotCombination : MonoBehaviour {
  
  public static int MAX_LINE = 9;
  public static int MAX_BET_PER_LINE_RANGER = 5;
  public static int NUM_REELS = 5;
  public static int MAX_DISPLAY_ITEMS = 15;
  
  // private float[] ITEM_RATES = new float[10] {4f, 20f, 12f, 11f, 10f, 10f, 10f, 9f, 8f, 6f};
  
  private float randomValue;
  
  public static int[,] COMBINATION = new int[,] {
    // line 1
    { 1, 4, 7, 10, 13 },
    // line 2
    { 0, 3, 6, 9, 12 },
    // line 3
    { 2, 5, 8, 11, 14 },
    // line 4
    { 0, 4, 8, 10, 12 },
    // line 5
    { 2, 4, 6, 10, 14 },
    // line 6
    { 0, 3, 7, 11, 14 },
    // line 7
    { 2, 5, 7, 9, 12 },
    // line 8
    { 1, 5, 7, 9, 13 },
    // line 9
    { 1, 3, 7, 11, 13 },
  };
  
  // public static int[,] PAYOUTS = new int[,] {
  //   // item 0 - ignore
  //   { 0, 0, 0, 0, 0 },
  //   // item 1
  //   { 0, 1, 3, 10, 85 },
  //   // item 2
  //   { 0, 0, 15, 30, 150 },
  //   // item 3
  //   { 0, 0, 20, 40, 250 },
  //   // item 4
  //   { 0, 0, 25, 50, 400 },
  //   // item 5
  //   { 0, 0, 30, 70, 550 },
  //   // item 6
  //   { 0, 0, 35, 80, 650 },
  //   // item 7
  //   { 0, 0, 45, 100, 800 },
  //   // item 8
  //   { 0, 0, 75, 175, 1250 },
  //   // item 9
  //   { 0, 0, 100, 200, 1750 }
  // };
  
  // default value - should be set by user
  private int mBetPerLine = 10;
  private int mNumLines = 9;
	private int mBetPerLineIndex = 0;
	private int[] betPerLines = new int[5];
  
	public int betPerLineIndex {
    get { return mBetPerLineIndex; }
    set { mBetPerLineIndex = value; }
	}
	
  public int betPerLine {
    get { return mBetPerLine; }
    set { mBetPerLine = value; }
  }
  
  public int numLines {
    get { return mNumLines; }
    set { mNumLines = value; }
  }
  
  public void Init(string betPerLinesData) {
		Utils.StringToIntArray(betPerLinesData, betPerLines);
		Debug.Log("####### " + Utils.ArrIntToString(betPerLines));
  }
  
  public void SetBetPerLine(int betVal) {
		if (betVal >= betPerLines.Length) {
			betVal = betPerLines.Length - 1;
		}
		betPerLineIndex = betVal;
    betPerLine = betPerLines[betVal];
  }
  
  public void SetNumLines(int lineVal) {
    numLines = Math.Min(lineVal, MAX_LINE);
  }
    
  // public int RandomItem() {
  //   float cap = 0;
  //   randomValue = UnityEngine.Random.value * 100;
  //   for (int i = 0; i < ITEM_RATES.Length; i++) {
  //     if (randomValue <= cap + ITEM_RATES[i]) {
  //       return i;
  //     } else {
  //       cap += ITEM_RATES[i];
  //     }
  //   }
  //   return 1;
  // }
  
  // // input data is array type of 15 items - output data is array winning gold of 9 lines
  // public int[] CalculateCombination(int[] reelData) {
  //   int[] winningLineCount = new int[numLines];
  //   int[] winningLineType = new int[numLines];
  //   int[] winningGold = new int[numLines];
  //   for (int i = 0; i < numLines; i++) {
  //     for (int j = 0; j < NUM_REELS - 1; j++) {
  //       if (j == 0 && reelData[COMBINATION[i, j]] != (int)SlotItem.Type.WILD) {
  //         winningLineCount[i]++;
  //         winningLineType[i] = reelData[COMBINATION[i, j]];
  //         continue;
  //       }
  //       if (reelData[COMBINATION[i, j]] == (int)SlotItem.Type.WILD) {
  //         winningLineCount[i]++;
  //       } else {
  //         if (winningLineType[i] == 0) {
  //           winningLineCount[i]++;
  //           winningLineType[i] = reelData[COMBINATION[i, j]];
  //           continue;
  //         } else if (reelData[COMBINATION[i, j]] == winningLineType[i]){
  //           winningLineCount[i]++;
  //           continue;
  //         } else {
  //           break;
  //         }
  //       }
  //     }
  //     winningGold[i] = PAYOUTS[winningLineType[i], winningLineCount[i] - 1] * betPerLine;
  //   }
  //   Debug.Log(Utils.ArrIntToString(winningLineCount));
  //   Debug.Log(Utils.ArrIntToString(winningLineType));
  //   Debug.Log(Utils.ArrIntToString(winningGold));
  //   return winningGold;
  // }
	
  public static JSONObject CalculateCombination(JSONArray reelData, int numLine) {
  	JSONObject results = new JSONObject();
    int[] winningLineCount = new int[numLine];
    int[] winningLineType = new int[numLine];
    for (int i = 0; i < numLine; i++) {
      for (int j = 0; j < NUM_REELS - 1; j++) {
        if (j == 0 && (int)reelData[COMBINATION[i, j]].Number != (int)SlotItem.Type.WILD) {
          winningLineCount[i]++;
          winningLineType[i] = (int)reelData[COMBINATION[i, j]].Number;
          continue;
        }
        if ((int)reelData[COMBINATION[i,j]].Number == (int)SlotItem.Type.WILD) {
          winningLineCount[i]++;
        } else {
          if (winningLineType[i] == 0) {
            winningLineCount[i]++;
            winningLineType[i] = (int)reelData[COMBINATION[i,j]].Number;
            continue;
          } else if ((int)reelData[COMBINATION[i,j]].Number == winningLineType[i]){
            winningLineCount[i]++;
            continue;
          } else {
            break;
          }
        }
      }
    }
		results.Add("wCount", new JSONArray(winningLineCount));
		results.Add("wType", new JSONArray(winningLineType));
    return results;
  }
}
