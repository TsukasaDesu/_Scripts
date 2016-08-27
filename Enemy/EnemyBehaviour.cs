using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour,IRecieveMessage {
        
    public enum Type { Normal,Big,Small};
    public Type type;
    public bool moveflg;
    EnemyClass enemyclass;
    float cnt;
    GameObject hp_gage;

	// Use this for initialization
	void Start () {
        switch(type)
        {
            case Type.Normal:
                enemyclass = new Normal();
                break;
            case Type.Big:
                enemyclass = new Big();
                break;
            case Type.Small:
                enemyclass = new Small();
                break;
        }

        enemyclass.Init(gameObject);

        hp_gage = transform.FindChild("Canvas").FindChild("Slider").gameObject;
        hp_gage.GetComponent<Slider>().maxValue = enemyclass.m_hp;
        hp_gage.GetComponent<Slider>().value = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //if (!moveflg) return;
        hp_gage.transform.LookAt(PlayerBehaviour.root.transform);

        if (transform.position.y < -10)
        {
            Dead();
        }

        cnt += Time.deltaTime;
        if(cnt > 3)
            enemyclass.Move(gameObject);
	}

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Plane")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            enemyclass.m_nav.enabled = true;
            moveflg = true;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            PlayerBehaviour.hp-=0.2f;
        }
    }

    public void OnRecieve(float power)
    {
        enemyclass.m_hp -= power;
        hp_gage.GetComponent<Slider>().value += power;
        if (enemyclass.m_hp <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
            GameMasterBehaviour.now_enmey_num--;
            PlayerBehaviour.money += enemyclass.m_drop_money;
            //Instantiate()//Effect

            Destroy(gameObject);
    }

}
