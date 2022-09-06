using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Mkey
{
    public class PanelContainerController : MonoBehaviour
    {
        public Button OpenCloseButton;
        public Button BrushSelectButton;
        public Image selector;
        public Image brushImage;
        public Text BrushName;
        public string capital;
        public List<GridObject> gridObjects;
        public RectTransform scrollPanelParent;

        [SerializeField]
        private ScrollPanelController ScrollPanelPrefab;

        internal ScrollPanelController ScrollPanel;

        [SerializeField]
        private bool adjustWidth = false;

        private void Start()
        {
            if (adjustWidth)
            {
                RectTransform rT = GetComponent<RectTransform>();
                RectTransform parent = transform.parent.GetComponentInParent<RectTransform>();
                float width = parent.rect.width;
                rT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
        }

        public ScrollPanelController InstantiateScrollPanel()
        {
            if (!ScrollPanelPrefab || !scrollPanelParent) return null;
            if (ScrollPanel) DestroyImmediate(ScrollPanel.gameObject);
            ScrollPanel = Instantiate(ScrollPanelPrefab, scrollPanelParent);
            return ScrollPanel;
        }
    }
}