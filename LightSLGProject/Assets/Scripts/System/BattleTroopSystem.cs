/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-13-17:39:32
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleTroopSystem : IBattleSystem<BattleManager>
{
    private Transform _SoldierParent;
    private List<Troop> _Troops = new List<Troop>();
    private List<Troop> _WaitTroops = new List<Troop>();
    private int _CreateCountPerFrame = 5;
    private BuildingVOProxy _BuildingProxy;
    private MapSoldierInfo _WaitForUseMapSoldierInfos = null;
    private MapSoldierInfo _WatiForUseMapSoldierInfoLast = null;
    private MapSoldierInfo _CurrentUsedMapSoldierInfos = null;

    public BattleTroopSystem(IBattleManager mgr) : base(mgr)
    {
    }
    public override void Initialize()
    {
        _SoldierParent = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_SOLDIERS_PARENT_NAME).transform;
        _BuildingProxy = facade.RetrieveProxy(BuildingVOProxy.NAME) as BuildingVOProxy;
    }
    public override void Update()
    {
        if (battleManager.isBattleOver) return;
        int total;
        AStarPathFindingAgent agent;
        System.Random random = new System.Random();
        //出兵
        for (int i = 0; i < _Troops.Count; i++)
        {
            Troop troop = _Troops[i];
            total = troop.start.soldierNum - _CreateCountPerFrame < 0 ? troop.start.soldierNum : _CreateCountPerFrame;
            total = troop.amount > total ? total : troop.amount;
            troop.start.soldierNum -= total;
            troop.amount -= total;
            _Troops[i] = troop;

            for (int j = 0; j < total; j++)
            {
                //创建指定数量的兵
                GameObject soldier = PoolManager.Instance.GetObject("Soldier", LoadAssetType.Normal, _SoldierParent);
                soldier.transform.position = troop.start.postion + Vector3.left * random.Next(-100,100) / 200f + Vector3.forward * random.Next(-100,100) / 200f;
                agent = soldier.GetComponent<AStarPathFindingAgent>();
                agent.SetTarget(troop.end.postion);
                agent.StopDistance = troop.end.radius + 0.5f;
                agent.AddCompleteListener(() =>
                {
                    //士兵到达处理
                    SoldierArrivedHandler(troop, soldier);
                    //TODO 处理占领，失败，胜利
                    if(troop.soldiers.Count <= 0 && troop.amount <= 0) _WaitTroops.Add(troop); 
                });
                troop.soldiers.Add(soldier);
            }    
        }

        //移除
        for(int i = _Troops.Count - 1; i >= 0; i--)
        {
            if (_Troops[i].amount <= 0)
            {
                _Troops.RemoveAt(i);
            }
        }

        //处理士兵链表
        if(_CurrentUsedMapSoldierInfos != null)
        {
            if (_WaitForUseMapSoldierInfos == null)
            {
                _WaitForUseMapSoldierInfos = _CurrentUsedMapSoldierInfos;
                _WatiForUseMapSoldierInfoLast = _CurrentUsedMapSoldierInfos;
            }
            else
            {
                _WatiForUseMapSoldierInfoLast.nextNode = _CurrentUsedMapSoldierInfos;
                MapSoldierInfo nextSoldior = _CurrentUsedMapSoldierInfos;
                while (nextSoldior.nextNode != null)
                {
                    nextSoldior = nextSoldior.nextNode;
                }
                _WatiForUseMapSoldierInfoLast = nextSoldior;
            }
        }


        _CurrentUsedMapSoldierInfos = null;
        MapSoldierInfo nextNode = null;
        //发送小地图士兵位置信息
        for (int i = 0; i < _Troops.Count; i++)
        {
            for (int j = 0; j < _Troops[i].soldiers.Count; j++)
            {
                if(nextNode != null)
                {
                    nextNode.nextNode = GetMapSoldierInfo();
                    nextNode = nextNode.nextNode;
                }
                else
                {
                    nextNode = GetMapSoldierInfo();
                    _CurrentUsedMapSoldierInfos = nextNode;
                }

                nextNode.coloerIndex = _Troops[i].starter.colorIndex;
                nextNode.position = _Troops[i].soldiers[j].transform.localPosition;
            }
        }

        if(_CurrentUsedMapSoldierInfos != null)
        {
            facade.SendNotification(GlobalSetting.Msg_MapUpdateSoldiersPositon, _CurrentUsedMapSoldierInfos);
        }
    }

    private void SoldierArrivedHandler(Troop troop, GameObject soldier)
    {
        if(!battleManager.isBattleOver)
        {
            _BuildingProxy.ReceiveSoldier(troop.end, troop.starter, UnityEngine.Time.time);
            troop.soldiers.Remove(soldier);
        }

        PoolManager.Instance.HideObjet(soldier);
    }

    public override void Release()
    {
        base.Release();

        _WaitTroops.Clear();
        _Troops.Clear();
        _WaitForUseMapSoldierInfos = null;
        _CurrentUsedMapSoldierInfos = null;
        _WatiForUseMapSoldierInfoLast = null;
    }
    //创建军队
    public void CreateTroop(MainBaseVO start, MainBaseVO end, int amount)
    {
        if (start != null && start.soldierNum > 0 && end != null && amount > 0)
        {
            Troop troop = default;
            if (_WaitTroops.Count == 0)
            {
                troop = new Troop();
                troop.soldiers = new List<GameObject>();
            }
            else
            {
                troop = _WaitTroops[0];
                _WaitTroops.RemoveAt(0);
                troop.soldiers.Clear();
            }

            troop.start = start;
            troop.starter = start.ower;
            troop.end = end;
            troop.amount = amount;

            _Troops.Add(troop);

        }
    }

    private MapSoldierInfo GetMapSoldierInfo()
    {
        MapSoldierInfo mapSoldierInfo = null;
        if (_WaitForUseMapSoldierInfos == null)
        {
            mapSoldierInfo = new MapSoldierInfo();
        }
        else
        {
            mapSoldierInfo = _WaitForUseMapSoldierInfos;
            _WaitForUseMapSoldierInfos = _WaitForUseMapSoldierInfos.nextNode;
            if(_WaitForUseMapSoldierInfos == null)
            {
                _WatiForUseMapSoldierInfoLast = null;
            }
            mapSoldierInfo.nextNode = null;
        }

        return mapSoldierInfo;
    }

    private struct Troop
    {
        public MainBaseVO start;
        public PlayerVO starter;
        public MainBaseVO end;
        public int amount;
        public List<GameObject> soldiers;
    }
}

public class MapSoldierInfo
{
    public int coloerIndex;
    public Vector3 position;
    public MapSoldierInfo nextNode;
}

