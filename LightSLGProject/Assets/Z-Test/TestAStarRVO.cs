/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-07-11:53:09
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using Pathfinding;
using Pathfinding.Examples;
using Pathfinding.RVO;
using UnityEngine;

public class TestAStarRVO : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    // Start is called before the first frame update
    void Start()
    {
        CreatePathGraphic();

        AStarPathFindingAgent[] list = GetComponentsInChildren<AStarPathFindingAgent>();

        for (int i = 0; i < list.Length; i++)
        {
            if (i%2 == 0)
                list[i].SetTarget(target.position);
            else
                list[i].SetTarget(target2.position);
        }
    }

    void CreatePathGraphic()
    {
        // This holds all graph data
        AstarData data = AstarPath.active.data;

        // This creates a Grid Graph
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;

        // Setup a grid graph with some values
        int width = 1000;
        int depth = 1000;
        float nodeSize = 1;     

        // Updates internal size from the above values
        gg.SetDimensions(width, depth, nodeSize);

        gg.center = new Vector3(0, 0, 0);
        //gg.collision.mask = LayerMask.GetMask("Obstacles");
        gg.collision.heightMask = LayerMask.GetMask("Ground");

        // Scans all graphs
        AstarPath.active.Scan();

        AstarPath.active.AddWorkItem(new AstarWorkItem(ctx =>
        {
            var ggd = AstarPath.active.data.gridGraph;
            for (int i = 995; i < 1000; i++)
            {
                for (int j = 995; j < 1000; j++)
                {
                    var node = ggd.GetNode(j, i);
                    node.Walkable = false;
                    ggd.CalculateConnectionsForCellAndNeighbours(j, i);
                }
            }
        }));
    }
}
