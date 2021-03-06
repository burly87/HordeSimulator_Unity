using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveEx0 : MonoBehaviour {

	//ADDS NEW KEYFRAMES TO A CURVE
	//AND MODIFIES EXISTING KEYFRAMES

	public KeyCode addK = KeyCode.Keypad0;
	public int maxKeyFrames = 10;

	public string[] labels;

	[System.Serializable] 
	public class Factors {

		[HideInInspector] public string label;
		public AnimationCurve curve;
		[Range(0.0f, 1.0f)] public float time;
		[Range(-1.0f, 1.0f)] public float value;
		public bool inc;//increasing or decreasing
	}
	public Factors[] factors;

	Keyframe[] ks;
	public int i = 1;
	int x;
	bool removed;
	int j;

	public bool doSetupAtStart;

	void Start()
	{
		if (doSetupAtStart) {
			for (j = 0; j < factors.Length; j++) {

				factors [j].label = labels [j];
				ks = new Keyframe[maxKeyFrames];

				if (factors [j].inc) factors [j].curve = new AnimationCurve (new Keyframe (0, 0), new Keyframe (1, 1));
				if (!factors [j].inc) factors [j].curve = new AnimationCurve (new Keyframe (0, 1), new Keyframe (1, 0));
				factors [j].curve.preWrapMode = WrapMode.Once;
				factors [j].curve.postWrapMode = WrapMode.Once;
			}
		}
	}


	void Update()
	{
		if (Input.GetKeyDown (addK)) {

			removed = false;
			for (x = 0; x < ks.Length; x++) {
				
				if (ks[x].time == factors[i].time) {
					factors[i].curve.RemoveKey (x);
					ks [x] = new Keyframe (factors[i].time, factors[i].value);
					factors[i].curve.AddKey (ks [x]);
					removed = true;
					print("key removed " + x);
					return;
				}
			}

			if (!removed && i<ks.Length-1) {
				ks [i] = new Keyframe (factors[i].time, factors[i].value);
				factors[i].curve.AddKey (ks [i]);
				print("key added " + i);
				i += 1;
			}
		}
	}



	/*
	//for adding keys from other script
	public void addKeyAt(float timex, float valuex){

		removed = false;
		for (x = 0; x < ks.Length; x++) {

			if (ks[x].time == timex) {
				curve.RemoveKey (x);
				ks [x] = new Keyframe (timex, valuex);
				curve.AddKey (ks [x]);
				removed = true;
				print("key removed " + x);
				return;
			}
		}

		if (!removed && i<ks.Length-1) {
			ks [i] = new Keyframe (timex, valuex);
			curve.AddKey (ks [i]);
			print("key added " + i);
			i += 1;
		}
	}
	*/


}
