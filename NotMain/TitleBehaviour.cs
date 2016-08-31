using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleBehaviour : MonoBehaviour {

   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick_Start()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClick_End()
    {
        Application.Quit();
    }
}
