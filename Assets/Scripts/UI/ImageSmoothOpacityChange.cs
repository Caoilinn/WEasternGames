using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSmoothOpacityChange : MonoBehaviour
{
    private float originOpacity = 0;
    private float targetOpacity = 0;
    private Image image;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ChangeOpacity(float targetOpacity) {

    }

    
}
