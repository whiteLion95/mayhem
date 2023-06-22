public class UI_Manager : UI_ManagerBase
{
    private GameManager _gameManager;

    protected override void Start()
    {
        base.Start();

        _gameManager = GameManager.Instance;

        _gameManager.OnLose += HandleLose;
        _gameManager.OnWin += HandleWin;
    }

    private void HandleLose()
    {
        ShowLoseScreen();
    }

    private void HandleWin()
    {
        ShowWinScreen();
    }
}
