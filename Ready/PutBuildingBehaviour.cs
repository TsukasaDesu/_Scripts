using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PutBuildingBehaviour : MonoBehaviour
{

    public GameObject CanPutPanel;
    public GameObject CanPutBox;
    public ClickObjClass clickobj_class;
    public GameObject Building;
    GameObject canputbox;
    GameObject canputpanel;
    public float building_height;
    float wait_time;
    int[,] put_box = new int[12, 12];//置かれているかどうか
    int[] point = new int[2];//[0]=i,x,[1]=j,y

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (wait_time > 0)
        //{
        //    wait_time += Time.deltaTime;
        //    if (wait_time > 2)
        //    {
        //       ReadyPanelBehaviour.root.SetActive(true);
        //        wait_time = 0;
        //    }
        //}
        if (ReadyPanelBehaviour.root.activeSelf && GameMasterBehaviour.NextFlg) return;

        transform.GetChild(0).gameObject.GetComponent<Text>().text = "設置する建物:"+clickobj_class.m_title;
        transform.GetChild(1).gameObject.GetComponent<Text>().text = "費用:" + clickobj_class.m_cost;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        

        if (Physics.Raycast(ray, out hit,1000))
        {
            Debug.Log(hit.collider.gameObject);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10.0f);
            GameObject obj = hit.collider.gameObject;

            if (hit.collider.tag == "CanPutPanel")
            {
                if (canputbox != null) Destroy(canputbox);
                string[] split_name = hit.collider.gameObject.name.Split(':');
                point[0] = int.Parse(split_name[0]);
                point[1] = int.Parse(split_name[1]);
                canputbox = Instantiate(CanPutBox, new Vector3(hit.collider.gameObject.transform.position.x, 6, hit.collider.gameObject.transform.position.z), Quaternion.identity) as GameObject;
            }
            else if (hit.collider.tag != "CanPutBox")
                Destroy(canputbox);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag != "CanPutBox" || PlayerBehaviour.money < clickobj_class.m_cost) return;
                clickobj_class.Clicked();
                put_box[point[0], point[1]] = 1;
                Instantiate(Building,new Vector3(canputbox.transform.position.x,building_height,canputbox.transform.position.z), Quaternion.identity);
                //Destroy(canputpanel);
                Destroy(canputbox);

                //for(int i = 0; i < gameObject.transform.childCount;i++)
                 //   Destroy(gameObject.transform.GetChild(i).gameObject);

                //wait_time = 1;
            }
        }
    }

    public void OnClick_Back()
    {
        if (canputbox != null) Destroy(canputbox);
        gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }

    public void InitCanPanel()
    {
        for (int i = 0; i < put_box.GetLength(0); i++)
        {
            for (int j = 0; j < put_box.GetLength(1); j++)
            {
                if (put_box[i, j] == 0)
                {
                    GameObject clone = Instantiate(CanPutPanel, new Vector3(100 + 20 * i, 1.1f, -100 + -20 * j), Quaternion.identity) as GameObject;
                    clone.transform.parent = gameObject.transform;
                    clone.name = i + ":" + j;
                }
            }
        }
    }
}
