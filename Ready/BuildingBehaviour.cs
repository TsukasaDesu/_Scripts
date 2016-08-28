using UnityEngine;
using System.Collections;

public class BuildingBehaviour : MonoBehaviour {

    public enum Builiding {Money,AutoGun,SlantingShot,Stop,SlantingBomb,ResponceShot};
    public Builiding building;


    public BuildingClass building_class;

	// Use this for initialization
	void Start () {
        switch(building)
        {
            case Builiding.Money:
                building_class = new MoneyBuilding();
                break;
            case Builiding.AutoGun:
                building_class = new AutoGunBuilding();
                break;
            case Builiding.SlantingShot:
                building_class = new SlantingShotBuilding();
                break;
            case Builiding.Stop:
                building_class = new StopBuilding();
                break;
            case Builiding.SlantingBomb:
                building_class = new SlantingBombBuilding();
                break;
            case Builiding.ResponceShot:
                building_class = new ResponceShotBuilding();
                break;
        }

        building_class.Init(gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
        //switch(building)
        //{
        //    case Builiding.Money:
        //        build.ability();
        //        break;
        //}
        building_class.ability();
	}

    void OnTriggerEnter(Collider col)
    {
        building_class.trigger_enter(col);
    }

    void OnTriggerExit(Collider col)
    {
        building_class.trigger_out(col);
    }
}
