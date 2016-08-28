using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverBehaiviour : MonoBehaviour {



	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.GetComponent<Text>().text = "生存時間   " + string.Format("{0:00}", Mathf.Floor(PlayerBehaviour.result_time / 60)) + ":" + string.Format("{0:00}", Mathf.Floor(PlayerBehaviour.result_time) % 60) + ":" + string.Format("{0:00}", (PlayerBehaviour.result_time - Mathf.Floor(PlayerBehaviour.result_time)) * 100);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick_Retry()
    {
        SceneManager.LoadScene(0);
    }
}
