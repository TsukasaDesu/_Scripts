using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public abstract class BuildingClass : MonoBehaviour {


    public float m_delta_cnt;
    public int m_level;//建物のレベル
    public int m_upgrade_cost;
    public string m_name;//名前
    public float m_hp;//建物の耐久力(今のところは実装予定なし)

    //[0]はLvUp前(実際に使用するやつ)、[1]はLvUp後
    public float[] m_action_rate = new float[2];//間隔
    public int[] m_ability_power = new int[2];//お金のあがり、銃の威力など
    public int[] m_range = new int[2];//射程

    public List<string> m_upgrade_text;//アップグレード時のテキスト

    public BuildingClass()
    {
        m_name = "テスト";
        m_hp = 100;
        m_level = 0;
        m_ability_power[0] = -1;
        m_action_rate[0] = 1;
        m_action_rate[1] = 2;
        m_upgrade_cost = 30;
        m_range[0] = -1;
        m_upgrade_text = new List<string>();
    }

    public virtual void ability(GameObject root)
    {

    }

    public virtual void trigger_enter(Collider col)
    {

    }

    public virtual void trigger_out(Collider col)
    {

    }


    public virtual void upgrade()
    {
   
    }

}

public class MoneyBuilding:BuildingClass
{

    public MoneyBuilding()
    {
        m_name = "増金施設";
        m_ability_power[0] = 1;
        m_action_rate[0] = 4;
        m_upgrade_text = new List<string>();
    }

    public override void ability(GameObject root)
    {
        m_delta_cnt += Time.deltaTime;

        if(m_delta_cnt >= m_action_rate[0])
        {
            PlayerBehaviour.money+=m_ability_power[0];
            m_delta_cnt = 0;
        }
    }

    public override void upgrade()
    {
        switch(m_level)
        {
            case 0:
                m_ability_power[1] = 2;
                m_action_rate[1] = 3;
                m_upgrade_cost = 40;
                break;
            case 1:
                m_ability_power[1] = 3;
                m_action_rate[1] = 2;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_ability_power[1] = 4;
                m_action_rate[1] = 1;
                m_upgrade_cost = 200;
                break;
            case 3:

                break;
        }
        if(m_upgrade_text.Count > 0)
        m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("お金:" + m_ability_power[0]+"円" + "→" + m_ability_power[1]+"円");
        m_upgrade_text.Add("間隔:" + m_action_rate[0]+"秒" + "→" + m_action_rate[1]+"秒");
    }

}

public class AutoGunBuilding:BuildingClass
{
    List<GameObject> nearEnemys;
    GameObject nearestEnemy;
    GameObject hiteffect;

    public AutoGunBuilding()
    {
        m_name = "自動銃";
        m_range[0] = 20;
        m_ability_power[0] = 30;
        nearEnemys = new List<GameObject>();
        hiteffect = Resources.Load<GameObject>("Effect/HitEffect");
        m_upgrade_text = new List<string>();
    }

    public override void ability(GameObject root)
    {
        nearEnemys = nearEnemys.Where(gmObj => gmObj != null).ToList();

        nearestEnemy = nearEnemys.Count() < 1 ? null : nearEnemys.OrderBy<GameObject, float>(gmObj => Vector3.Distance(gmObj.transform.position, root.transform.position)).First();

        if (nearestEnemy != null)
        {
            root.transform.GetChild(1).LookAt(nearestEnemy.transform.position);
            m_delta_cnt += Time.deltaTime;
            if (m_delta_cnt > m_action_rate[0])
            {
                ExecuteEvents.Execute<IRecieveMessage>(
                           target: nearestEnemy,
                           eventData: null,
                           functor: (recievetarget, y) => recievetarget.OnRecieve(m_ability_power[0])
                           );
                GameObject clone = Instantiate(hiteffect, nearestEnemy.transform.position - root.gameObject.transform.forward, Quaternion.Euler(root.transform.forward)) as GameObject;
                Destroy(clone, 2);
                m_delta_cnt = 0;
            }
        }
    }

