using Lam.zGame.Core_game.Core.Utilities.Custom_Inspector.Custom_Attributes;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.UI
{
    public class ScreenSafeArea : MonoBehaviour
    {
        public Canvas canvas;
        public RectTransform root;
        public bool fixedTop;
        public bool fixedBottom;
        private ScreenOrientation mCurrentOrientation;
        private Rect mCurrentSafeArea;

        private void Start()
        {
            mCurrentOrientation = Screen.orientation;
            mCurrentSafeArea = Screen.safeArea;

            CheckSafeArea2();
        }

        /// <summary>
        /// This method work well in simulator or device but in editor it is little buggy if Simulator is not currently active
        /// So this method for only infomation purpose. The method 2 is much better
        /// </summary>
        [InspectorButton]
        public void CheckSafeArea()
        {
            var safeArea = Screen.safeArea;
            var sWidth = Screen.currentResolution.width;
            var sHeight = Screen.currentResolution.height;
            var oWidthTop = (Screen.currentResolution.width - safeArea.width - safeArea.x) / 2f;
            var oHeightTop = (Screen.currentResolution.height - safeArea.height - safeArea.y) / 2f;
            var oWidthBot = -safeArea.x / 2f;
            var oHeightBot = -safeArea.y / 2f;
            Debug.Log($"Screen size: (width:{sWidth}, height:{sHeight})" +
                $"\nSafe area: {safeArea}" +
                $"\nOffset Top: (width:{oWidthTop}, height:{oHeightTop})" +
                $"\nOffset Bottom: (width:{oWidthBot}, height:{oHeightBot})");

            var offsetTop = new Vector2(oWidthTop, oHeightTop);
            var offsetBottom = new Vector2(oWidthBot, oHeightBot);

            if (!fixedTop)
                root.offsetMax = new Vector2(0, -offsetTop.y);
            else
                root.offsetMax = Vector2.zero;
            if (!fixedBottom)
                root.offsetMin = new Vector2(0, -offsetBottom.y);
            else
                root.offsetMin = Vector2.zero;

        }

        [InspectorButton]
        public void CheckSafeArea2()
        {
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;
            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            if (!fixedBottom)
                root.anchorMin = anchorMin;
            else
                root.anchorMin = Vector2.zero;
            if (!fixedTop)
                root.anchorMax = anchorMax;
            else
                root.anchorMax = Vector2.one;

            mCurrentOrientation = Screen.orientation;
            mCurrentSafeArea = Screen.safeArea;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (mCurrentOrientation != Screen.orientation || mCurrentSafeArea != Screen.safeArea)
                CheckSafeArea2();
        }
#endif
    }
}