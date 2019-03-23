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
    private List<Troop> troops = new List<Troop>();
    private List<Troop> waitTroops = new List<Troop>();
    private int CreateCountPerFrame = 5;
    public BattleTroopSystem(IBattleManager mgr) : base(mgr)
    {
    }

    public override void Initialize()
    {
        _SoldierParent = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_SOLDIERS_PARENT_NAME).transform;
    }

    public override void Update()
    {
        int total;
        AStarPathFindingAgent agent;
        System.Random random = new System.Random();
        //出兵
        for (int i = 0; i < troops.Count; i++)
        {
            Troop troop = troops[i];
            total = troop.start.soldierNum - CreateCountPerFrame < 0 ? troop.start.soldierNum : CreateCountPerFrame;
            total = troop.amount > total ? total : troop.amount;
            troop.start.soldierNum -= total;
            troop.amount -= total;
            troops[i] = troop;

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
                    //TODO 士兵到达处理
                    troop.end.ReceiveSoldier(troop.starter, UnityEngine.Time.time);
                    PoolManager.Instance.HideObjet(soldier);
                    //TODO 处理占领，失败，胜利
                });
            }    
        }

        //移除
        for (int i = troops.Count - 1; i >= 0; i--)
        {
            if (troops[i].amount <= 0)
            {
                waitTroops.Add(troops[i]);
                troops.RemoveAt(i);
            }
        }
    }

    //创建军队
    public void CreateTroop(MainBaseVO start, MainBaseVO end, int amount)
    {
        if (start != null && start.soldierNum > 0 && end != null && amount > 0)
        {
            Troop troop = default;
            if (waitTroops.Count == 0)
            {
                troop = new Troop();
            }
            else
            {
                troop = waitTroops[0];
                waitTroops.RemoveAt(0);
            }

            troop.start = start;
            troop.starter = start.ower;
            troop.end = end;
            troop.amount = amount;

            troops.Add(troop);
        }
    }

    private void SoldierArriveAtTarget(MainBaseVO mainBaseVO)
    {
        //TODO占领
    }

    private struct Troop
    {
        public MainBaseVO start;
        public PlayerVO starter;
        public MainBaseVO end;
        public int amount;
    }
}

