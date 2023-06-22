using System.Collections.Generic;
using UnityEngine;

public class UI_ManagerBase : MonoBehaviour
{
    [SerializeField] private List<UI_Screen> screens;
    [SerializeField] private UI_Screen defaultScreen;

    protected virtual void Start()
    {
        InitScreens();
    }

    private void InitScreens()
    {
        foreach (var screen in screens)
        {
            if (!screen.Equals(defaultScreen))
            {
                screen.Hide(0f);
            }
            else
            {
                screen.Show(0f);
            }
        }
    }

    public void ShowLoseScreen()
    {
        ShowScreen(UI_Screen.Name.Lose);
    }

    public void ShowWinScreen()
    {
        ShowScreen(UI_Screen.Name.Win);
    }

    public void ShowScreen(UI_Screen.Name screenName)
    {
        foreach (var screen in screens)
        {
            if (screen.ScreenName == screenName)
                screen.Show();
            else
                screen.Hide();
        }
    }
}
