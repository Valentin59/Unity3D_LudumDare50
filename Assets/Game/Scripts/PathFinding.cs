using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class PathFinding : MonoBehaviour
{
    public Transform player;
    //public Transform zombie;

    public Map map;
    public PFMap pathFindingMap;

    public UnityEvent onCarteUpdatedCallback;

    private void Awake()
    {
        map.onGenerationComplete.AddListener(Initialize);

        
        //Debug.Log("Player position " + ConvertWorldPositionToArray(player.position));
        //Debug.Log("Zombie position " + ConvertWorldPositionToArray(zombie.position));
    }

    public Vector2Int ConvertWorldPositionToArray(Vector3 position)
    {
        Vector2Int convertposition = new Vector2Int(Mathf.FloorToInt(position.x / 3f + 0.5f), Mathf.FloorToInt(position.z / 3f+0.5f));

        return convertposition;
    }


    public void Initialize()
    {
        //pathFindingMap.//map
        // ------------------- Init First Time -----------------
        pathFindingMap.map = new int[map.map.GetLength(0), map.map.GetLength(1)];
        for (int y = 0; y < map.map.GetLength(1); y++)
        {
            for (int x = 0; x < map.map.GetLength(0); x++)
            {
                if (!map.map[x, y])
                {
                    pathFindingMap.map[x, y] = int.MaxValue;
                }
                else
                {
                    pathFindingMap.map[x, y] = 0;
                }
            }
        }
        // ------------------------------------------------------

        // ------------------- Pathfinding Algo -----------------


        DikjtraAlgo();


        //--------------------------------------------------------
        /*string s = "";
        foreach(var d in PathToPlayer(zombie.position))
        {
            s += d + "\n";
        }
        Debug.Log(s);*/
    }


    public int Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return dx + dy;
        /*if (dx > dy)
            return dy * diagonale + forward * (dx - dy);
        else
            return dx * diagonale + forward * (dy - dx);*/
    }

    float _timer;
    public void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > 1f)
        {
            DikjtraAlgo();
            _timer = 0f;
        }
    }


    public Vector2Int[] GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        //Deplacements possibles
        Vector2Int[] DIRS = {
            new Vector2Int(1,0), // Droite
            new Vector2Int(-1,0),// Gauche
            new Vector2Int(0,1), // Haut
            new Vector2Int(0,-1) // Bas
            //new Vector2Int(-1,-1), // Bas - Droite
            //new Vector2Int(1,-1), // Bas - Gauche
            //new Vector2Int(1, 1), // Haut - Droite
            //new Vector2Int(-1,1) // haut - Gauche
        };

        foreach (var d in DIRS)
        {
            Vector2Int n = node + d;
            if (n.x >= 0
                && n.x < pathFindingMap.map.GetLength(0)
                && n.y >= 0
                && n.y < pathFindingMap.map.GetLength(1))
            {
                neighbors.Add(n);
            }
        }

        return neighbors.ToArray();
    }



    public void DikjtraAlgo()
    {
        if (player == null)
            return;

        for (int y = 0; y < map.map.GetLength(1); y++)
        {
            for (int x = 0; x < map.map.GetLength(0); x++)
            {
                if (map.map[x, y])
                {
                    pathFindingMap.map[x, y] = 0;
                }
            }
        }


        List<Vector2Int> openSet = new List<Vector2Int>();
        HashSet<Vector2Int> closeSet = new HashSet<Vector2Int>();

        //Vector2Int startPosition = new Vector2Int(map.map.GetLength(0) / 2, map.map.GetLength(1) / 2);
        Vector2Int startPosition = ConvertWorldPositionToArray(player.position);


        openSet.Add(new Vector2Int(startPosition.x, startPosition.y));


        while (openSet.Count != 0)
        {
            Vector2Int current = openSet[0];

            openSet.Remove(current);
            closeSet.Add(current);

            foreach (Vector2Int n in GetNeighbors(current))
            {
                if (!map.map[n.x, n.y] || closeSet.Contains(n))
                    continue;

                int tmp_cost = Heuristic(current, n) + pathFindingMap.map[current.x, current.y];
                if (tmp_cost < pathFindingMap.map[n.x, n.y] || !openSet.Contains(n))
                {
                    pathFindingMap.map[n.x, n.y] = tmp_cost;
                    openSet.Add(n);
                }
            }
        }


        onCarteUpdatedCallback?.Invoke();
    }

    public Vector2Int[] PathToPlayer(Vector3 position)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int indexPlayer = ConvertWorldPositionToArray(player.position);
        Vector2Int indexEnnemy = ConvertWorldPositionToArray(position);

        Vector2Int size = new Vector2Int(pathFindingMap.map.GetLength(0),
                                            pathFindingMap.map.GetLength(1));

        int maxloop = 25;
        int loop = 0;

        if(indexEnnemy.x >= 0 && indexEnnemy.y >= 0 && indexEnnemy.x < size.x && indexEnnemy.y < size.y )
        {
            Vector2Int startPosition = indexEnnemy;
            while (startPosition != indexPlayer && loop < maxloop)
            {
                
                Vector2Int newDirection = GetCheapestNeighborsDirection(startPosition);

                path.Add(newDirection);
                startPosition += newDirection;
                if(pathFindingMap.map[startPosition.x, startPosition.y] == int.MaxValue)
                {
                    break;

                }
                loop++;
            }

        }


        return path.ToArray();
    }

    public Vector3[] PathToPlayerWithPosition(Vector3 position)
    {
        List<Vector3> path = new List<Vector3>();

        Vector2Int indexPlayer = ConvertWorldPositionToArray(player.position);
        Vector2Int indexEnnemy = ConvertWorldPositionToArray(position);

        Vector2Int size = new Vector2Int(pathFindingMap.map.GetLength(0),
                                            pathFindingMap.map.GetLength(1));

        int maxloop = 25;
        int loop = 0;

        path.Add(new Vector3(indexEnnemy.x * 3f, 0f, indexEnnemy.y * 3f));

        if (indexEnnemy.x >= 0 && indexEnnemy.y >= 0 && indexEnnemy.x < size.x && indexEnnemy.y < size.y)
        {
            Vector2Int startPosition = indexEnnemy;
            while (startPosition != indexPlayer && loop < maxloop)
            {

                Vector2Int newPosition = startPosition + GetCheapestNeighborsDirection(startPosition);

                path.Add(new Vector3(newPosition.x * 3f, 0f, newPosition.y * 3f));

                startPosition = newPosition;
                if (pathFindingMap.map[startPosition.x, startPosition.y] == int.MaxValue)
                {
                    break;

                }
                loop++;
            }
        }
        

        return path.ToArray();
    }


    public Vector2Int GetCheapestNeighborsDirection(Vector2Int node)
    {
        //Deplacements possibles
        Vector2Int[] DIRS = {
            new Vector2Int(1,0), // Droite
            new Vector2Int(-1,0),// Gauche
            new Vector2Int(0,1), // Haut
            new Vector2Int(0,-1) // Bas
            //new Vector2Int(-1,-1), // Bas - Droite
            //new Vector2Int(1,-1), // Bas - Gauche
            //new Vector2Int(1, 1), // Haut - Droite
            //new Vector2Int(-1,1) // haut - Gauche
        };
        Shuffle(ref DIRS);
        //Debug.Log(DIRS);

        /*List<Vector2Int> DIRS = new List<Vector2Int>();
        DIRS.Add(new Vector2Int(1, 0)); // Droite
        DIRS.Add(new Vector2Int(-1, 0));// Gauche
        DIRS.Add(new Vector2Int(0, 1)); // Haut
        DIRS.Add(new Vector2Int(0, -1)); // Bas*/


        Vector2Int direction = Vector2Int.zero;
        int minCost = pathFindingMap.map[node.x, node.y];

        foreach (var d in DIRS)
        {
            Vector2Int n = node + d;
            if (n.x >= 0
                && n.x < pathFindingMap.map.GetLength(0)
                && n.y >= 0
                && n.y < pathFindingMap.map.GetLength(1))
            {
                if(minCost > pathFindingMap.map[n.x, n.y])
                {
                    direction = d;
                    minCost = pathFindingMap.map[n.x, n.y];
                }
            }
        }

        return direction;
    }


    public void Shuffle(ref Vector2Int[] array)
    {
        int n = array.Length;

        while (n > 1)
        {
            int k = (UnityEngine.Random.Range(0, n));
            n--;
            Vector2Int value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

#if UNITY_EDITOR
    public bool showGrid = true;
    public bool labelGrid = true;
    private void OnDrawGizmos()
    {
        if (pathFindingMap.map == null)
            return;

        Vector3 position = Vector3.zero;



        for (int y = 0; y < map.map.GetLength(1) ; y++)
        {
            for (int x = 0; x < map.map.GetLength(0); x++)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;

                position.x = x * 3f;
                position.y = 1f;
                position.z = y * 3f;

                if (showGrid)
                {
                    if(map.map[x,y])
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.black;
                    }
                
                    Gizmos.DrawCube(position, Vector3.one*0.8f);
                }

                if(labelGrid)
                {
                    Handles.Label(position, "" + pathFindingMap.map[x, y], style);
                }
            }
        }
    }
#endif

}
