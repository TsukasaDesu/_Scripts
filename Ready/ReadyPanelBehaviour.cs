using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReadyPanelBehaviour : MonoBehaviour
{

    public static GameObject root;
    public GameObject ClickObj;
    GameObject UpgradePanel;
    GameObject PutBuildingPanel;
    GameObject[] obj_clone = new GameObject[8];

    ClickObjClass[] objclass = new ClickObjClass[8];

    void Start()
    {
        root = gameObject;
        UpgradePanel = transform.parent.GetChild(2).gameObject;
        PutBuildingPanel = transform.parent.GetChild(3).gameObject;
        objclass[0] = new HealPlayer();
        objclass[1] = new PowerUP();
        objclass[2] = new BuildMoney();
        objclass[3] = new BuildAutoGun();
        objclass[4] = new BuildSlatingShot();
        objclass[5] = new BuildStop();
        objclass[6] = new BuildSlantingBomb();
        objclass[7] = new BuildResponceShot();

    }

    void Update()
    {

    }

    void CreateObj(Vector3 posi, int no)//objclassの番号
    {
        obj_clone[no] = Instantiate(ClickObj, Vector3.zero, Quaternion.identity) as GameObject;
        obj_clone[no].transform.parent = gameObject.transform.FindChild("Panel");
        obj_clone[no].transform.localPosition = posi;
        obj_clone[no].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = objclass[no].m_title;
        obj_clone[no].transform.GetChild(2).gameObject.GetComponent<Text>().text = "費用:" + objclass[no].m_cost + "円";
        obj_clone[no].transform.GetChild(1).gameObject.GetComponent<Text>().text = objclass[no].m_explain;
        obj_clone[no].transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerBehaviour.money < objclass[no].m_cost) return;
            if (objclass[no].m_building == null)
                objclass[no].Clicked();
            Destroy(obj_clone[no]);
            CreateObj(posi, no);
            if (objclass[no].m_building != null)
            {
                PutBuildingPanel.GetComponent<PutBuildingBehaviour>().clickobj_class = objclass[no];
                PutBuildingPanel.GetComponent<PutBuildingBehaviour>().Building = objclass[no].m_building;
                PutBuildingPanel.GetComponent<PutBuildingBehaviour>().building_height = objclass[no].m_height;
                PutBuildingPanel.GetComponent<PutBuildingBehaviour>().InitCanPanel();
                PutBuildingPanel.SetActive(true);
                gameObject.SetActive(false);
            }

        });
    }




    public void Click_Ziki()
    {
        if (root.transform.FindChild("Panel").childCount > 0)
        {
            for (int i = 0; i < root.transform.FindChild("Panel").childCount; i++)
                Destroy(root.transform.FindChild("Panel").GetChild(i).gameObject);
        }
        CreateObj(new Vector3(-320, 920, 0), 0);
        CreateObj(new Vector3(-320, 820, 0), 1);
    }

    public void Click_Building()
    {
        if (gameObject.transform.FindChild("Panel").childCount > 0)
        {
            for (int i = 0; i < gameObject.transform.FindChild("Panel").childCount; i++)
                Destroy(gameObject.transform.FindChild("Panel").GetChild(i).gameObject);
        }

        CreateObj(new Vector3(-320, 920, 0), 2);
        CreateObj(new Vector3(-320, 820, 0), 3);
        CreateObj(new Vector3(-320, 720, 0), 4);
        CreateObj(new Vector3(-320, 620, 0), 5);
        CreateObj(new Vector3(-320, 520, 0), 6);
        CreateObj(new Vector3(-320, 420, 0), 7);
    }

    public void Click_Upgrade()
    {
        gameObject.SetActive(false);
        UpgradePanel.SetActive(true);
    }

    public void Click_Next()
    {
        GameMasterBehaviour.NextFlg = true;
        gameObject.SetActive(false);
    }

    public void OnBarChanged()
    {
        transform.FindChild("Panel").localPosition = new Vector3(200, transform.FindChild("Scrollbar").gameObject.GetComponent<Scrollbar>().value * 1000 - 700);
    }

}
