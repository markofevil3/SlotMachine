using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	void Start () {
		TLMBCard card = new TLMBCard("3H");
		Debug.Log(card.ToString());

		TLMBCard card2 = new TLMBCard(36);
		Debug.Log(card2.ToString());

		TLMBCombination combination = new TLMBCombination("3S3H3D");
		Debug.Log(combination.ToCardString() + " " + combination.GetTypeName());
	}
}