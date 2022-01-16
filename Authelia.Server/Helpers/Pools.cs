using Microsoft.Extensions.ObjectPool;

namespace Authelia.Server.Helpers
{
    public class Pools
    {
        public static StringBuilderPooledObjectPolicy StringBuilderPool { get; } = new StringBuilderPooledObjectPolicy();
    }
}
