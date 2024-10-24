using System.Collections;
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
         StartCoroutine(LoadGameplayScene("Gameplay_Scene"));
    }

    public void OnClickLobby()
    {
          UiManager.Instance.ShowGamePopup<LobbyUI>(isShowSuperImage : false);
    }

    private IEnumerator LoadGameplayScene(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
