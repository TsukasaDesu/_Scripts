using UnityEngine;
using System.Collections;

public class SlantingBombBehaviour : MonoBehaviour {

    public float power = 0;
    public GameObject effect_obj;

	// Use this for initialization
	void Start () {
        effect_obj = Resources.Load<GameObject>("Effect/BombEffect_obj");
	}
	
	// Update is called once per frame
	void Update () {	
	}

    void OnCollisionEnter(Collision col)
    {

        if (col.collider.gameObject.name == "Plane")
        {
            GameObject clone = Instantiate(effect_obj, transform.position, Quaternion.identity) as GameObject;
            clone.GetComponent<BombEffectBehaviour>().power = power;
            Destroy(gameObject);
        }
    }
}
