namespace Authelia.Server.Exceptions
{
    public static class ErrorCodes
    {
        public const string C_InvalidUserCreationObject = "A-4001";
        public const string C_InvalidUserLoginObject = "A-4002";
        public const string C_UnauthorizedUser = "A-4003";
        public const string C_EmptyEntityList = "A-4004";
        public const string C_InvalidUserMetaCreationObject = "A-4005";
        public const string C_InvalidUserMetaUpdateObject = "A-4006";
        public const string C_DatabaseItemInsertError = "A-4007";
        public const string C_DatabaseItemUpdateError = "A-4008";
        public const string C_DatabaseItemDeleteError = "A-4009";
        public const string C_NullEntity = "A-4010";
        public const string C_InvalidAdminCreationObject = "A-4011";
        public const string C_AdminAlreadyExists = "A-4012";




        public const string S_UnknownServerError = "A-5000";
        public const string S_UserStoreAlreadyCreated = "A-5001";
        public const string S_RoleStoreAlreadyCreated = "A-5002";
        public const string S_ArgumentNull = "A-5003";
        public const string S_DatabaseInsert = "A-5004";
    }
}
