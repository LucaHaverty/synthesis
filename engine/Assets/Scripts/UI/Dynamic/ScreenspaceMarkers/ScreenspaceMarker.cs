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

            SetPosition(new Vector2(0, 0));
            
            // rootObject.transform.localPosition = new Vector3(0, 0); // Works, is centered around center
            // RootRectTransform.anchoredPosition = new Vector2(1920 / 2, 1080 / 2); // Does the same thing as the one above
        }

        public void SetPosition(Vector2 pos) {
            RootRectTransform.anchoredPosition = pos;
        }
    }
}
