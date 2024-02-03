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
        /// ��ͼƬ����ȡͼƬ
        /// </summary>
        /// <param name="name">ͼƬ��</param>
        /// <returns>ͼƬ</returns>
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
