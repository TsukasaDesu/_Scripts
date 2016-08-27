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
    public int m_drop_exp;
    public int m_drop_money;
    public NavMeshAgent m_nav;
    public float m_gage_height;

    public EnemyClass()
    { 
        m_hp = 100;
        m_attack = 10;
        m_level = 0;
        m_drop_exp = m_drop_money = 1;
        m_gage_height = 1;
    }

    public void Init(GameObject root)
    {
        //m_body = root.transform.FindChild("body").gameObject;
        m_nav = root.GetComponent<NavMeshAgent>();
        m_nav.enabled = false;
    }

    public virtual void Move(GameObject root)
    {
        //m_body.transform.localPosition = Vector3.zero;
        if (m_nav.pathStatus != NavMeshPathStatus.PathInvalid)
            m_nav.SetDestination(PlayerBehaviour.root.transform.position);
    }

}

public class CubeMan:EnemyClass
{

    public override void Move(GameObject root)
    {
        base.Move(root);
    }
}
