using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryBehaviour : MonoBehaviour {

    Text txt;
    Animator anim;
    public GameObject belt_point;
    public GameObject born_point;
    GameObject[] box = new GameObject[3];
    GameObject clone;
    float cnt;
    public GameObject RedLight;
    public GameObject player_src;
    public GameObject player_look;
    public GameObject LightBlock;
    int flg;

	// Use this for initialization
	void Start () {
        flg = 0;
        txt = transform.GetChild(0).gameObject.GetComponent<Text>();
        anim = Camera.main.gameObject.GetComponent<Animator>();
        box[0] = Resources.Load<GameObject>("Enemy/enemy_small");
        box[1] = Resources.Load<GameObject>("Enemy/enemy_normal");
        box[2] = Resources.Load<GameObject>("Enemy/enemy_big");
        StartCoroutine(Story());
        clone = Instantiate(box[Random.Range(0, 3)], born_point.transform.position, Quaternion.identity) as GameObject;
        Destroy(clone.GetComponent<EnemyBehaviour>());
        Destroy(clone.GetComponent<NavMeshAgent>());
        Destroy(clone.transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update () {
        cnt += Time.deltaTime;

        if (flg == 1 || flg == 2)
        {
            if (cnt > 0.5f)
            {
                if (RedLight.GetComponent<Light>().intensity == 0)
                {
                    RedLight.GetComponent<Light>().intensity = 8;
                    LightBlock.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1,0,0,1));
                }
                else
                {
                    RedLight.GetComponent<Light>().intensity = 0;
                    LightBlock.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f, 1));
                }
                cnt = 0;

                if (flg != 2) return;
                clone = Instantiate(box[Random.Range(0, 3)], born_point.transform.position, Quaternion.identity) as GameObject;
                clone.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
                Destroy(clone.GetComponent<EnemyBehaviour>());
                Destroy(clone.GetComponent<NavMeshAgent>());
                Destroy(clone.transform.GetChild(0).gameObject);
            }
            return;
        }
        
        
        belt_point.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, Time.time));
        if (cnt > 4f)
        {
            clone = Instantiate(box[Random.Range(0, 3)], born_point.transform.position, Quaternion.identity) as GameObject;
            Destroy(clone.GetComponent<EnemyBehaviour>());
            Destroy(clone.GetComponent<NavMeshAgent>());
            Destroy(clone.transform.GetChild(0).gameObject);
            cnt = 0;
        }
        else
            clone.GetComponent<Rigidbody>().velocity = new Vector3(0,-1,-1f);
	}

    IEnumerator Story()
    {


        txt.text = "ここはキューブ工場";

        yield return new WaitForSeconds(4);
        txt.text = "";

        yield return new WaitForSeconds(1);
        txt.text = "今日もたくさんのキューブがつくられている";

        yield return new WaitForSeconds(4);
        txt.text = "";

        yield return new WaitForSeconds(1);
        txt.text = "しかしある日...";

        GameObject player =Instantiate(player_src, born_point.transform.position, Quaternion.identity) as GameObject;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1);
        Destroy(player.GetComponent<PlayerBehaviour>());
        Destroy(player.GetComponent<ThirdPersonCameraController>());


        yield return new WaitForSeconds(5.5f);
        player_look.SetActive(true);
        player_look.transform.position = player.transform.position + new Vector3(0.5f, 1, 0);
        player_look.transform.LookAt(Camera.main.transform.position);
        player_look.transform.Rotate(0, -180, 0,Space.World);
        Time.timeScale = 0.01f;
        txt.text = "なんとキュウブ(球部)が紛れ込んでいたのだ";

        yield return new WaitForSeconds(0.05f);
        player_look.SetActive(false);
        txt.text = "すぐさま緊急ベルが鳴らされ";
        RedLight.GetComponent<Light>().color = new Color(1, 0, 0, 1);
        Time.timeScale = 1;
        flg = 1;
        yield return new WaitForSeconds(3);
        txt.text = "キューブはキュウブ(球部)を排除するために迎撃に向かった";
        flg = 2;

        yield return new WaitForSeconds(5);
        anim.SetBool("Flg", true);

        yield return new WaitForSeconds(5);
       

        txt.text = "かくしてキュウブ(球部)vsキューブの戦が幕を開けたのであった...";

        yield return new WaitForSeconds(3);
        txt.text = "";
        transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Title");
        yield return new WaitForSeconds(7);

        SceneManager.LoadScene(2);


    }

    public void OnClick_Skip()
    {
        SceneManager.LoadScene(2);
    }


}
