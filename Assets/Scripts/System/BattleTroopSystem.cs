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
                });
            }    
        }

        //移除
        for (int i = _Troops.Count - 1; i >= 0; i--)
        {
            if (_Troops[i].amount <= 0)
            {
                _WaitTroops.Add(_Troops[i]);
                _Troops.RemoveAt(i);
            }
        }
    }

    private void SoldierArrivedHandler(Troop troop, GameObject soldier)
    {
        if(!battleManager.isBattleOver)
        {
            _BuildingProxy.ReceiveSoldier(troop.end, troop.starter, UnityEngine.Time.time);
        }
        
        PoolManager.Instance.HideObjet(soldier);
    }

    public override void Release()
    {
        base.Release();

        _WaitTroops.Clear();
        _Troops.Clear();
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
            }
            else
            {
                troop = _WaitTroops[0];
                _WaitTroops.RemoveAt(0);
            }

            troop.start = start;
            troop.starter = start.ower;
            troop.end = end;
            troop.amount = amount;

            _Troops.Add(troop);

        }
    }

    private struct Troop
    {
        public MainBaseVO start;
        public PlayerVO starter;
        public MainBaseVO end;
        public int amount;
    }
}

