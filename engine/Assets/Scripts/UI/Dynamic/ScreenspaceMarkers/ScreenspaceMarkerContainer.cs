using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UImage = UnityEngine.UI.Image;

#nullable enable

namespace Synthesis.UI.Dynamic {

    public class ScreenspaceMarkerContainer : Content {
        public ScreenspaceMarkerContainer(Content c) : base(c.Parent, c.RootGameObject, null) {

            var rect = GameObject.Find("UI").transform.Find("ScreenSpace").GetComponent<RectTransform>();
            RootGameObject.transform.parent = rect;
            base.SetStretch<Content>();

            var m = CreateMarker();
            // m.SetPosition(new Vector2(0, 0));
        }

        private ScreenspaceMarker CreateMarker() {
            var content = this.CreateSubContent(new Vector2(50, 50));
            var marker = new ScreenspaceMarker(this, content.RootGameObject);
            return marker;
        }

        public void UpdateMarkerLocations() {
            Vector3[] corners = new Vector3[4];
            RootRectTransform.GetWorldCorners(corners);
            
            
        }
    }

}
