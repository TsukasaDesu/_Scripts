using UnityEngine;
using System.Collections;

public abstract class ClickObjClass : MonoBehaviour {

    public string m_title;
    public int m_cost;
    public string m_explain;
    public int m_buy_cnt;
    public float m_height;//建築物専用、Y座標を決める
    public GameObject m_building;

    public ClickObjClass()
    {
        m_title = "";
        m_cost = 0;
        m_explain = "";
        m_buy_cnt = 0;
        m_height = 0;
        m_building = null;
    }

    

    public virtual void Clicked()
    {
        m_buy_cnt++;
        PlayerBehaviour.money -= m_cost;
    }

}

public class HealPlayer : ClickObjClass
{
    public HealPlayer()
    {
        m_title = "体力回復";
        m_cost = 10;
        m_explain = "体力が10回復する";
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost += 10;
        PlayerBehaviour.hp =(PlayerBehaviour.hp > 90)?100:PlayerBehaviour.hp+10;
    }
}


public class PowerUP:ClickObjClass
{
    public PowerUP()
    {
        m_title = "攻撃力UP";
        m_cost = 20;
        m_explain = "攻撃力:" + PlayerBehaviour.power + "→" + (PlayerBehaviour.power + 10);
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost = 20 + m_buy_cnt * 10;
        PlayerBehaviour.power += 10;
        m_explain = "攻撃力:" + PlayerBehaviour.power + "→" + (PlayerBehaviour.power + 10);
    }
}

public class BuildMoney:ClickObjClass
{
    public BuildMoney()
    {
        m_title = "増金施設";
        m_cost = 30;
        m_explain = "４秒毎に所持金が１円増える施設";
        m_building = Resources.Load<GameObject>("Building/MoneyBuilding");
        m_height = 6;
    }
    
    public override void Clicked()
    {
        base.Clicked();
        m_cost += m_buy_cnt * 10;

    }
}

public class BuildAutoGun:ClickObjClass
{
    public BuildAutoGun()
    {
        m_title = "自動銃";
        m_cost = 20;
        m_explain = "2秒毎に範囲内の最も近い敵に30ダメージ";
        m_building = Resources.Load<GameObject>("Building/AutoGunBuilding");
        m_height = 3;
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost += m_buy_cnt * 20;
    }
}

public class BuildSlatingShot:ClickObjClass
{
    public BuildSlatingShot()
    {
        m_title = "斜め撃ち";
        m_cost = 30;
        m_explain = "２秒毎に特殊弾を斜め方向をだす";
        m_building = Resources.Load<GameObject>("Building/SlantingShotBuilding");
        m_height = 3;
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost += m_cost * 20;
    }
}

public class BuildStop:ClickObjClass
{
    public BuildStop()
    {
        m_title = "壁召喚";
        m_cost = 30;
        m_explain = "7秒毎に弾しか通れない２秒間で消える壁を召喚する";
        m_building = Resources.Load<GameObject>("Building/StopBuilding");
        m_height = 6;
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost = m_cost +10;

    }
}

public class BuildSlantingBomb:ClickObjClass
{
    public BuildSlantingBomb()
    {
        m_title = "斜め爆弾";
        m_cost = 30;
        m_explain = "7秒毎に斜め4方向に爆弾を発射する";
        m_building = Resources.Load<GameObject>("Building/SlantingBombBuilding");
        m_height = 3;
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost = m_cost + 30;
    }
}

public class BuildResponceShot:ClickObjClass
{
    public BuildResponceShot()
    {
        m_title = "反応撃ち";
        m_cost = 50;
        m_explain = "これに弾が当たるとランダムな方向に新たな弾が出る";
        m_building = Resources.Load<GameObject>("Building/ResponceShotBuilding");
        m_height = 3;
    }

    public override void Clicked()
    {
        base.Clicked();
        m_cost = m_cost + 30*m_buy_cnt;
    }
}
