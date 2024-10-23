
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image backgroundPopup;
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private RectTransform gamePopupParent;
    [SerializeField] private GamePopup[] gamePopupInScene;
    [SerializeField] private PopupPrefabSO popupPrefabs;

    public static UiManager Instance { get; private set; }
    public static bool IsInitialized { get; private set; }

    //public Color SuperImageColor => superImage.color;
    public Image BackgroundPopup => backgroundPopup;

    public Canvas RootCanvas => rootCanvas;

    public static float SizeScale { get; private set; } = 1;

    public UnityAction OnCloudHide;

    private Dictionary<string, GamePopup> gamePopups = new();
    private Dictionary<string, bool> isLoadingGamePopup = new();

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private IEnumerator Start()
    {
        backgroundPopup.gameObject.SetActive(false);

        if (gamePopupInScene != null && gamePopupInScene.Length > 0)
        {
            foreach (var popup in gamePopupInScene)
            {
                string popupName = popup.name;

                if (!gamePopups.ContainsKey(popupName))
                    gamePopups.Add(popupName, popup);
            }
        }

        yield return null;



        // get canvas height
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        if (canvasScaler != null)
        {
            float panelWidth = Screen.width;
            float panelHeight = Screen.height;

            SizeScale = canvasScaler.referenceResolution.y / panelHeight;
            if (SizeScale * panelWidth < canvasScaler.referenceResolution.x)
            {
                SizeScale = canvasScaler.referenceResolution.x / panelWidth;
            }

            Debug.Log($"UiManager: panelWidth: {panelWidth} - panelHeight: {panelHeight} - sizeScale: {SizeScale}");
        }
        else
        {
            Debug.Log("Cannot get sizeScale. canvasScaler is NULL");
        }

        IsInitialized = true;
        ShowGamePopup<MainMenuUI>(isShowSuperImage : false);
    }

    private void OnDisable()
    {
        Debug.Log("Destroy UiManager");
        StopAllCoroutines();
        //IsInitialized = false;
        //Instance = null;
    }

    public /*async*/ void ShowGamePopup<T>(UnityAction loadPopupDone = null, object data = null, UnityAction<object> closeCallback = null, bool isHideLastPopup = true, bool isShowSuperImage = true, bool isShowLoading = true) where T : GamePopup
    {
        string popupName = typeof(T).Name;
        Debug.Log($"Show popup: {popupName}");

        if (isLoadingGamePopup.TryGetValue(popupName, out bool isLoading) && isLoading)
            return;

        if (!gamePopups.ContainsKey(popupName))
        {
            isLoadingGamePopup.Add(popupName, true);

            //if (isShowLoading)
            //    GameManager.Instance.ShowLoading(true);

            //AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Popups/{popupName}.prefab");
            //await handle;

            if (IsInitialized && !gamePopups.ContainsKey(popupName))
            {
                gamePopups.Add(popupName, Instantiate(popupPrefabs.GetPrefab(popupName), gamePopupParent).GetComponent<T>());
            }

            //Addressables.ReleaseInstance(handle);

            isLoadingGamePopup[popupName] = false;

            //if (isShowLoading)
            //{
            //    GameManager.Instance.ShowLoading(false);
            //}
        }

        if (IsInitialized)
        {
            loadPopupDone?.Invoke();
            gamePopups[popupName].Show(isHideLastPopup, data, closeCallback, isShowSuperImage);
        }
    }


    public void ShowBackgroundPopup(RectTransform rect, Vector4 margin)
    {
        backgroundPopup.transform.SetAsLastSibling();
        backgroundPopup.gameObject.SetActive(true);
        //backgroundPopup.UpdateVertices(rect, rect == null ? Vector4.zero : margin);
    }

    public void HideBackgroundPopup()
    {
        backgroundPopup.gameObject.SetActive(false);
    }
}
