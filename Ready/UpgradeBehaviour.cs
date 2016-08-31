using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeBehaviour : MonoBehaviour {

    GameObject text_src;
    BuildingClass building_class;
    List<GameObject> text_all;
    public GameObject readypanel;
    public GameObject upgrade_window;

	// Use this for initialization
	void Start () {
        text_src = Resources.Load<GameObject>("UI/Text");
        text_all = new List<GameObject>();
        readypanel = transform.parent.GetChild(1).gameObject;
        upgrade_window = transform.GetChild(0).gameObject;
        upgrade_window.SetActive(false);
        gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.collider.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag != "Building") return;

                PlayerBehaviour.root.GetComponent<ThirdPersonCameraController>().mouseSensitivity = 0;

                building_class = null;
                GameObject have_compornent_obj = null;
                while(building_class == null)
                {
                     have_compornent_obj = (have_compornent_obj == null)?hit.collider.gameObject:have_compornent_obj.transform.parent.gameObject;
                    if (have_compornent_obj.GetComponent<BuildingBehaviour>() != null)
                    {
                        building_class = have_compornent_obj.GetComponent<BuildingBehaviour>().building_class;
                        break;
                    }
                }

                //building_class = (hit.collider.gameObject.GetComponent<BuildingBehaviour>() == null)?
                //        hit.collider.gameObject.transform.parent.gameObject.GetComponent<BuildingBehaviour>().building_class:
                //        hit.collider.gameObject.GetComponent<BuildingBehaviour>().building_class;

                building_class.upgrade();

                upgrade_window.SetActive(true);

                upgrade_window.transform.FindChild("Title").gameObject.GetComponent<Text>().text = building_class.m_name;
                upgrade_window.transform.FindChild("Cost").gameObject.GetComponent<Text>().text = "費用:"+building_class.m_upgrade_cost+"円";
                
                for(int i = 0; i < building_class.m_upgrade_text.Count;i++)
                {
                    text_all.Add(Instantiate(text_src, Vector3.zero, Quaternion.identity) as GameObject);
                    text_all[i].transform.parent = upgrade_window.transform;
                    text_all[i].transform.localPosition = new Vector3(20, 180 - 40 * i, 0);
                    text_all[i].GetComponent<Text>().text = building_class.m_upgrade_text[i];
                }
               
            }
        }
    }
    
    public void Click_Upgrade()
    {
        if (PlayerBehaviour.money < building_class.m_upgrade_cost) return;
        
        PlayerBehaviour.money -= building_class.m_upgrade_cost;

        if(building_class.m_level < 3)
        building_class.m_level++;
        building_class.m_ability_power[0] = building_class.m_ability_power[1];
        building_class.m_action_rate[0] = building_class.m_action_rate[1];
        if (building_class.m_range[0] != -1)
            building_class.m_range[0] = building_class.m_range[1];

        foreach (GameObject obj in text_all)
            Destroy(obj);
        text_all.Clear();

        upgrade_window.SetActive(false);
        PlayerBehaviour.root.GetComponent<ThirdPersonCameraController>().mouseSensitivity = 30;

    }

    public void Click_Back()
    {
        foreach (GameObject obj in text_all)
            Destroy(obj);
        text_all.Clear();

        upgrade_window.SetActive(false);
        readypanel.SetActive(true);
        gameObject.SetActive(false);
        PlayerBehaviour.root.GetComponent<ThirdPersonCameraController>().mouseSensitivity = 30;

    }
}
