namespace LoadImageTools
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System;


    public class ConversionTools
    {
        // Texture from TextureJson
        public static Texture2D TextureFromJson(string textureDataBase64, Vector2Int textureSize, FilterMode filterMode)
        {
            // Get bytes from base64 string
            byte[] pngBytes = Convert.FromBase64String(textureDataBase64);

            // Make your new (empty) texture
            Texture2D texture = new(textureSize.x, textureSize.y)
            {
                filterMode = filterMode,
            };

            // Create a texture from bytes
            texture.LoadImage(pngBytes);

            return texture;
        }



        public static Dictionary<string, Sprite> SpritesFromJson(SpriteInfoLibrary spriteInfoLib)
        {
            // Get my collection of sprite info (texture in raw base64 bytes and their position on that texture)
            Texture2D allSpritesTexture = TextureFromJson(spriteInfoLib.textureAsBase64, spriteInfoLib.textureSize, spriteInfoLib.filterMode);

            // This is where we will store all our sprites
            Dictionary<string, Sprite> allSprites = new();

            // Create sprites using all sprite info datas and our texture.
            foreach (SpriteInfo spriteInfo in spriteInfoLib.allSpriteInfos)
            {

                Sprite newSprite = Sprite.Create(allSpritesTexture, spriteInfo.rect, new Vector2(0.5f, 0.5f), spriteInfo.ppu);

                newSprite.name = spriteInfo.name;

                allSprites.Add(newSprite.name, newSprite);
            }

            return allSprites;
        }
    }



    [System.Serializable]
    public class SpriteInfo
    {
        public string name;

        public Rect rect;
        public float ppu; // Pixels per unit
    }

    [System.Serializable]
    public class SpriteInfoLibrary
    {
        public FilterMode filterMode;

        public Vector2Int textureSize;

        public List<SpriteInfo> allSpriteInfos = new();

        public string textureAsBase64;
    }
}
