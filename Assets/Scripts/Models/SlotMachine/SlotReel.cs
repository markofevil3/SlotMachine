using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SlotReel : MonoBehaviour {

  public Transform stopLineSprite;
  public int itemSize = 100;
  public delegate void OnFinished ();

	public OnFinished onFinished;
  
  private Transform targetItem;
	private int STOP_ROUND_COUNT = 6;
	private int targetIndex = 3;
	
	private bool stop = true;
	private bool isStopRound = false;
	private float stopLineY;
	private int targetPassCount = 0;
	private float speed = 20f;
	private float MIN_SPEED = 3f;
  private bool startCount = false;
  float bounceDistance = 11;
	float crtBounce = 0;
	Vector3 directionOfTravel;
	bool bounce = false;
  
	private Transform mTrans;
	private UIPanel mPanel;
	private UIScrollView mScroll;
	private bool mHorizontal = false;
	private bool mFirstTime = true;
	private List<Transform> mChildren = new List<Transform>();
  
  // list items
  private List<SlotItem> slotItems = new List<SlotItem>();
  private List<SlotItem.Type> slotItemTypes = new List<SlotItem.Type>();

  protected virtual void Start ()
	{
		SortBasedOnScrollMovement();
		WrapContent();
		mFirstTime = false;
		stopLineY = stopLineSprite.position.y;
		
		slotItems.Clear();
		slotItemTypes.Clear();
		
		for (int i = 0; i < (int)SlotItem.Type.TOTAL; i++) {
		  slotItemTypes.Add((SlotItem.Type)i);
		}
    slotItemTypes.Shuffle();
    SlotItem tempSlotItem;
    for (int i = 0; i < mChildren.Count; i++) {
      tempSlotItem = mChildren[i].GetComponent<SlotItem>();
      tempSlotItem.Init(slotItemTypes[i], i);
      slotItems.Add(tempSlotItem);
    }
	}

  public void Reset() {
    stop = true;
  	isStopRound = false;
  	targetPassCount = 0;
  	speed = 5f;
    startCount = false;
    crtBounce = 0;
  	bounce = false;
  }

  public void StartMachine() {
		for (int i = 0; i < slotItems.Count; i++) {
			slotItems[i].StopGlow();
		}
    stop = false;
  }

  public void SetTarget(SlotItem.Type target) {
    for (int i = 0; i < slotItems.Count; i++) {
      if (slotItems[i].IsTarget(targetIndex)) {
        targetItem = slotItems[i].transform;
      }
    }
    STOP_ROUND_COUNT = UnityEngine.Random.Range(5, 8);
    startCount = true;
  }

  public void SetResults(int[] items) {
    slotItems[2].Init((SlotItem.Type)items[0], 2);
    slotItems[3].Init((SlotItem.Type)items[1], 3);
    slotItems[4].Init((SlotItem.Type)items[2], 4);
    targetItem = slotItems[targetIndex].transform;
    STOP_ROUND_COUNT = UnityEngine.Random.Range(5, 8);
    startCount = true;
  }

	public void StartGlowItems() {
		for (int i = 0; i < slotItems.Count; i++) {
			slotItems[i].Glow();
		}
	}

	public List<SlotItem> GetSlotItems() {
		List<SlotItem> temp = new List<SlotItem>();
		temp.Add(slotItems[2]);
		temp.Add(slotItems[3]);
		temp.Add(slotItems[4]);
		return temp;
	}

	/// <summary>
	/// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
	/// </summary>

  // protected virtual void OnMove (UIPanel panel) { WrapContent(); }
  
	public void SortBasedOnScrollMovement ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		Transform t;
		for (int i = 0; i < mTrans.childCount; ++i) {
		  t = mTrans.GetChild(i);
		  mChildren.Add(t);
		}

		// Sort the list of children so that they are in order
		if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
		else mChildren.Sort(UIGrid.SortVertical);
		ResetChildPositions();
	}
	
	protected bool CacheScrollView ()
	{
		mTrans = transform;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
		mHorizontal = false;
		
		return true;
	}

	/// <summary>
	/// Helper function that resets the position of all the children.
	/// </summary>
	public void ResetChildPositions ()
	{
		for (int i = 0, imax = mChildren.Count; i < imax; ++i)
		{
			Transform t = mChildren[i];
			t.localPosition = mHorizontal ? new Vector3(i * itemSize, 0f, 0f) : new Vector3(0f, -i * itemSize, 0f);
		}
	}
	
	public static float ElasticEaseOut( float t, float b, float c, float d ){
    if ( ( t /= d ) == 1 )
        return b + c;

    float p = d * 0.3f;
    float s = p / 4;

    return (float)( c * Math.Pow( 2, -10 * t ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) + c + b );
  }
	
	void Update() {
	  if (stop) {
	    return;
	  }
 	  if (isStopRound) {
      directionOfTravel = stopLineSprite.position - targetItem.position;
      if (directionOfTravel.y < -0.01f) { 
        directionOfTravel.Normalize();
               
        //now normalize the direction, since we only want the direction information
        //scale the movement on each axis by the directionOfTravel vector components
        transform.Translate(0,(directionOfTravel.y * speed * Time.deltaTime),0,Space.World);
        WrapContent();
      } else {
        bounce = true;
        directionOfTravel.Normalize();
        if (crtBounce < bounceDistance) {
          transform.Translate(0,(-0.1f * speed * Time.deltaTime),0,Space.World);
          crtBounce++;
        } else {
          stop = true;
          SpringPosition sp = SpringPosition.Begin(gameObject,
                                                   new Vector3(transform.position.x, transform.position.y + Mathf.Abs(targetItem.position.y - stopLineSprite.position.y), transform.position.z),
                                                   8f);
          sp.worldSpace = true;
          sp.onFinished = EventReelFinish;
        }
      }
	  } else {
      // speed = Mathf.Max(speed - targetPassCount / 1.5f * Time.deltaTime, MIN_SPEED);
  	  transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.deltaTime), transform.position.z);
  	  WrapContent();
	  }
	}
	
	private void EventReelFinish() {
	  if (onFinished != null) {
	    onFinished();
	  }
	}
	
	public void WrapContent ()
	{
		float extents = itemSize * mChildren.Count * 0.5f;
		Vector3[] corners = mPanel.worldCorners;
		
		for (int i = 0; i < 4; ++i)
		{
			Vector3 v = corners[i];
			v = mTrans.InverseTransformPoint(v);
			corners[i] = v;
		}
		
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
		bool allWithinRange = true;
		float ext2 = extents * 2f;

		float min = corners[0].y - itemSize;
		float max = corners[2].y + itemSize;

		for (int i = 0, imax = mChildren.Count; i < imax; ++i)
		{
			Transform t = mChildren[i];
			float distance = t.localPosition.y - center.y;
			if (distance < -extents)
			{
				Vector3 pos = t.localPosition;
				pos.y += ext2;
				distance = pos.y - center.y;
				int realIndex = Mathf.RoundToInt(pos.y / itemSize);
        t.localPosition = pos;
        if (startCount && slotItems[i].IsTarget(targetIndex)) {
          targetPassCount++;
          if (targetPassCount == STOP_ROUND_COUNT - 1) {
            // CalculateLengthToStopLine(t);
            isStopRound = true;
          }
        }
			}
			else if (distance > extents)
			{			  
				Vector3 pos = t.localPosition;
				pos.y -= ext2;
				distance = pos.y - center.y;
				int realIndex = Mathf.RoundToInt(pos.y / itemSize);
        t.localPosition = pos;
			}
		}
	}
}
