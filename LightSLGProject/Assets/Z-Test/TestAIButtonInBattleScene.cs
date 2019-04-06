/*
 *	Author:	贾树永
 *	CreateTime:	2019-04-02-18:12:12
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class TestAIButtonInBattleScene : MonoBehaviour
{
    public int userID;
    public E_Building buildingType;
    public int mainbaseIndex;
    public int targetPlyerID;
    public int targetMainbaseIndex;
    // Start is called before the first frame update

    public void CreateBuilding()
    {
        PlayerVO playerVO = (GameFacade.Instance.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy).GetPlayer(userID);
        BattleManager.Instance.aiSystem._AIBuilder.Build(playerVO, buildingType, playerVO.mainBases[0]);
    }

    public void UpgradeMainbase()
    {
        PlayerVO playerVO = (GameFacade.Instance.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy).GetPlayer(userID);
        BattleManager.Instance.aiSystem._AIBuilder.UpgradeMainbase(playerVO.mainBases[mainbaseIndex]);
    }

    public void MoveTroops()
    {
        PlayerVO from = (GameFacade.Instance.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy).GetPlayer(userID);
        PlayerVO end = (GameFacade.Instance.RetrieveProxy(PlayerVOProxy.NAME) as PlayerVOProxy).GetPlayer(targetPlyerID);
        BattleManager.Instance.aiSystem._AICommander.MoveTroops(from.mainBases[mainbaseIndex], end.mainBases[targetMainbaseIndex]);
    }
}
