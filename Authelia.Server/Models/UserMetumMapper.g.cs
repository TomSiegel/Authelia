namespace Authelia.Database.Model
{
    public static partial class UserMetumMapper
    {
        public static Authelia.Database.Model.UserMetum AdaptToUserMetum(this Authelia.Database.Model.UserMetumDto p1)
        {
            return p1 == null ? null : new Authelia.Database.Model.UserMetum()
            {
                UserId = p1.UserId,
                UserMetaKey = p1.UserMetaKey,
                UserMetaValue = p1.UserMetaValue
            };
        }
        public static Authelia.Database.Model.UserMetum AdaptTo(this Authelia.Database.Model.UserMetumDto p2, Authelia.Database.Model.UserMetum p3)
        {
            if (p2 == null)
            {
                return null;
            }
            Authelia.Database.Model.UserMetum result = p3 ?? new Authelia.Database.Model.UserMetum();
            
            result.UserId = p2.UserId;
            result.UserMetaKey = p2.UserMetaKey;
            result.UserMetaValue = p2.UserMetaValue;
            return result;
            
        }
        public static Authelia.Database.Model.UserMetumDto AdaptToDto(this Authelia.Database.Model.UserMetum p4)
        {
            return p4 == null ? null : new Authelia.Database.Model.UserMetumDto()
            {
                UserId = p4.UserId,
                UserMetaKey = p4.UserMetaKey,
                UserMetaValue = p4.UserMetaValue
            };
        }
        public static Authelia.Database.Model.UserMetumDto AdaptTo(this Authelia.Database.Model.UserMetum p5, Authelia.Database.Model.UserMetumDto p6)
        {
            if (p5 == null)
            {
                return null;
            }
            Authelia.Database.Model.UserMetumDto result = p6 ?? new Authelia.Database.Model.UserMetumDto();
            
            result.UserId = p5.UserId;
            result.UserMetaKey = p5.UserMetaKey;
            result.UserMetaValue = p5.UserMetaValue;
            return result;
            
        }
    }
}