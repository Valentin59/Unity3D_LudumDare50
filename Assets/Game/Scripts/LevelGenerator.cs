using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelGenerator : MonoBehaviour
{

    // --------------- Level Configuration -------------------- \\

    public enum CELLTYPE
    {
        WALL,
        FLOOR,
        WATER
    }

    [Header("Map")]
    public Map map;
    public Transform parent;
    public CELLTYPE[,] cmap;
    public bool[,] bmap;
    public Vector2Int size = new Vector2Int(10, 15);

    [Header("Randomness")]
    public int seed = 100;
    public bool randomSeed = true;

    [Header("Cellular Automata")]
    [Range(0f,1f)]
    public float floorDensityRandomRate = 0.5f;
    public int iteration = 2;

    [Header("Ennemies")]
    public GameObject[] simple;
    public GameObject[] champion;
    public GameObject[] boss;


    [Header("Tiles")]
    public GameObject wall;
    public GameObject floor;
    public GameObject water;
    public Vector2 cellSize = new Vector2(3f, 3f);


    [Header("Tree")]
    [Range(0f, 100f)]
    public float treeDensity = 20f;
    public GameObject[] trees;


    [Header("Details")]
    [Range(0f, 100f)]
    public float detailDensity = 20f;
    public GameObject[] details;


    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }


    public void GenerateLevel()
    {
        cmap = new CELLTYPE[size.x, size.y];
        //map.map = new bool[size.x, size.y];
        map.Initialize(size);
        //bmap = new bool[size.x, size.y];


        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        // Random Number 
        if (randomSeed)
        {
            seed = Random.Range(0, int.MaxValue);
        }
        Random.InitState(seed);

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(y == 0 || x == 0 || y == size.y-1 || x == size.x - 1)
                {
                    map.map[x, y] = false;
                }
                else
                {
                    float rng = Random.Range(0, 1f);

                    // Create procedural level map
                    map.map[x, y] = rng < floorDensityRandomRate ? true : false;
                }
            }
        }

        for (int i = 0; i < iteration; i++)
        {
            bool[,] nmap = new bool[size.x, size.y];
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    //float rng = Random.Range(0, 1f);
                    /*if(rng < 0.9f && rng > 0.1f)
                        cmap[x, y] = CELLTYPE.FLOOR;
                    else if(rng <= 0.1f)
                        cmap[x, y] = CELLTYPE.WATER;
                    else if (rng >= 0.9f)
                        cmap[x, y] = CELLTYPE.WALL;*/

                    if (CountFloorNeighboor(map.map, x, y) > 3)
                    {
                        nmap[x, y] = true;
                    }
                    else
                    {
                        nmap[x, y] = false;
                    }

                    //map[x, y] = CELLTYPE.FLOOR;
                }
            }
            map.map = nmap;
        }

        // Generate all models

        // -------------------- Tiles ---------------------
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector3 position = new Vector3(x * cellSize.x, 0f, y * cellSize.y);
                GameObject go = null;
                /*switch (cmap[x,y])
                {
                    case CELLTYPE.WALL:
                        go = GameObject.Instantiate(wall, position, Quaternion.identity);
                        break;
                    default:
                    case CELLTYPE.FLOOR:
                        go = GameObject.Instantiate(floor, position, Quaternion.identity);
                        break;
                    case CELLTYPE.WATER:
                        go = GameObject.Instantiate(water, position, Quaternion.identity);
                        break;
                }*/

                if(map.map[x,y])
                {
                    go = GameObject.Instantiate(floor, position, Quaternion.identity);
                    go.name = "tileFloor (" + x + "," + y + ")";


                    float rng = Random.Range(0, 1f);
                    float density;
                    density = detailDensity / 100f;
                    if (rng < density)
                    {
                        //GameObject
                        density *= 10f;
                        for (int i = 0; i < Mathf.FloorToInt(density); i++)
                        {
                            GameObject dprefab = GetRandomDetails();
                            if(dprefab != null)
                            {
                                Vector3 dposition = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                                GameObject tmpgo = GameObject.Instantiate<GameObject>(dprefab, position +  dposition, Quaternion.identity);
                                tmpgo.transform.parent = parent;
                            }
                        }
                    }

                    /*if (//(x > 0 && x < 3 && y < size.y && y > size.y - 3)||
                        ((x > 0 && x < 3) || (y < size.y && y > size.y - 3)))
                        //((x > 0 && x < 3) || (y < 3 && y > 0)))// || (x < size.x - 3 && x) )
                    {*/
                        rng = Random.Range(0, 100f);
                        density = treeDensity;
                        if (rng < density)
                        {
                            GameObject tprefab = GetRandomTree();
                            if (tprefab != null)
                            {
                                Vector3 tposition = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
                                GameObject tmpgo = GameObject.Instantiate<GameObject>(tprefab, position + tposition, Quaternion.identity);
                                tmpgo.transform.parent = parent;
                            }
                        }
                    //}

                }
                else
                {
                    go = GameObject.Instantiate(water, position, Quaternion.identity);
                    go.name = "tileWall (" + x + "," + y + ")";
                }
                go.transform.parent = parent;
            }
        }

        // ---------------- Details ---------------


        // ---------------- Tree ------------------




        map.MapGenerated();
    }

    public GameObject GetRandomDetails()
    {
        if (details != null && details.Length > 0)
            return details[Random.Range(0, details.Length)];
        else
            return null;
    }

    public GameObject GetRandomTree()
    {
        if(trees != null && trees.Length > 0)
            return trees[Random.Range(0, trees.Length)];
        else
            return null;
    }


    public int CountFloorNeighboor(bool[,] map, int x, int y)
    {
        int count = 0;

        //   X   X   X
        //   X   0   X
        //   X   X   X

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int pos_y = i + y;
                int pos_x = j + x;

                if (pos_x == x && pos_y == y)
                {
                    // rien
                }
                else if (pos_x < size.x && pos_x >= 0 &&
                    pos_y < size.y && pos_y >= 0)
                {
                    if (map[pos_x, pos_y])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(10f, 10f, 100f, 35f), "Generate Map"))
        {
            GenerateLevel();
        }
    }



        // Update is called once per frame
        void Update()
    {
        
    }
}
