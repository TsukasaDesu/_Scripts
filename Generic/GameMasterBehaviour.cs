using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMasterBehaviour : MonoBehaviour {

    public static int now_enmey_num;
    public static bool NextFlg;
    public static float interval_wall;
    public static int wave;

    public GameObject ApplyPoint;
    public GameObject ReadyPanel;
    public Text EnemyNumText;

    int[] max_enemy_num = new int[10];
    int ready_enemy_num;
    GameObject[] Enemy = new GameObject[3];
    float apply_cnt;

    void Awake()
    {
        Enemy[0] = Resources.Load<GameObject>("Enemy/enemy_small");
        Enemy[1] = Resources.Load<GameObject>("Enemy/enemy_normal");
        Enemy[2] = Resources.Load<GameObject>("Enemy/enemy_big");

        wave = 0;
        max_enemy_num[0] = 15;
        max_enemy_num[1] = 20;
        max_enemy_num[2] = 25;
        max_enemy_num[3] = 30;
        max_enemy_num[4] = 40;
        max_enemy_num[5] = 50;
        max_enemy_num[6] = 55;
        max_enemy_num[7] = 30;
        max_enemy_num[8] = 80;
        max_enemy_num[9] = 100;
        now_enmey_num = 0;
        ready_enemy_num = max_enemy_num[0];
        ReadyPanel.transform.FindChild("NextWave").gameObject.GetComponent<Text>().text = "次:Wave" + wave;

        NextFlg = false;
    }

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        EnemyNumText.text = "敵の数 "+now_enmey_num + "/" + max_enemy_num[wave];
        interval_wall += Time.deltaTime;
        if (interval_wall > 7)
            interval_wall = 0;

        if (!NextFlg) return;

        if (ready_enemy_num > 0)
        {
            apply_cnt += Time.deltaTime;
            if (apply_cnt > 0.3f)
            {
                ready_enemy_num--;
                now_enmey_num++;
                Instantiate(Enemy[Random.Range(0,Enemy.Length)], ApplyPoint.transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)), Quaternion.identity);
                apply_cnt = 0;
            }
        }

        if (now_enmey_num != 0 || ready_enemy_num != 0) return;
        wave++;
        ready_enemy_num = max_enemy_num[wave];
        NextFlg = false;
        ReadyPanel.SetActive(true);
        ReadyPanel.transform.FindChild("NextWave").gameObject.GetComponent<Text>().text = "次:Wave" + wave;
    }


}
