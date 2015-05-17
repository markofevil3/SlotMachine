using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class UIDraggableCameraExtent : UIDraggableCamera {
  
  //   public enum MoveDirection {
  //     NULL,
  //     UP,
  //     DOWN,
  //  LEFT,
  //  RIGHT,
  // }
  //   
  //   public MoveDirection direction = MoveDirection.NULL;
  //   
  //   //   public void Reset() {
  //   //  if (sp != null) sp.enabled = false;
  //   //  currentMomentum = Vector2.zero;
  //   //  transform.localPosition = startLocalPos;
  //   // }
  // 
  // void LateUpdate() {
  //     Utils.Log("LateUpdate " + direction + " " + currentMomentum);
  //   if (currentMomentum == Vector2.zero) {
  //     direction = MoveDirection.NULL;
  //     return;
  //   }
  //   if (currentMomentum.y < 0) {
  //    direction = MoveDirection.DOWN;
  //  } else if (currentMomentum.y > 0) {
  //    direction = MoveDirection.UP;
  //  }
  // }
  // 
  // public GameObject rowPrefab;
  // public GameObject parentRowPrefab;
  // public UIGridExtent grid;
  // public UIDraggableCamera draggableCamera;
  // public int cachedRowNum = 15;
  // public int startMoveIndex = 10;
  // public int currentNumRow;
  // 
  // [HideInInspector] public UIDragCameraExtent[] listRowScripts;
  // [HideInInspector] public List<RowChildData> cacheRows = new List<RowChildData>();
  // // gameobject of script to send callback function
  // [HideInInspector] public GameObject parent;
  // [HideInInspector] public int boundTop;
  // [HideInInspector] public int boundBottom;
  // [HideInInspector] public GameObject row;
  // [HideInInspector] public UIDragCameraExtent rowScript;
  // 
  // private bool isIniting = false;
  // 
  // public bool IsIniting {
  //  get { return isIniting; }
  // }
  // 
  // public virtual void Init(int numRow, List<JSONObject> rowDatas, GameObject parent) {
  //  isIniting = true;
  //  this.parent = parent;
  //  int initRowNum = 0;
  //  currentNumRow = numRow;
  //     // Reset();
  //  grid.ClearChildren();
  //  InitCallback(numRow, rowDatas, parent);
  // }
  // 
  // void InitCallback(int numRow, List<JSONObject> rowDatas, GameObject parent) {
  //  listRowScripts = new UIDragCameraExtent[numRow];
  //  cacheRows.Clear();
  //  int listRowCount = listRowScripts.Length;
  //  for (int i = 0; i < listRowCount; i++) {
  //    row = NGUITools.AddChild(grid.gameObject, parentRowPrefab);
  //    rowScript = row.transform.Find("VisibleCheck").GetComponent<UIDragCameraExtent>();
  //    listRowScripts[i] = rowScript;
  //    rowScript.Init(this, rowDatas[i]);
  //    row.name = Utils.GetSortCharacters(i);
  //    row.transform.localScale = Vector3.one;
  //    row.transform.parent = grid.transform;
  //    if (i < cachedRowNum) {
  //      GameObject tempChild = NGUITools.AddChild(row, rowPrefab);
  //      tempChild.name = "child";
  //      tempChild.GetComponent<UIDragCamera>().draggableCamera = draggableCamera;
  //      rowScript.AddChild(tempChild);
  //      rowScript.UpdateRowData(i, parent);
  //      cacheRows.Add(new RowChildData(i, tempChild));
  //      boundBottom = i;
  //    }
  //  }
  //  boundTop = 0;
  //  grid.Reset();
  //  Invoke("FinishInitData", 0.5f);
  // }
  // 
  // void FinishInitData() {
  //  isIniting = false;
  // }
  // 
  // public bool HasChild() {
  //  return grid.transform.childCount > 0;
  // }
  // 
  // public void MoveRow(int rowIndex, UIDragCameraExtent detectMoveRow) {
  //  if (isIniting) {
  //    return;
  //  }
  //  int nextRowIndex;
  //  if (direction == MoveDirection.DOWN) {
  //    if (rowIndex >= startMoveIndex) {
  //      nextRowIndex = boundBottom + 1;
  //      if (nextRowIndex < listRowScripts.Length && listRowScripts[nextRowIndex] != null && cacheRows[0].index < rowIndex - startMoveIndex) {         
  //        listRowScripts[nextRowIndex].AddChild(cacheRows[0].childPrefab);
  //        listRowScripts[nextRowIndex].UpdateRowData(nextRowIndex, parent);
  //        cacheRows.Add(new RowChildData(nextRowIndex, cacheRows[0].childPrefab));
  //        cacheRows.RemoveAt(0);
  //        detectMoveRow.RemoveChild();
  //        detectMoveRow.movedRow = nextRowIndex;
  //        boundTop++;
  //        boundBottom = nextRowIndex;
  //      }
  //    }
  //  } else {
  //    if (rowIndex <= listRowScripts.Length - startMoveIndex) {
  //      int lastCacheRow = cachedRowNum - 1;
  //      nextRowIndex = boundTop - 1;
  //      // Utils.Log("MOVE UP: " + nextRowIndex + " bound " + boundTop + " " + boundBottom);
  //      if (nextRowIndex >= 0 && listRowScripts[nextRowIndex] != null && cacheRows[lastCacheRow].index > rowIndex + startMoveIndex) {
  //        // Utils.Log("--MOVE UP");
  //        listRowScripts[nextRowIndex].AddChild(cacheRows[lastCacheRow].childPrefab);
  //        listRowScripts[nextRowIndex].UpdateRowData(nextRowIndex, parent);
  //        cacheRows.RemoveAt(lastCacheRow);
  //        cacheRows.Insert(0, new RowChildData(nextRowIndex, listRowScripts[nextRowIndex].child));
  //        detectMoveRow.RemoveChild();
  //        detectMoveRow.movedRow = nextRowIndex;
  //        boundTop = nextRowIndex;
  //        boundBottom--;
  //      }
  //    }
  //  }
  // }
  // 
  // public virtual void UpdateRowData(int rowIndex, GameObject childPrefab) {}
  // 
  // public class RowChildData {
  //  public int index;
  //  public GameObject childPrefab;
  //  
  //  public RowChildData(int index, GameObject child){
  //    this.index = index;
  //    this.childPrefab = child;
  //      }
  // 
  //  public void UpdateRowIndex(int aIndex) {
  //    index = aIndex;
  //  }
  // }
}