    public override void trigger_enter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            nearEnemys.Add(col.gameObject);
        }
    }

    public override void trigger_out(Collider col)
    {
        if (col.tag == "Enemy")
            nearEnemys.Remove(col.gameObject);
    }

    public override void upgrade()
    {
        switch(m_level)
        {
            case 0:
                m_action_rate[1] = 1.5f;
                m_ability_power[1] = 50;
                m_range[1] = 30;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_action_rate[1] = 1.0f;
                m_ability_power[1] = 75;
                m_range[1] = 40;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_action_rate[1] = 0.5f;
                m_ability_power[1] = 100;
                m_range[1] = 50;
                m_upgrade_cost = 150;
                break;
            case 3:
                break;
        }
        if(m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0] + "→" + m_ability_power[1]);
        m_upgrade_text.Add("間隔:" + m_action_rate[0]+"秒" + "→" + m_action_rate[1]+"秒");
        m_upgrade_text.Add("射程:"+m_range[0]+"m" + "→" +m_range[1]+"m");
    }

}

public class SlantingShotBuilding:BuildingClass
{
    GameObject bullet_src;
    float cnt;

    public SlantingShotBuilding()
    {
        m_name = "斜め撃ち";
        m_ability_power[0] = 30;
        m_action_rate[0] = 2;
        m_upgrade_cost = 50;
        bullet_src = Resources.Load<GameObject>("Bullet/SlantingBullet");
        cnt = 0;
    }

    public override void ability(GameObject root)
    {
        root.transform.GetChild(0).Rotate(0, m_level*5+1, 0);
        m_delta_cnt += Time.deltaTime;
        if(m_delta_cnt > m_action_rate[0])
        {
            for (int i = 0; i < m_level + 1; i++)
            {
                GameObject bul = Instantiate(bullet_src, root.transform.position+root.transform.right*Mathf.Sin(Mathf.Deg2Rad*i*90)*2+root.transform.forward* Mathf.Cos(Mathf.Deg2Rad * i * 90)*2, Quaternion.identity) as GameObject;
                bul.GetComponent<SlantingBulletBehaviour>().power = m_ability_power[0];
                bul.transform.parent = root.transform;
                bul.GetComponent<Rigidbody>().velocity = root.transform.right * Mathf.Sin(Mathf.Deg2Rad * (cnt+i*90)) *30 + root.transform.forward*Mathf.Cos(Mathf.Deg2Rad * (cnt + i*90)) *30 + root.transform.up * -10;
                cnt++;
            }
            m_delta_cnt = 0;
        }
    }

    public override void upgrade()
    {
        switch (m_level)
        {
            case 0:
                m_action_rate[1] = 1.5f;
                m_ability_power[1] = 50;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_action_rate[1] = 1.0f;
                m_ability_power[1] = 75;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_action_rate[1] = 0.5f;
                m_ability_power[1] = 100;
                m_upgrade_cost = 150;
                break;
            case 3:
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0] + "→" + m_ability_power[1]);
        m_upgrade_text.Add("間隔:" + m_action_rate[0] + "秒" + "→" + m_action_rate[1] + "秒");
    }
}

public class StopBuilding:BuildingClass
{
     
    public StopBuilding()
    {
        m_name = "通行止め";
        m_action_rate[0] = 6.84f;
        m_upgrade_cost = 50;
        m_delta_cnt = 0;
        
    }

    public override void ability(GameObject root)
    {
        if (GameMasterBehaviour.interval_wall > 3 && GameMasterBehaviour.interval_wall < 4f)
        {
            Debug.Log(root.transform.GetChild(0).position.y);
            if (root.transform.GetChild(0).localPosition.y > 0)
                for (int i = 0; i < 4; i++)
                    root.transform.GetChild(i).Translate(root.transform.up*-2,Space.World);
            else
                for (int i = 0; i <4;i++)
                    root.transform.GetChild(i).localPosition = new Vector3(root.transform.GetChild(i).localPosition.x, 0, root.transform.GetChild(i).localPosition.z);

        }
            if (GameMasterBehaviour.interval_wall >= m_action_rate[0])
        {
            if (root.transform.GetChild(0).localPosition.y < 1)
                for (int i = 0; i < 4; i++)
                    root.transform.GetChild(i).Translate(root.transform.up*2,Space.World);
            else
                for (int i = 0; i < 4;i++)
                    root.transform.GetChild(i).localPosition = new Vector3(root.transform.GetChild(i).localPosition.x, 1, root.transform.GetChild(i).localPosition.z);
        }
    }

    public override void trigger_enter(Collider col)
    {

    }

    public override void upgrade()
    {
        switch (m_level)
        {
            case 0:
                m_action_rate[1] = 7f;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_action_rate[1] = 5.0f;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_action_rate[1] = 3f;
                m_upgrade_cost = 150;
                break;
            case 3:
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("間隔:" + m_action_rate[0] + "秒" + "→" + m_action_rate[1] + "秒");
    }
}