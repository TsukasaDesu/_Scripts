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
    public float height_subcam = 100;

    GameObject tpscamera;
    GameObject subcamera;

    Rigidbody rigid;

    // Use this for initialization
    void Awake   () {
        root = gameObject;
        money = 1000;
        hp = 100;
        power = 30;
        rigid = GetComponent<Rigidbody>();
        result_time = 0;
        tpscamera = GameObject.Find("TPSCamera");
        subcamera = GameObject.Find("SubCamera");
        Camera.main.depth = 1;
    }

    // Update is called once per frame
    void Update() {
        result_time += Time.deltaTime;
        result_time_txt.text = string.Format("{0:00}", Mathf.Floor(result_time / 60)) + ":" + string.Format("{0:00}", Mathf.Floor(result_time) % 60) + ":" + string.Format("{0:00}",(result_time - Mathf.Floor(result_time))*100);
        money_text.text = "所持金:" + money + "円";
        hp_gage.value = hp;
        rigid.velocity = ((tpscamera.tag == "MainCamera") ? new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z):new Vector3(Camera.main.transform.up.x,0,Camera.main.transform.up.z)) * Input.GetAxis("Vertical") * 10 + Camera.main.transform.right * Input.GetAxis("Horizontal") * 10;
        subcamera.transform.position = transform.position + new Vector3(0, height_subcam, 0);
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameMasterBehaviour.NextFlg) return;

            GameObject bul;
            if(tpscamera.tag == "MainCamera")
            {
                bul = Instantiate(bullet, transform.position + Camera.main.transform.forward, Quaternion.Euler(transform.forward)) as GameObject;
                bul.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 50;
                Destroy(bul, 1);
            }
            else
            {
                Vector3 mousepos = Input.mousePosition;
                mousepos.z = Camera.main.transform.position.z;
                Vector3 mousepos_world = Camera.main.ScreenToWorldPoint(mousepos);
                Debug.Log((mousepos_world - transform.position).normalized);
                bul = Instantiate(bullet, transform.position + new Vector3(transform.position.x-mousepos_world.x,0,transform.position.z-mousepos_world.z).normalized*2, Quaternion.Euler(transform.forward)) as GameObject;
                bul.GetComponent<Rigidbody>().velocity = new Vector3(transform.position.x - mousepos_world.x, 0, transform.position.z - mousepos_world.z).normalized * 50;
                Destroy(bul, 1);

            }


        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(tpscamera.tag == "MainCamera")
            {
                Camera.main.depth = 0;
                subcamera.tag = "MainCamera";
                tpscamera.tag = "Untagged";
                Camera.main.depth = 1;
            }
            else
            {
                Camera.main.depth = 0;
                subcamera.tag = "Untagged";
                tpscamera.tag = "MainCamera";
                Camera.main.depth = 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            height_subcam += Input.GetAxis("Mouse ScrollWheel") * 100;
            height_subcam = Mathf.Clamp(height_subcam, 10.0f, 200.0f);
        }

         if (hp <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
