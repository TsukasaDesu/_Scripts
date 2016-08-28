using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BombEffectBehaviour : MonoBehaviour {

    GameObject effect;
    public float power;

	// Use this for initialization
	void Start () {
        effect = Resources.Load<GameObject>("Effect/BombEffect");
        GameObject clone = Instantiate(effect, gameObject.transform.position, Quaternion.identity) as GameObject;

        Vector3 center = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Collider[] cols = Physics.OverlapSphere(center, 10);
        foreach (Collider obj in cols)
        {

            if (obj.gameObject.tag == "Enemy")
            {
                ExecuteEvents.Execute<IRecieveMessage>(
                        target: obj.gameObject,
                        eventData: null,
                        functor: (recievetarget, y) => recievetarget.OnRecieve(power)
                        );
            }
        }

        Destroy(clone,2);
        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
