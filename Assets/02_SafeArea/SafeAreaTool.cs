using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mhoow6.SafeArea
{
    public abstract class SafeAreaTool : MonoBehaviour
    {
        [Tooltip("Device Simulator 사용해도 TRUE")]
        public bool IsInMoblieDevice;

        protected RectTransform rectTransform;
        protected Canvas canavs;

        protected void Awake()
        {
            if (!IsInMoblieDevice)
            {
                enabled = false;
                return;
            }

            rectTransform = GetComponent<RectTransform>();

            if (TryGetCanvas(out var canvas))
                canavs = canvas;
            else
            {
                Canvas[] canvaslist = FindObjectsOfType<Canvas>();
                canavs = canvaslist.First(canvas =>
                {
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        return true;
                    return false;
                });
            }
        }

        bool TryGetCanvas(out Canvas result)
        {
            Canvas find = null;
            result = find;

            return false;
        }

        protected virtual void OnAwake() { }
    }
}

