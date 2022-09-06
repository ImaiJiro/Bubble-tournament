using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickShopBtn() {
        SceneManager.LoadScene("ShopCurrencies_portrait");
    }

    public void OnClickShopExitBtn() {
        SceneManager.LoadScene("ListTournaments_portrait");
    }
}
