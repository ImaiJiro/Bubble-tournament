using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIModel : MonoBehaviour
{

    /// <summary>
    /// Shows whole UI panel
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides whole UI panel
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
