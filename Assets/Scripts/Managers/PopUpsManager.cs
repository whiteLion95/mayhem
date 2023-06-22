using UnityEngine;

public class PopUpsManager : MonoBehaviour
{
    [SerializeField]
    private World_PopUp_Text_Image _worldPopUp;
    [SerializeField]
    private string _headShotText = "HEAD SHOT!";

    private void OnEnable()
    {
        Enemy.OnHeadShot += HandleEnemyHeadShot;
    }

    private void OnDisable()
    {
        Enemy.OnHeadShot -= HandleEnemyHeadShot;
    }

    private void HandleEnemyHeadShot(Enemy enemy, Vector3 hitPos)
    {
        World_PopUp_Text_Image popUp = Instantiate(_worldPopUp, hitPos, Quaternion.identity);
        popUp.Text.text = _headShotText;
        popUp.Show();
    }
}
