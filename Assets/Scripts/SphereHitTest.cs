using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHitTest : MonoBehaviour {
    [SerializeField] Transform _sphereOne;
    [SerializeField] Transform _sphereTwo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Mathf.Pow(_sphereTwo.position.x - _sphereOne.position.x, 2) +
                         Mathf.Pow(_sphereTwo.position.y - _sphereOne.position.y, 2) +
                         Mathf.Pow(_sphereTwo.position.z - _sphereOne.position.z, 2);
        float rad = (_sphereOne.localScale.x + _sphereTwo.localScale.x) / 2;

        if(distance < rad * rad)
        {
            Debug.Log("hit");
        }
        else
        {
            Debug.Log("not hit");
        }
	}
}
