using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FontManager : MonoBehaviour
{
    [System.Serializable]
    public class FontGroup
    {
        public string groupName;
        public string tableCollectionName = "Font Table"; // имя таблицы
        public string entryName;                          // ключ, например "titanBoldFont"
        public List<TextMeshProUGUI> labels;
    }

    [SerializeField] private List<FontGroup> fontGroups;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        StartCoroutine(InitializeFonts());
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private IEnumerator InitializeFonts()
    {
        // ждём пока локализация инициализируется
        yield return LocalizationSettings.InitializationOperation;
        ApplyAllFonts(LocalizationSettings.SelectedLocale);
    }

    private void OnLocaleChanged(Locale locale)
    {
        ApplyAllFonts(locale);
    }

    private async void ApplyAllFonts(Locale locale)
    {
        bool isRTL = locale.Identifier.Code == "ar";

        foreach (var group in fontGroups)
        {
            var op = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<TMP_FontAsset>(
                group.tableCollectionName,
                group.entryName
            );

            await op.Task;

            if (op.Result == null)
            {
                Debug.LogWarning($"[FontManager] Шрифт не найден: {group.tableCollectionName}/{group.entryName}");
                continue;
            }

            foreach (var label in group.labels)
            {
                label.font = op.Result;
                label.isRightToLeftText = isRTL;
            }
        }
    }
}