using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignUtils : MonoBehaviour
{
    public Player player;
    public CinemachineVirtualCamera battleCam;
    public List<CoverPlace> coverPlaces;

    private int _curCoverPlaceIndex = -1;

    [Button]
    public void Assign()
    {
        player = FindObjectOfType<Player>();
        coverPlaces = new List<CoverPlace>(FindObjectsOfType<CoverPlace>());
        battleCam = GameObject.Find("battleCam").GetComponent<CinemachineVirtualCamera>();
    }

    [Button]
    public void SetPlayerInCoverPlace(int index)
    {
        if (index >= 0 && index < coverPlaces.Count)
        {
            _curCoverPlaceIndex = index;
            player.transform.position = coverPlaces[_curCoverPlaceIndex].transform.position;
            battleCam.Follow = coverPlaces[_curCoverPlaceIndex].CamFocus;
            battleCam.LookAt = coverPlaces[_curCoverPlaceIndex].CamFocus;
            battleCam.Priority = 1;
        }
    }

    [Button]
    public void SetPlayerInNextCoverPlace()
    {
        if (_curCoverPlaceIndex < coverPlaces.Count - 1)
            SetPlayerInCoverPlace(_curCoverPlaceIndex + 1);
        else
            SetPlayerInCoverPlace(0);
    }

    [Button]
    public void ResetPlayer()
    {
        player.transform.position = Vector3.zero;
        battleCam.Priority = 0;
        _curCoverPlaceIndex = -1;
    }
}
