using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIWrapContentCamera : MonoBehaviour {
	
  // public int itemSize = 100;
  // 
  // /// <summary>
  // /// Whether the content will be automatically culled. Enabling this will improve performance in scroll views that contain a lot of items.
  // /// </summary>
  // 
  // public bool cullContent = true;
  // 
  // /// <summary>
  // /// Minimum allowed index for items. If "min" is equal to "max" then there is no limit.
  // /// For vertical scroll views indices increment with the Y position (towards top of the screen).
  // /// </summary>
  // 
  // public int minIndex = 0;
  // 
  // /// <summary>
  // /// Maximum allowed index for items. If "min" is equal to "max" then there is no limit.
  // /// For vertical scroll views indices increment with the Y position (towards top of the screen).
  // /// </summary>
  // 
  // public int maxIndex = 0;
  // 
  // Transform mTrans;
  // UICamera mCamera;
  // UIDraggableCamera mScroll;
  // bool mHorizontal = false;
  // bool mFirstTime = true;
  // List<Transform> mChildren = new List<Transform>();
  // 
  // void Start ()
  // {
  //  SortBasedOnScrollMovement();
  //  WrapContent();
  //  if (mScroll != null) mScroll.onMove = OnMove;
  //  mFirstTime = false;
  // }
  // 
  // public void SortBasedOnScrollMovement ()
  // {
  //  if (!CacheScrollView()) return;
  // 
  //  // Cache all children and place them in order
  //  mChildren.Clear();
  //  for (int i = 0; i < mTrans.childCount; ++i)
  //    mChildren.Add(mTrans.GetChild(i));
  // 
  //  // Sort the list of children so that they are in order
  //  if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
  //  else mChildren.Sort(UIGrid.SortVertical);
  //  ResetChildPositions();
  // }
  // 
  // void ResetChildPositions ()
  // {
  //  for (int i = 0, imax = mChildren.Count; i < imax; ++i)
  //  {
  //    Transform t = mChildren[i];
  //    t.localPosition = mHorizontal ? new Vector3(i * itemSize, 0f, 0f) : new Vector3(0f, -i * itemSize, 0f);
  //  }
  // }
  // 
  // protected bool CacheScrollView ()
  // {
  //  mTrans = transform;
  //  if (mScroll == null) return false;    
  //  if (mScroll.scale.x == 1) mHorizontal = true;
  //  else if (mScroll.scale.y == 1) mHorizontal = false;
  //  else return false;
  //  return true;
  // }
  // 
  // protected virtual void OnMove() { WrapContent(); }
  // 
  // void WrapContent() {
  //  
  // }
}
