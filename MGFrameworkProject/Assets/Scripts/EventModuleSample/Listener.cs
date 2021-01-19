using MGFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{
    [SerializeField]
    private bool _mirror = false;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<Vector3>(EventId.POSITION, OnPosition);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<Vector3>(EventId.POSITION, OnPosition);
    }

    private void OnPosition(Vector3 arg)
    {
        this.transform.position = arg * (_mirror ? -1 : 1);
    }
}
