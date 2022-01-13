namespace Authelia.Database.Model
{
    public static partial class UserMapper
    {
        public static Authelia.Database.Model.User AdaptToUser(this Authelia.Database.Model.UserDto p1)
        {
            return p1 == null ? null : new Authelia.Database.Model.User()
            {
                UserId = p1.UserId,
                UserName = p1.UserName,
                UserPassword = p1.UserPassword,
                UserOrder = p1.UserOrder,
                UserCreated = p1.UserCreated,
                UserCreatorIp = p1.UserCreatorIp,
                UserVerified = p1.UserVerified,
                UserMail = p1.UserMail
            };
        }
        public static Authelia.Database.Model.User AdaptTo(this Authelia.Database.Model.UserDto p2, Authelia.Database.Model.User p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Authelia.Database.Model.User result = p3 ?? new Authelia.Database.Model.User();
            
            result.UserId = p2.UserId;
            result.UserName = p2.UserName;
            result.UserPassword = p2.UserPassword;
            result.UserOrder = p2.UserOrder;
            result.UserCreated = p2.UserCreated;
            result.UserCreatorIp = p2.UserCreatorIp;
            result.UserVerified = p2.UserVerified;
            result.UserMail = p2.UserMail;
            return result;
            
        }
        public static Authelia.Database.Model.UserDto AdaptToDto(this Authelia.Database.Model.User p4)
        {
            return p4 == null ? null : new Authelia.Database.Model.UserDto()
            {
                UserId = p4.UserId,
                UserName = p4.UserName,
                UserPassword = p4.UserPassword,
                UserOrder = p4.UserOrder,
                UserCreated = p4.UserCreated,
                UserCreatorIp = p4.UserCreatorIp,
                UserVerified = p4.UserVerified,
                UserMail = p4.UserMail
            };
        }
        public static Authelia.Database.Model.UserDto AdaptTo(this Authelia.Database.Model.User p5, Authelia.Database.Model.UserDto p6)
        {
            if (p5 == null)
            {
                return null;
            }
            Authelia.Database.Model.UserDto result = p6 ?? new Authelia.Database.Model.UserDto();
            
            result.UserId = p5.UserId;
            result.UserName = p5.UserName;
            result.UserPassword = p5.UserPassword;
            result.UserOrder = p5.UserOrder;
            result.UserCreated = p5.UserCreated;
            result.UserCreatorIp = p5.UserCreatorIp;
            result.UserVerified = p5.UserVerified;
            result.UserMail = p5.UserMail;
            return result;
            
        }
    }
}