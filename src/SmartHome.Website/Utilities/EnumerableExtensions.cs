
using System.Runtime.CompilerServices;

namespace SmartHome.Website.Utilities
{
    public static class EnumerableExtensions
    {

        public static async Task<ThatEnumerable<T>> ToThatTypeAsync<T>(this Task<IEnumerable<T>> enumerable) => new(await enumerable);

        public static ThatEnumerable<T> ToThatType<T>(this  IEnumerable<T> enumerable) => new(enumerable);

        public class ThatEnumerable<T>
        {
            private readonly IEnumerable<T> content;

            public ThatEnumerable(IEnumerable<T> content)
            {
                this.content = content;
            }

            public static implicit operator T[](ThatEnumerable<T> self)
                => self.content.ToArray();

            public static implicit operator List<T>(ThatEnumerable<T> self)
                => self.content.ToList();

        }

    }
}
