using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]private float camaraSpeed = 5;
    private float speed;
    // Update is called once per frame
    void Update()
    {
        speed= camaraSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Translate(Vector3.up * camaraSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.Translate(Vector3.down * camaraSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.Translate(Vector3.right * camaraSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.Translate(Vector3.left * camaraSpeed);
        }
    }
}
