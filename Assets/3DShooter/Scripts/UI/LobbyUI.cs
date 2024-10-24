using UnityEngine.Events;

public class LobbyUI : GamePopup
{
     public override void Show(bool isHideLastPopup = true, object data = null, UnityAction<object> closeCallback = null, bool isShowBackgroundPopup = true, CheckCondition condition = null)
    {
        base.Show(isHideLastPopup, data, closeCallback, isShowBackgroundPopup, condition);
    }
}
