
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuUI : GamePopup
{
    public override void Show(bool isHideLastPopup = true, object data = null, UnityAction<object> closeCallback = null, bool isShowBackgroundPopup = true, CheckCondition condition = null)
    {
        base.Show(isHideLastPopup, data, closeCallback, isShowBackgroundPopup, condition);
    }

    public void OnClickPlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Gameplay_Scene");
    }
}
