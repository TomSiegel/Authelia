using System.Text.RegularExpressions;

namespace Authelia.Server.Helpers
{
    public static class FormatHelper
    {
        public static string RemoveWhitespace(this string source)
        {
            if (source == null) return null;

            return new string(source
                .ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
