using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(new Vector3(0, -Time.deltaTime * _speed, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(new Vector3(0, Time.deltaTime * _speed, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Rotate(new Vector3(-Time.deltaTime * _speed, 0, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Rotate(new Vector3(Time.deltaTime * _speed, 0, 0), Space.World);
        }
    }
}
