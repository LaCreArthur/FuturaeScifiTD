using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelUpChoiceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI descriptionTMP;
    [SerializeField] Image image;
    [SerializeField] GameObject newTag;
    Button _button;
    LevelUpUI _levelUpUI;

    public static event Action UpgradeChosen;

    void Awake() => _button = GetComponent<Button>();
}
