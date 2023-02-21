using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UImage = UnityEngine.UI.Image;

#nullable enable

namespace Synthesis.UI.Dynamic {
    public class ScreenspaceMarker : UIComponent {
        public ScreenspaceMarker(UIComponent? parent, GameObject rootObject) : base(parent, rootObject) {
            var img = new Image(parent, RootGameObject);
            img.SetSprite(SynthesisAssetCollection.GetSpriteByName("250r-rounded"));
            img.SetColor(Color.cyan);
            var rectParent = RootRectTransform.GetComponent<RectTransform>();
            RootRectTransform.position = new Vector3(rectParent.sizeDelta.x / 2, rectParent.sizeDelta.y / 2);
        }

        public void SetPosition(Vector2 pos) {
            RootRectTransform.anchoredPosition = pos;
        }
    }
}
