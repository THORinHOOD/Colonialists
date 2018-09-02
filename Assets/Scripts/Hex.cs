using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
    
    public GameObject Tree;
    public GameObject Rock;
    public GameObject Brick;
    public GameObject Sheep;
    public GameObject Wheat;
    public GameObject Desert;

    public Color green;
    public Color gray;
    public Color brown;
    public Color yellow;

    private Coord coord;
    private Game.ResourceType res;
    private int number;

    public int X { get { return coord.X; } }
    public int Y { get { return coord.Y; } }
    public Game.ResourceType Resource { get { return res; } }
    public int Number { get { return number; } }
    
    public void InitHex(int x, int y, Map map)
    {
        coord = new Coord(x, y);
        number = Random.Range(2, 12);
        switch(Random.Range(1, 7))
        {
            case 1:
                DesginHex(Game.ResourceType.Brick, brown, Brick);
                break;
            case 2:
                DesginHex(Game.ResourceType.Rock, gray, Rock);
                break;
            case 3:
                DesginHex(Game.ResourceType.Sheep, green, Sheep);
                break;
            case 4:
                DesginHex(Game.ResourceType.Tree, green, Tree);
                break;
            case 5:
                DesginHex(Game.ResourceType.Wheat, green, Wheat);
                break;
            case 6:
                DesginHex(Game.ResourceType.Desert, yellow, Desert);
                break;
        }
    }   

    private void DesginHex(Game.ResourceType type, Color color, GameObject toInst)
    {
        res = type;
        if (toInst != null)
        {
            GameObject obj = Instantiate(toInst, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
        }
        if (color != null)
            GetComponentInChildren<Renderer>().material.color = color;
    }
}
