using System.Collections;
using VRC.Core;

namespace FavStack.Utils
{
    public static class Extensions
    {
        public static UnityEngine.Coroutine Start(this IEnumerator enumerator) => Coroutine.Start(enumerator);
        public static void Stop(this UnityEngine.Coroutine coroutine) => Coroutine.Stop(coroutine);
        public static ApiAvatar ToApiAvatar(this FSConfig.FavoriteObject a)
        {
            return new ApiAvatar
            {
                name = a.Name,
                id = a.ID,
                thumbnailImageUrl = a.ThumbnailImageURL
            };
        }
        public static FSConfig.FavoriteObject ToModFavAvi(this ApiAvatar a)
        {
            return new FSConfig.FavoriteObject
            {
                Name = a.name,
                ID = a.id,
                ThumbnailImageURL = a.thumbnailImageUrl
            };
        }
    }
}
