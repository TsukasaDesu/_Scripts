using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

    public static GameObject root;
    public static int money;
    public static float hp;
    public static float power;
    public static float result_time;

    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public GameObject bullet;
    public Text money_text;
    public Slider hp_gage;
    public Text result_time_txt;

    Rigidbody rigid;

    // Use this for initialization
    void Awake   () {
        root = gameObject;
        money = 1000;
        hp = 100;
        power = 30;
        rigid = GetComponent<Rigidbody>();
        result_time = 0;
	}

    // Update is called once per frame
    void Update() {
        result_time += Time.deltaTime;
        result_time_txt.text = string.Format("{0:00}", Mathf.Floor(result_time / 60)) + ":" + string.Format("{0:00}", Mathf.Floor(result_time) % 60) + ":" + string.Format("{0:00}",(result_time - Mathf.Floor(result_time))*100);
        money_text.text = "所持金:" + money + "円";
        hp_gage.value = hp;
        rigid.velocity = new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z) * Input.GetAxis("Vertical")*10+ Camera.main.transform.right * Input.GetAxis("Horizontal") * 10;

        if (Input.GetMouseButtonDown(0))
        {
            if (!GameMasterBehaviour.NextFlg) return;
            GameObject bul = Instantiate(bullet, transform.position + Camera.main.transform.forward, Quaternion.Euler(transform.forward)) as GameObject;
            bul.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 50;
            Destroy(bul, 1);
        }

        if(hp <= 0)
        {
            SceneManager.LoadScene(1);
        }

        //ExecuteEvents.Execute<IRecieveMessage>(
        //                    target: hit.collider.gameObject,
        //                    eventData: null,
        //                    functor: (recievetarget, y) => recievetarget.OnRecieve(gun[no].power)
        //                    );
    }
}
