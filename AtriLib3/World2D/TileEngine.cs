using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.World2D
{
    public static class TileEngine
    {
        private static List<TileData> _tdata;
        private static Dictionary<int, Texture2D> _textures;

        static TileEngine()
        {
            _tdata = new List<TileData>();
            _textures = new Dictionary<int, Texture2D>();
        }

        /// <summary>
        /// Add Tile Data to the Tile Engine
        /// </summary>
        /// <param name="key">Tile Data Key</param>
        /// <param name="tileData">TileData object</param>
        public static void AddTileData(TileData tileData)
        {
            _tdata.Add(tileData);
        }

        /// <summary>
        /// Add a Tile Sheet to the Tile Engine
        /// </summary>
        /// <param name="key">Tile Sheet Key</param>
        /// <param name="textureSheet"></param>
        public static void AddTileSheet(int key, Texture2D tileSheet)
        {
            _textures.Add(key, tileSheet);
        }

        /// <summary>
        /// Removes a Tile Sheet from the Tile Engine
        /// </summary>
        /// <param name="key">TileSheet Key</param>
        public static void RemoveTilesheet(int key)
        {
            _textures.Remove(key);
        }

        /// <summary>
        /// Removes TileData from the Tile Engine
        /// </summary>
        /// <param name="key">TileData Key</param>
        public static void RemoveTileData(int index)
        {
            _tdata.RemoveAt(index);
        }

        /// <summary>
        /// Clears All TileData from the Tile Engine
        /// </summary>
        public static void ClearTileData()
        {
            _tdata.Clear();
        }

        /// <summary>
        /// Clears All TileSheets from the Tile Engine
        /// </summary>
        public static void ClearTileSheets()
        {
            _textures.Clear();
        }

        private static Texture2D GetRealTexture(int key)
        {
            foreach(var data in _textures)
            {
                if(data.Key == key)
                {
                    return data.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Texture Information from the TileSheet 
        /// (Use this if you do not intend to copy a texture object.)
        /// </summary>
        /// <param name="key">TileSheet Key</param>
        /// <returns>Texture2DExt</returns>
        public static Texture2DExt GetTexture(string textureID)
        {
            Texture2DExt _ext = new Texture2DExt();

            for(int i = 0; i < _tdata.Count; i++)
            {
                if(_tdata[i].TextureID == textureID)
                {
                    _ext.SourceRectangle = _tdata[i].TileSheetRectangle;
                    _ext.Texture = GetRealTexture(_tdata[i].TextureSheetNum);
                }
            }

            return _ext;
        }
    }

    /// <summary>
    /// Extended Texture2D Struct
    /// </summary>
    public struct Texture2DExt
    {
        public Texture2D Texture;
        public Rectangle SourceRectangle;
    }

    /// <summary>
    /// TileData Struct
    /// </summary>
    public struct TileData
    {
        public int TextureSheetNum;
        public string TextureID;
        public Rectangle TileSheetRectangle;
    }
}
