using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BulletBehaviour : MonoBehaviour {

    GameObject HitEffect;

	// Use this for initialization
	void Start () {
        HitEffect = Resources.Load<GameObject>("Effect/HitEffect");        	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnCollisionEnter(Collision col)
    {

        if(col.collider.gameObject.tag == "Enemy")
        {
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
