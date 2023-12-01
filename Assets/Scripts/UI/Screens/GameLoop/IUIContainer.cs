using UI.Windows;
using UnityEngine;

namespace UI.Screens.GameLoop
{
    public interface IUIContainer
    {
        GameObject GameObject { get; }
        StartGameWindow StartGameWindow { get; }
        GameOverWindow GameOverWindow { get; }
        SettingsWindow SettingsWindow { get; }
        FinishLevelWindow FinishLevelWindow { get; }
        GameScreen GameScreen { get; }
        void Construct();
        void HideAll();
        void ShowStartGameWindow();
        void CloseStartGameWindow();
        void ShowFinishLevelWindow();
        void CloseFinishLevelWindow();
        void ShowGameOverWindow();
        void CloseGameOverWindow();
        void PrepareLevel();
    }
}