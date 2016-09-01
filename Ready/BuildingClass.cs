using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public abstract class BuildingClass : MonoBehaviour
{

    public GameObject m_root;
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

    public void Init(GameObject root)
    {
        m_root = root;
    }

    public virtual void ability()
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

public class MoneyBuilding : BuildingClass
{

    public MoneyBuilding()
    {
        m_name = "増金施設";
        m_ability_power[0] = 1;
        m_action_rate[0] = 4;
        m_upgrade_text = new List<string>();
    }

    public override void ability()
    {
        m_delta_cnt += Time.deltaTime;

        if (m_delta_cnt >= m_action_rate[0])
        {
            PlayerBehaviour.money += m_ability_power[0];
            m_delta_cnt = 0;
        }
    }

    public override void upgrade()
    {
        switch (m_level)
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
                m_upgrade_cost = 0;
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("お金:" + m_ability_power[0] + "円" + "→" + m_ability_power[1] + "円");
        m_upgrade_text.Add("間隔:" + m_action_rate[0] + "秒" + "→" + m_action_rate[1] + "秒");
    }

}

public class AutoGunBuilding : BuildingClass
{
    List<GameObject> nearEnemys;
    GameObject nearestEnemy;
    GameObject hiteffect;

    public AutoGunBuilding()
    {
        m_name = "自動銃";
        m_range[0] = 20;
        m_ability_power[0] = 20;
        nearEnemys = new List<GameObject>();
        hiteffect = Resources.Load<GameObject>("Effect/HitEffect");
        m_upgrade_text = new List<string>();
    }

    public override void ability()
    {
        nearEnemys = nearEnemys.Where(gmObj => gmObj != null).ToList();

        nearestEnemy = nearEnemys.Count() < 1 ? null : nearEnemys.OrderBy<GameObject, float>(gmObj => Vector3.Distance(gmObj.transform.position, m_root.transform.position)).First();

        if (nearestEnemy != null)
        {
            m_root.transform.GetChild(1).LookAt(nearestEnemy.transform.position);
            m_delta_cnt += Time.deltaTime;
            if (m_delta_cnt > m_action_rate[0])
            {
                ExecuteEvents.Execute<IRecieveMessage>(
                           target: nearestEnemy,
                           eventData: null,
                           functor: (recievetarget, y) => recievetarget.OnRecieve(m_ability_power[0])
                           );
                GameObject clone = Instantiate(hiteffect, nearestEnemy.transform.position - m_root.gameObject.transform.forward, Quaternion.Euler(m_root.transform.forward)) as GameObject;
                Destroy(clone, 2);
                m_delta_cnt = 0;
            }
        }
    }

    public override void trigger_enter(Collider col)
    {
        if (col.tag == "Enemy")
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
        switch (m_level)
        {
            case 0:
                m_action_rate[1] = 1.5f;
                m_ability_power[1] = 25;
                m_range[1] = 30;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_action_rate[1] = 1.0f;
                m_ability_power[1] = 30;
                m_range[1] = 40;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_action_rate[1] = 0.5f;
                m_ability_power[1] = 35;
                m_range[1] = 50;
                m_upgrade_cost = 150;
                break;
            case 3:
                m_upgrade_cost = 0;
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0] + "→" + m_ability_power[1]);
        m_upgrade_text.Add("間隔:" + m_action_rate[0] + "秒" + "→" + m_action_rate[1] + "秒");
        m_upgrade_text.Add("射程:" + m_range[0] + "m" + "→" + m_range[1] + "m");
    }

}

public class SlantingShotBuilding : BuildingClass
{
    GameObject bullet_src;
    float cnt;

    public SlantingShotBuilding()
    {
        m_name = "斜め撃ち";
        m_ability_power[0] = 50;
        m_action_rate[0] = 2;
        m_upgrade_cost = 50;
        bullet_src = Resources.Load<GameObject>("Bullet/SlantingBullet");
        cnt = 0;
    }

