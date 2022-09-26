using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BackgroundImageScaler : MonoBehaviour
{
    Image backgroundImage;
    RectTransform rectTransform;
    float scaleRatio;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        rectTransform = backgroundImage.rectTransform;
        scaleRatio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rectTransform) {
            return;
        }

        if (Screen.height * scaleRatio > Screen.width) {
            rectTransform.sizeDelta = new Vector2(Screen.height * scaleRatio, Screen.height);
        } else {
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / scaleRatio);
        }
    }
}
