using UnityEngine;
using UnityEditor;

internal sealed class SpriteImporter : AssetPostprocessor {

    public void OnPreprocessTexture() {
        var importer = (TextureImporter)assetImporter;
        var textureSize = EditorUtils.GetPreprocessedImageSize(importer);
        var path = assetPath;
        var assetName = path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1);

        if (path.Contains("Sprites") || path.Contains("UI") || path.Contains("tilesets")) {
            if (!path.Contains("tilesets")) {
                importer.filterMode = FilterMode.Point;
            }
            
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.textureType = TextureImporterType.Sprite;
        }
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        foreach (var path in importedAssets) {
            if (path.EndsWith(".png") && path.Contains("Sprites")) {
                var assetName = path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1);
            }
        }
    }
}
