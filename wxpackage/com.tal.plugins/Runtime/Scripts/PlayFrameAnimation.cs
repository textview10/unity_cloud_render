using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayFrameAnimation : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image fadeImage;
    [HideInInspector]
    public float fadeDuration = 0.2f;
    private void Start()
    {
        var comp = GetComponent<FramesAnimation>();
        comp.AnimPlay(sprites,false);
        comp.OnComplete(() => { this.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.2f); });
    }
}
