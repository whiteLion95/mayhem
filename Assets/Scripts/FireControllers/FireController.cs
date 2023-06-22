using System;
using UnityEngine;

public abstract class FireController : MonoBehaviour
{
    public Action OnFire;

    protected Gun _gun;

    protected virtual void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
    }

    protected virtual void Start()
    {

    }
}