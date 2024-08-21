using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("UI")]
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI reloadItem;
    public int reloadValue = 0;
    public Slider hp;
    public TextMeshProUGUI hp_Txt;

    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(this.gameObject);
        hp.value = 1000;
        hp_Txt.text = "1000";
    }

    private void Update()
    {
        
    }
}
