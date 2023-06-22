using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 5f;

    private void OnEnable()
    {
        StartCoroutine(LifeTimeRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator LifeTimeRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
