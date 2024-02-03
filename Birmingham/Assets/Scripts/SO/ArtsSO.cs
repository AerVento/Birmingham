using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Game.Art
{
    [CreateAssetMenu(fileName = "NewArtsSOFile", menuName = "SO/ArtsSO")]
    public class ArtsSO : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<string, Sprite> _spriteDic;

        /// <summary>
        /// 以图片名获取图片
        /// </summary>
        /// <param name="name">图片名</param>
        /// <returns>图片</returns>
        public Sprite GetSprite(string name)
        {
            if (_spriteDic.TryGetValue(name, out var sprite))
            {
                return sprite;
            }
            return null;
        }
    }
}
