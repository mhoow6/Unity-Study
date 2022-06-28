using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mhoow6.SafeArea
{
    public class Background : SafeAreaTool
    {
        private void Start()
        {
            if (spaceOverlayCanavs)
            {
                var canvasRectTransform = spaceOverlayCanavs.GetComponent<RectTransform>();

                // ĵ���� RectTransform.sizeDelta�� ����̽��� ũ�⿡ ���缭 �˾Ƽ� �þ��.
                float fullscreen_width = Mathf.Floor(canvasRectTransform.sizeDelta.x);
                float fullscreen_height = Mathf.Ceil(canvasRectTransform.sizeDelta.y);

                rectTransform.sizeDelta = new Vector2(fullscreen_width, fullscreen_height);
            }
        }
    }
}

