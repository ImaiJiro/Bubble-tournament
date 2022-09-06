using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPan : BaseUIModel
{
    public TMP_Text loading_title;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onChangeLoadingTitle(string title_str) {
        loading_title.text = title_str;
    }
}
