using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public InputField consoleInput;

    public float speed = 1f;
    private float dec = 10f;

    public float Speed
    {
        get { return speed / dec; }
    }
    
	void Update () {

        if (consoleInput != null)
            if (consoleInput.isFocused)
                return;
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
        Quaternion rotation = Quaternion.Euler(0, transform.eulerAngles.y - 90, 0);
        movement = rotation * movement;
        transform.position += movement * Speed;
        
        if (Input.GetKey(KeyCode.E))
            transform.eulerAngles += new Vector3(0, 1, 0) * Speed * dec / 1.5f;

        if (Input.GetKey(KeyCode.Q))
            transform.eulerAngles += new Vector3(0, -1, 0) * Speed * dec / 1.5f;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            transform.Translate(Vector3.forward * Speed * 1.5f);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            transform.Translate(Vector3.forward * (-Speed) * 1.5f);


        if (Input.GetKeyDown(KeyCode.Space))
            Game.endTurn();
    }
}