    public override void ability()
    {
        m_root.transform.GetChild(0).Rotate(0, m_level * 5 + 1, 0);
        m_delta_cnt += Time.deltaTime;
        if (m_delta_cnt > m_action_rate[0])
        {
            for (int i = 0; i < m_level + 1; i++)
            {
                GameObject bul = Instantiate(bullet_src, m_root.transform.position + m_root.transform.right * Mathf.Sin(Mathf.Deg2Rad * i * 90) * 2 + m_root.transform.forward * Mathf.Cos(Mathf.Deg2Rad * i * 90) * 2, Quaternion.identity) as GameObject;
                bul.GetComponent<SlantingBulletBehaviour>().power = m_ability_power[0];
                bul.transform.parent = m_root.transform;
                bul.GetComponent<Rigidbody>().velocity = m_root.transform.right * Mathf.Sin(Mathf.Deg2Rad * (cnt + i * 90)) * 30 + m_root.transform.forward * Mathf.Cos(Mathf.Deg2Rad * (cnt + i * 90)) * 30 + m_root.transform.up * -10;
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
                m_ability_power[1] = 75;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_action_rate[1] = 1.0f;
                m_ability_power[1] = 100;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_action_rate[1] = 0.5f;
                m_ability_power[1] = 125;
                m_upgrade_cost = 150;
                break;
            case 3:
                m_upgrade_cost = 0;
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0] + "→" + m_ability_power[1]);
        m_upgrade_text.Add("間隔:" + m_action_rate[0] + "秒" + "→" + m_action_rate[1] + "秒");
    }
}

public class StopBuilding : BuildingClass
{

    public StopBuilding()
    {
        m_name = "通行止め";
        m_action_rate[0] = 6.84f;
        m_upgrade_cost = 0;
        m_delta_cnt = 0;

    }

    public override void ability()
    {
        if (GameMasterBehaviour.interval_wall > 3 && GameMasterBehaviour.interval_wall < 4f)
        {
            if (m_root.transform.GetChild(0).localPosition.y > 0)
                for (int i = 0; i < 4; i++)
                    m_root.transform.GetChild(i).Translate(m_root.transform.up * -2, Space.World);
            else
                for (int i = 0; i < 4; i++)
                    m_root.transform.GetChild(i).localPosition = new Vector3(m_root.transform.GetChild(i).localPosition.x, 0, m_root.transform.GetChild(i).localPosition.z);

        }
        if (GameMasterBehaviour.interval_wall >= m_action_rate[0])
        {
            if (m_root.transform.GetChild(0).localPosition.y < 1)
                for (int i = 0; i < 4; i++)
                    m_root.transform.GetChild(i).Translate(m_root.transform.up * 2, Space.World);
            else
                for (int i = 0; i < 4; i++)
                    m_root.transform.GetChild(i).localPosition = new Vector3(m_root.transform.GetChild(i).localPosition.x, 1, m_root.transform.GetChild(i).localPosition.z);
        }
    }


    public override void upgrade()
    {
    }
}

public class SlantingBombBuilding : BuildingClass
{
    GameObject bomb_src;
    bool shot_flg;

    public SlantingBombBuilding()
    {
        m_name = "斜め爆弾";
        m_ability_power[0] = 75;
        m_action_rate[0] = 2;
        m_upgrade_cost = 50;
        bomb_src = Resources.Load<GameObject>("Bullet/SlantingBomb");
        shot_flg = false;
    }

    public override void ability()
    {
        if (GameMasterBehaviour.interval_wall > 0f && GameMasterBehaviour.interval_wall < 1f)
        {
            if (shot_flg) return;
            for (int i = 0; i < 4; i++)
            {
                GameObject clone = Instantiate(bomb_src, m_root.transform.position + m_root.transform.forward * ((i < 2) ? -2 : 2) + m_root.transform.right * ((i % 2 == 0) ? -2 : 2) + m_root.transform.up * 8, Quaternion.identity) as GameObject;
                clone.GetComponent<Rigidbody>().velocity = m_root.transform.forward * ((i < 2) ? -7.8f : 7.8f) + m_root.transform.right * ((i % 2 == 0) ? -7.8f : 7.8f)+m_root.transform.up*15f;
                clone.GetComponent<SlantingBombBehaviour>().power = m_ability_power[0];
            }
            shot_flg = true;
        }

        if (GameMasterBehaviour.interval_wall > 3f && GameMasterBehaviour.interval_wall < 4f)
        {
            shot_flg = false;
        }

    }

