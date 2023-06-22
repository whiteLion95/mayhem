using System;
using UnityEngine;

public class BattleCamTrigger : MonoBehaviour
{
    public static Action OnPlayerTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerTrigger?.Invoke();
        }
    }
}
