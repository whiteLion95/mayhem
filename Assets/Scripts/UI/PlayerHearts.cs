using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHearts : MonoBehaviour
{
    private List<Image> _hearts;
    private Player _player;

    private void Awake()
    {
        _hearts = new List<Image>(GetComponentsInChildren<Image>());
    }

    void Start()
    {
        _player = Player.Instance;

        _player.HealthController.OnTakeDamage += HandlePlayerTakeDamage;
    }

    private void LoseHeart()
    {
        if (_hearts != null && _hearts.Count > 0)
        {
            _hearts[_hearts.Count - 1].gameObject.SetActive(false);
            _hearts.RemoveAt(_hearts.Count - 1);
        }
    }

    private void HandlePlayerTakeDamage(float damage)
    {
        if (damage > 0f)
        {
            LoseHeart();
        }
    }
}