    public override void upgrade()
    {
        switch (m_level)
        {
            case 0:
                m_ability_power[1] = 100;
                m_upgrade_cost = 50;
                break;
            case 1:
                m_ability_power[1] = 125;
                m_upgrade_cost = 100;
                break;
            case 2:
                m_ability_power[1] = 150;
                m_upgrade_cost = 150;
                break;
            case 3:
                m_upgrade_cost = 0;
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0]*2 + "→" + m_ability_power[1]*2);
    }
}

public class ResponceShotBuilding:BuildingClass
{
    GameObject bullet_src;
    Vector3 rotate_rand;
    bool coolflg;
    bool hitflg;
    int fire_cnt;
    float local_scale;
    public ResponceShotBuilding()
    {
        m_name = "反応撃ち";
        m_ability_power[0] = 30;
        m_upgrade_cost = 1000;
        bullet_src = Resources.Load<GameObject>("Bullet/ResponceBomb");
        rotate_rand = new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3));
        coolflg = false;
        fire_cnt = -1;
        local_scale = 3;
    }

    public override void ability()
    {

        if (fire_cnt >= 0)
        {
            float rand = Random.Range(0, 360);
            GameObject clone = Instantiate(bullet_src, m_root.transform.position + m_root.transform.right * Mathf.Sin(Mathf.Deg2Rad * rand) * 2 + m_root.transform.forward * Mathf.Cos(Mathf.Deg2Rad * rand) * 2 + m_root.transform.up * 8, Quaternion.identity) as GameObject;
            clone.GetComponent<SlantingBombBehaviour>().power = m_ability_power[0];
            clone.GetComponent<Rigidbody>().velocity = m_root.transform.right * Mathf.Sin(Mathf.Deg2Rad * rand) * Random.Range(10, 30) + m_root.transform.forward * Mathf.Cos(Mathf.Deg2Rad * rand) * Random.Range(10, 30) + m_root.transform.up * Random.Range(10, 30);
            fire_cnt++;
            if (fire_cnt > 100 + 20 * m_level)
            {
                fire_cnt = -1;
                m_delta_cnt = 0;
                hitflg = false;
            }
        }

        if(fire_cnt != -1) return;

        if (!hitflg)
        m_delta_cnt += Time.deltaTime;
    
        if (m_delta_cnt >= 10 && hitflg)
        {
            fire_cnt = 0;
        }

        foreach(Transform r in m_root.transform.GetChild(2))
        {
            r.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f - m_delta_cnt / 20, 0.5f - m_delta_cnt / 20));
        }
        m_root.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.5f, 0.5f - m_delta_cnt / 20, 0.5f - m_delta_cnt / 20));
        m_root.transform.GetChild(0).Rotate(0,m_delta_cnt*2f,0,Space.World);
        local_scale = 3 + m_delta_cnt / 5;
        local_scale = Mathf.Clamp(local_scale, 3, 5);
        m_root.transform.GetChild(0).localScale = new Vector3(local_scale,local_scale,local_scale); ;
    }

    public override void trigger_enter(Collider col)
    { 
        if(col.tag =="bullet")
        {
            if(m_delta_cnt >= 10)
                hitflg = true;
        }
    }

    public override void upgrade()
    {
        switch (m_level)
        {
            case 0:
                m_ability_power[1] = 60;
                m_upgrade_cost = 500;
                break;
            case 1:
                m_ability_power[1] = 100;
                m_upgrade_cost = 1000;
                break;
            case 2:
                m_ability_power[1] = 240;
                m_upgrade_cost = 150;
                break;
            case 3:
                m_upgrade_cost = 0;
                break;
        }
        if (m_upgrade_text.Count > 0)
            m_upgrade_text.Clear();
        m_upgrade_text.Add("レベル:" + m_level + "→" + (m_level + 1));
        m_upgrade_text.Add("攻撃力:" + m_ability_power[0]*2 + "→" + m_ability_power[1]*2);
    }

}