/*
 *	Author:	贾树永
 *	CreateTime:	2019-02-25-17:15:15
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class MainBase : Building
{
    private MainBaseVO m_Data;
    private int m_CurrentRadius;
    public Transform m_EdgeF;
    public Transform m_EdgeR;
    public Transform m_EdgeB;
    public Transform m_EdgeL;

    public void InitMainBase(MainBaseVO vO)
    {
        this.m_Data = vO;
        m_CurrentRadius = vO.radius;
    }
    public void UpdateArea()
    {
        if (m_EdgeF && m_EdgeR && m_EdgeB && m_EdgeL)
        {
            for (int i = 0; i < m_Data.radius - m_CurrentRadius; i++)
            {
                Vector3 newPosition = new Vector3(-m_EdgeF.childCount - 1, -m_EdgeF.childCount - 1, 0);
                ChangeEdge(m_EdgeF, newPosition, Vector3.forward);
                ChangeEdge(m_EdgeB, newPosition, Vector3.back);
                ChangeEdge(m_EdgeR, newPosition, Vector3.right);
                ChangeEdge(m_EdgeL, newPosition, Vector3.left);
            }
            m_CurrentRadius = m_Data.radius;
        }
    }

    private void ChangeEdge(Transform edge, Vector3 newPosition, Vector3 moveVector)
    {
        edge.localPosition += moveVector;
        Transform child = edge.GetChild(edge.childCount - 1);
        child = Instantiate<Transform>(child);
        child.SetParent(edge);
        child.localRotation = Quaternion.identity;
        child.localPosition = newPosition;
    }
}