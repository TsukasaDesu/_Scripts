using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SlantingBulletBehaviour : MonoBehaviour {

    float destroy_time;
    public float power;
    float cnt;
    GameObject HitEffect;

    void Start () {
        HitEffect = Resources.Load<GameObject>("Effect/HitEffect");
        destroy_time = 1;
	}
	
	void Update () {
        cnt += Time.deltaTime;
        if (cnt > destroy_time) Destroy(gameObject);
	}

    public void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Building")
        {
            destroy_time += 1f;
            gameObject.GetComponent<Rigidbody>().velocity *= 2;
        }
        if (col.collider.gameObject.tag == "Enemy")
        {
            destroy_time += 0.5f;
            gameObject.GetComponent<Rigidbody>().velocity *= 1.5f;
            GameObject clone = Instantiate(HitEffect, col.contacts[0].point, Quaternion.Euler(col.collider.gameObject.transform.forward)) as GameObject;
            Destroy(clone, 2);
            ExecuteEvents.Execute<IRecieveMessage>(
                            target: col.collider.gameObject,
                            eventData: null,
                            functor: (recievetarget, y) => recievetarget.OnRecieve(PlayerBehaviour.power)
                            );
        }
    }
}
