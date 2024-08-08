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

    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(this.gameObject);
        hp.value = 100;
    }

    private void Update()
    {
        
    }
}
