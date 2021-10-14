using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropertiedTile))]
public class PropertiedTileEditor : Editor {

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
        var data = (PropertiedTile)target;

        if (data == null) {
            return null;
        }

        var sprite = data.GetSprite();
        if (sprite == null) {
            return null;
        }

        var tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        var pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
        tex.SetPixels(0, 0, (int)sprite.rect.width, (int)sprite.rect.height, pixels);
        tex.Apply();
        TextureScale.Point(tex, width, height);

        return tex;
    }
}
