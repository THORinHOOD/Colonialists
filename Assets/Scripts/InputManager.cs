using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    public GameObject map;
    private GameObject choosedPlaceTown;
    private Text hexInfo;

    void Start () {
       hexInfo = GameObject.Find("Canvas/Caption_HexInfo").GetComponent<Text>();
    }
	
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            if (choosedPlaceTown != null)
            {
                if (choosedPlaceTown.GetComponent<Town>().Owner == -1)
                    choosedPlaceTown.GetComponentInChildren<MeshRenderer>().enabled = false;
                choosedPlaceTown = null;
            }

            GameObject smthHit = hitInfo.collider.transform.parent.gameObject;
            
            if (smthHit.name.Contains("place_town"))
            {
                if (smthHit.GetComponent<Town>().Owner == -1) {
                    choosedPlaceTown = smthHit;
                    choosedPlaceTown.GetComponentInChildren<MeshRenderer>().enabled = true;
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (map != null)
                            BuilderManager.buildTown(choosedPlaceTown, map.GetComponent<Map>());
                        else
                            Debug.LogError("Map not found!!!");
                    }
                }
            } else if (smthHit.name.Contains("Hex"))
            {
                if (hexInfo != null)
                {
                    Hex hex = smthHit.GetComponent<Hex>();
                    hexInfo.text = hex.Resource.ToString() + " " + hex.Number;
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log(smthHit.name);
            }
        } else
        {
            if (choosedPlaceTown != null)
            {
                choosedPlaceTown.GetComponentInChildren<MeshRenderer>().enabled = false;
                choosedPlaceTown = null;
            }

            if (hexInfo != null)
            {
                hexInfo.text = "";
            }
        }



    }

}
