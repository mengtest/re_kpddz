using UnityEngine;
using System.Collections;

public class PlayLoginBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

    void OnEnable()
    {
        GameObject pSingletonObj = GameObject.Find("Singleton");
        if (pSingletonObj != null)
        {
            UtilTools.SetBgm("Sounds/BGM/login");
        }
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
