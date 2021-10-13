using UnityEngine;
using UnityEngine.Tilemaps;

public class PropertiedTile : Tile  {

    public TilePropertyData properties;

    public PropertiedTile(TilePropertyData properties) {
        this.properties = properties;
    }

    public TilePropertyData GetData() {
        return properties;
    }

    public bool EqualsTile(TileBase tile) {
        if (tile == null || !(tile is PropertiedTile)) {
            return false;
        }
        PropertiedTile other = (PropertiedTile)tile;
        return sprite == other.sprite;
    }

    public Sprite GetSprite() {
        return sprite;
    }
}
