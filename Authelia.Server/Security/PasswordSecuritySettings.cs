using Authelia.Server.Helpers;

namespace Authelia.Server.Security
{
    public class PasswordSecuritySettings
    {
        public const string LowercaseMatch = "(?=.*?[a-z])";
        public const string UppercaseMatch = "(?=.*?[A-Z])";
        public const string NumericDigitsMatch = "(?=.*?[0-9])";
        public const string SpecialCharMatch = "(?=.*?[!@#$%^&*()_+\\-=\\[\\]{};':\"\\|,.<>\\/?])";
        public const int MaxPasswordLength = 50;

        public static PasswordSecuritySettings Default { get; set; } = new PasswordSecuritySettings();
        public static PasswordSecuritySettings Admin { get; set; } = new PasswordSecuritySettings()
        {
            MinLength = 8,
            Security = PasswordSecurityCheck.LowercaseLetters | PasswordSecurityCheck.UppercaseLetters | PasswordSecurityCheck.NumericDigits | PasswordSecurityCheck.SpecialCharacters
        };

        public uint MinLength { get; set; } = 8;

        public PasswordSecurityCheck Security { get; set; } = PasswordSecurityCheck.LowercaseLetters | PasswordSecurityCheck.NumericDigits;

        public string BuildRegex()
        {
            var builder = Pools.StringBuilderPool.Create();

            builder.Append('^');

            if ((Security & PasswordSecurityCheck.LowercaseLetters) == PasswordSecurityCheck.LowercaseLetters) builder.Append(LowercaseMatch);
            if ((Security & PasswordSecurityCheck.UppercaseLetters) == PasswordSecurityCheck.UppercaseLetters) builder.Append(UppercaseMatch);
            if ((Security & PasswordSecurityCheck.NumericDigits) == PasswordSecurityCheck.NumericDigits) builder.Append(NumericDigitsMatch);
            if ((Security & PasswordSecurityCheck.SpecialCharacters) == PasswordSecurityCheck.SpecialCharacters) builder.Append(SpecialCharMatch);

            builder.Append(".{");
            builder.Append(MinLength);
            builder.Append(',');
            builder.Append(MaxPasswordLength);
            builder.Append("}$");

            var result = builder.ToString();
            Pools.StringBuilderPool.Return(builder);

            return result;
        }
    }

    public enum PasswordSecurityCheck
    {
        LowercaseLetters = 1,
        UppercaseLetters = 2,
        NumericDigits = 4,
        SpecialCharacters = 8
    }
}
