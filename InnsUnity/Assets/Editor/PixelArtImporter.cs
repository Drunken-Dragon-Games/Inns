using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace DrunkenDragon.Editor
{
    /// <summary>
    /// ArtImporter enforces correct texture settings for items included in the Assets/Art/PixelArt folder.
    /// Be aware changes to this file may trigger a full-project reimport.
    /// </summary>
    public class PixelArtImporter : AssetPostprocessor
    {
        /// <summary>
        /// Regex to determine if the path of the asset being imported should be considered pixel art.
        /// </summary>
        private static readonly Regex PixelArtPath = new Regex(@"^Assets\/Art\/PixelArt\/.*\.png$");

        private static readonly Regex PixelArtAtlasPath = new Regex(@"^Assets\/Art\/PixelArt\/.*\.spriteatlas$");

        private static SpriteAtlasTextureSettings AtlasTextureSettings = new SpriteAtlasTextureSettings()
        {
            sRGB = true, // for color integrity
            filterMode = FilterMode.Point, // for crisp edges
        };

        private static TextureImporterPlatformSettings AtlasPlatformSettings = new TextureImporterPlatformSettings()
        {
            format = TextureImporterFormat.RGBA32, // for color integrity
        };

        private void OnPreprocessTexture()
        {
            if (PixelArtPath.IsMatch(assetPath) && assetImporter is TextureImporter textureImporter)
            {
                // All pixel art should use Point filter mode for crisp edges
                textureImporter.filterMode = FilterMode.Point;
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string importedAsset in importedAssets)
            {
                if (PixelArtAtlasPath.IsMatch(importedAsset))
                {
                    SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(importedAsset);
                    spriteAtlas.SetTextureSettings(AtlasTextureSettings);
                    spriteAtlas.SetPlatformSettings(AtlasPlatformSettings);
                    EditorUtility.SetDirty(spriteAtlas);
                }
            }
        }
    }
}