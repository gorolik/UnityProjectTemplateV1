using UnityEngine;

namespace Sources.Utilities.Extensions
{
    public static class CanvasGroupExtension
    {
        public static void SetView(this CanvasGroup canvasGroup, bool value)
        {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.blocksRaycasts = value;
        }
    }
}