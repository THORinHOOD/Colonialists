using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public GameObject hex;
    public GameObject townplace;
    public GameObject road;

    public int width = 5;
    public int height = 5;

    public float offRowXOffset = 0.5f;
    public float offRowZOffset = 0.75f;

    private float hexWidth = 1.732051f;
    private float hexHeight = 2f;
    private float hexDepth = 0.34f;

    private Dictionary<string, GameObject> hexes;
    private Dictionary<string, GameObject> townplaces;
    private Dictionary<string, GameObject> roads;

    void Start () {
        Game.StartGame();
        CreateMap();
    }

    public GameObject getHex(string key)
    {
        if (hexes.ContainsKey(key))
            return hexes[key];
        else
            return null;
    }
    
    public List<GameObject> townNeighbors(Coord coordOfTown)
    {
        List<GameObject> neighbors = new List<GameObject>();

        if (coordOfTown.Y % 2 == 0)
        {
            Coord _1, _2, _3;

            int pos = coordOfTown.Y / 2;
            if (pos % 2 == 0)
            {
                _1 = new Coord(coordOfTown.X, coordOfTown.Y - 1);
                _2 = new Coord(coordOfTown.X, coordOfTown.Y + 1);
                _3 = new Coord(coordOfTown.X + 1, coordOfTown.Y + 1);
            } else
            {
                _1 = new Coord(coordOfTown.X, coordOfTown.Y - 1);
                _2 = new Coord(coordOfTown.X, coordOfTown.Y + 1);
                _3 = new Coord(coordOfTown.X - 1, coordOfTown.Y + 1);
            }

            if (townplaces.ContainsKey(_1.ToString()))
                neighbors.Add(townplaces[_1.ToString()]);
            if (townplaces.ContainsKey(_2.ToString()))
                neighbors.Add(townplaces[_2.ToString()]);
            if (townplaces.ContainsKey(_3.ToString()))
                neighbors.Add(townplaces[_3.ToString()]);
        } else
        {
            Coord _1, _2, _3;

            int pos = coordOfTown.Y / 2;
            int type = pos % 2;
            if (pos % 2 == 0)
            {
                _1 = new Coord(coordOfTown.X, coordOfTown.Y + 1);
                _2 = new Coord(coordOfTown.X, coordOfTown.Y - 1);
                _3 = new Coord(coordOfTown.X - 1, coordOfTown.Y - 1);
            } else
            {
                _1 = new Coord(coordOfTown.X, coordOfTown.Y + 1);
                _2 = new Coord(coordOfTown.X, coordOfTown.Y - 1);
                _3 = new Coord(coordOfTown.X + 1, coordOfTown.Y - 1);
            }
           
            if (townplaces.ContainsKey(_1.ToString()))
                neighbors.Add(townplaces[_1.ToString()]);
            if (townplaces.ContainsKey(_2.ToString()))
                neighbors.Add(townplaces[_2.ToString()]);
            if (townplaces.ContainsKey(_3.ToString()))
                neighbors.Add(townplaces[_3.ToString()]);
        }
        return neighbors;
    }
    
    /// <summary>
    /// Сначала должен отработать метод CreateTownsGrid !!!
    /// </summary>
    private void CreateRoadsGrid()
    {
        if (road != null)
        {
            roads = new Dictionary<string, GameObject>();
            
            for (int row = 0; row < 4 + (height - 1) * 2; row += 2)
            {
                int col = 0;
                string key = col + "_" + row;
                while (townplaces.ContainsKey(key))
                {
                    List<GameObject> buf = townNeighbors(new Coord(col, row));

                    Vector3 from = townplaces[key].transform.position;
                    foreach (GameObject nTown in buf)
                    {
                        Town currentTown = nTown.GetComponent<Town>();
                        Vector3 to = nTown.transform.position;

                        float newX = (from.x + to.x) / 2f;
                        float newY = (from.z + to.z) / 2f;

                        GameObject roadObj = Instantiate(road, new Vector3(newX, hexDepth / 2.0f, newY), Quaternion.identity);
                        roadObj.transform.SetParent(transform.Find("Roads").transform);
                        roadObj.transform.right = from - to;

                        roadObj.name = "road-" + key + "-" + currentTown.Coord.ToString();
                    }
                    ++col;
                    key = col + "_" + row;
                }
            }
        } else
            Debug.LogError("road prefab not found!!!");
    }


    private void MakeRow_Town(float x_f, float y_f, int count, int row)
    {
        if (townplace != null) {
            float x = x_f;
            float y = y_f;
            for (int i = 0; i < count; i++)
            {
                GameObject townplaceObj = Instantiate(townplace, new Vector3(x, hexDepth / 2.0f, y), Quaternion.identity);
                townplaceObj.transform.SetParent(transform.Find("Towns").transform);
                townplaceObj.name = "place_town_" + i + "_" + row;
                townplaceObj.GetComponent<Town>().SetCoord(new Coord(i, row));
                townplaces.Add(townplaceObj.GetComponent<Town>().Coord.ToString(), townplaceObj);
                x += hexWidth;
            }
        } else
            Debug.LogError("townplace prefab not found!!!");
    }
    
    private void CreateTownsGrid(float x_c, float y_c)
    {
        if (townplace != null)
        {
            townplaces = new Dictionary<string, GameObject>();
            float x = x_c;
            float y = y_c - hexHeight / 2.0f;
            int count = 4;
            int row = 0;

            for (int i = 0; i <= height; i++)
            {
                if (i != 0)
                {
                    y += hexHeight / 2f;
                    if (i % 2 != 0)
                        y += hexHeight / 2f;
                }

                count = width;
                if ((i != 0 && i != height) || (i == height && height % 2 == 0))
                    count = width + 1;
                
                MakeRow_Town(x, y, count, row);
                row += 3;
                if (i % 2 != 0)
                    row -= 2;
            }

            x = x_c - hexWidth/2f;
            y = y_c - hexHeight / 4f;
            row = 1;
            for (int i = 0; i <= height; i++)
            {
                if (i != 0)
                {
                    y += hexHeight / 2f;
                    if (i % 2 != 1)
                        y += hexHeight / 2f;
                }

                count = width + 1;
                if (i == height && height % 2 == 0)
                {
                    x += hexWidth;
                    count--;
                }
                
                MakeRow_Town(x, y, count, row);
                row += 3;
                if (i % 2 == 0)
                    row -= 2;
            }
        }
        else
            Debug.LogError("townplace prefab not found!!!");
    }

    public void GiveResources(int number)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                string key = x + "_" + y;
                if (hexes.ContainsKey(key))
                {
                    Hex hex = hexes[key].GetComponent<Hex>();
                    if (hex.Number == number && hex.Resource != Game.ResourceType.Desert)
                        for (int row = y * 2; row <= y * 2 + 3; row++)
                        {
                            if (row == 2 * (height + 1) - 1 && y % 2 == 1)
                            {
                                int col = x;
                                transferRes(col + "_" + row, hex);
                            }
                            else
                            {
                                if (row == y * 2 || row == y * 2 + 3)
                                {
                                    int col;
                                    if (y % 2 == 0)
                                        col = x;
                                    else
                                        col = x + 1;
                                    transferRes(col + "_" + row, hex);
                                }
                                else
                                    for (int col = x; col <= x + 1; col++)
                                        transferRes(col + "_" + row, hex);
                            }
                        }
                }
            }
    }

    private void transferRes(string town_key, Hex hex)
    {
        if (townplaces.ContainsKey(town_key))
        {
            Town town = townplaces[town_key].GetComponent<Town>();
            if (town.Owner != -1)
                Game.players[town.Owner].addRes(hex.Resource, 1);
        }
    }

    private void CreateMap()
    {
        Game.giveResources += GiveResources;
        if (hex != null || townplace != null  || road != null)
        {
            hexes = new Dictionary<string, GameObject>();
            float startX = transform.position.x;
            float startY = transform.position.z;
            
            //установить дороги
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    hexes.Add((new Coord(x, y)).ToString(), CreateHex(x , y, startX, startY));
            
            //установить города
            CreateTownsGrid(startX, startY);
            //установить дороги
            CreateRoadsGrid();
        }
        else
            Debug.LogError("Prefab of hex or townplace or road not found");
    }

    private GameObject CreateHex(int x, int y, float startX = 0, float startY = 0)
    {
        float xPos = x;
        if (y % 2 == 1)
            xPos += offRowXOffset;
        
        GameObject mapHex = Instantiate(hex, new Vector3(startX + xPos * hexWidth, 0, startY + y * hexHeight * offRowZOffset), Quaternion.identity);
        mapHex.name = "Hex_" + x + "_" + y;
        mapHex.transform.SetParent(transform);
        mapHex.isStatic = true;
        mapHex.GetComponent<Hex>().InitHex(x, y, this);
        return mapHex;
    }

	void Update () {
		
	}
}
