using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class EnemyClass : MonoBehaviour {

    public GameObject m_body;
    public float m_hp;
    public float m_attack;
    public int m_level;
    public GameObject m_target;
    public float m_speed;
    public int m_drop_exp;
    public int m_drop_money;
    public NavMeshAgent m_nav;
    public float m_gage_height;
    public float[] m_hp_max = new float[10];

    public EnemyClass()
    { 
        m_hp = 100;
        m_attack = 10;
        m_level = 0;
        m_speed = 10;
        m_drop_exp = m_drop_money = 1;
        m_gage_height = 1;

    }

    public void Init(GameObject root)
    {
        //m_body = root.transform.FindChild("body").gameObject;

        m_hp = m_hp_max[GameMasterBehaviour.wave];

        if (Random.Range(0, 10) < 1)
        {
            float rand = Random.Range(1.5f, 3);
            m_hp *= rand;
            m_speed *= (rand / 1.5f <= 1) ? 1 : rand / 1.5f;
            root.transform.localScale *= rand;
        }

        m_nav = root.GetComponent<NavMeshAgent>();
        m_nav.speed = m_speed;
        m_nav.enabled = false;
    }

    public virtual void Move(GameObject root)
    {
        //m_body.transform.localPosition = Vector3.zero;
        if (m_nav.pathStatus != NavMeshPathStatus.PathInvalid)
            m_nav.SetDestination(PlayerBehaviour.root.transform.position);
    }

}

public class Normal:EnemyClass
{
    public Normal()
    {
        m_hp_max[0] = 100;
        m_hp_max[1] = 200;
        m_hp_max[2] = 300;
        m_hp_max[3] = 400;
        m_hp_max[4] = 500;
        m_hp_max[5] = 600;
        m_hp_max[6] = 700;
        m_hp_max[7] = 800;
        m_hp_max[8] = 900;
        m_hp_max[9] = 1000;

    }



    public override void Move(GameObject root)
    {
        base.Move(root);
    }
}

public class Small:EnemyClass
{
    public Small()
    {
        m_hp = 50;
        m_speed = 20;

        m_hp_max[0] = 50;
        m_hp_max[1] = 80;
        m_hp_max[2] = 110;
        m_hp_max[3] = 150;
        m_hp_max[4] = 210;
        m_hp_max[5] = 250;
        m_hp_max[6] = 300;
        m_hp_max[7] = 350;
        m_hp_max[8] = 400;
        m_hp_max[9] = 500;
    }
}

public class Big:EnemyClass
{
    public Big()
    {
        m_hp = 300;
        m_attack = 30;
        m_speed = 7;

        m_hp_max[0] = 300;
        m_hp_max[1] = 600;
        m_hp_max[2] = 900;
        m_hp_max[3] = 1200;
        m_hp_max[4] = 1500;
        m_hp_max[5] = 1900;
        m_hp_max[6] = 2400;
        m_hp_max[7] = 3000;
        m_hp_max[8] = 3800;
        m_hp_max[9] = 5000;
    }

}
