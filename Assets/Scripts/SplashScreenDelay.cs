using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenDelay : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        //Delays on the splash screen for 4 seconds
    
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("MainMenu");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
