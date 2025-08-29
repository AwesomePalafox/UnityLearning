using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float turnSpeed = 50f;

    #region Light Switch
    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        LightSwitch();
        TurnAndMove();
    }
    #endregion


    void LightSwitch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myLight.enabled = !myLight.enabled;
        }
    }


    void TurnAndMove()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            
            
}

}
