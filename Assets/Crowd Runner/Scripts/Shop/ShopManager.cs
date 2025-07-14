using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Button buyButton;
    [SerializeField] private SkinButton[] skinButtons;

    [Header("Skins")]
    [SerializeField] private Sprite[] skins;

    [Header("Pricing")]
    [SerializeField] private int skinPrice;
    [SerializeField] private Text priceText;

    public static Action<int> onSkinSelected;

    private void Awake()
    {
        UnlockSkin(0);

        priceText.text = skinPrice.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        ConfigureButtons();

        UpdateBuyButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UnlockSkin(Random.Range(0, skinButtons.Length));

        if (Input.GetKeyDown(KeyCode.D))
            PlayerPrefs.DeleteAll();
    }

    private void ConfigureButtons()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            bool unlocked = PlayerPrefs.GetInt("skinButton" + i) == 1;

            skinButtons[i].Configure(skins[i], unlocked);

            // can dong nay de lưu lại biến i. Vì khi chạy SelectSkin sẽ không được gọi luôn nên biến i đã thay đổi thành 9
            int skinIndex = i;

            skinButtons[i].GetButton().onClick.AddListener(() => SelectSkin(skinIndex));
        }
    }

    public void UnlockSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("skinButton" + skinIndex, 1);
        skinButtons[skinIndex].Unlock();
    }

    private void UnlockSkin(SkinButton skinButton)
    {
        int skinIndex = skinButton.transform.GetSiblingIndex();
        UnlockSkin(skinIndex);
    }

    private void SelectSkin(int skinIndex)
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            if (skinIndex == i)
                skinButtons[i].Select();
            else
                skinButtons[i].Deselect();
        }

        onSkinSelected?.Invoke(skinIndex);
    }

    public void BuySkin()
    {
        List<SkinButton> skinButtonList = new List<SkinButton> ();

        for (int i = 0; i < skinButtons.Length; i++)
        {
            if (!skinButtons[i].IsUnlocked())
                skinButtonList.Add(skinButtons[i]);
        }

        if (skinButtonList.Count <= 0)
            return;

        SkinButton randomSkinButton = skinButtonList[Random.Range(0, skinButtonList.Count)];

        UnlockSkin(randomSkinButton);
        SelectSkin(randomSkinButton.transform.GetSiblingIndex());

        DataManager.instance.UseCoins(skinPrice);

        UpdateBuyButton();
    }

    public void UpdateBuyButton()
    {
        if(DataManager.instance.GetCoins() < skinPrice)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;
    }
}
