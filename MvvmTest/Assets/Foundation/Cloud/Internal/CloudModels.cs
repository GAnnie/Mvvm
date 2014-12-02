using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cloud.Internal
{

    /// <summary>
    /// Used for header messages
    /// </summary>
    public class ResponseMetadata
    {
        /// <summary>
        /// General error title
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validation Errors. apiEntity.PropertyName by errors[]
        /// </summary>
        public Dictionary<string, string[]> ModelState { get; set; }
    }


    /// <summary>
    /// Defines the protection level for a Storage Object
    /// </summary>
    public enum StorageACLType
    {
        Public,
        User,
        Admin,
        // Group,
    }
    
    /// <summary>
    /// Describes a storage object. Object saved in ObjectData as Json.
    /// </summary>
    [UnityRoute("api/Storage")]
    public class StorageRequest
    {
        /// <summary>
        /// Application Id
        /// </summary>
        [Required]
        public string AppId { get; set; }

        /// <summary>
        /// Unique object Id
        /// </summary>
        [Required]
        public string ObjectId { get; set; }

        /// <summary>
        /// Object Table Name, computed
        /// </summary>
        [Required]
        public string ObjectType { get; set; }

        /// <summary>
        /// Object Sort Index, computed
        /// </summary>
        public float ObjectScore { get; set; }

        /// <summary>
        /// Object Table Name, computed
        /// </summary>
        [Required]
        public string ObjectData { get; set; }

        /// <summary>
        /// Book keeping
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Book keeping
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Security. Current Write Protection
        /// </summary>
        public StorageACLType AclType { get; set; }
        /// <summary>
        /// Security Parameter. Current Write Protection (UserId / GroupId)
        /// </summary>
        public string AclParam { get; set; }

        #region ==
        public override int GetHashCode()
        {
            return (ObjectId != null ? ObjectId.GetHashCode() : 0);
        }
        #endregion
    }

    /// <summary>
    /// Increment/Decrement a storage object property
    /// </summary>
    [UnityRoute("api/Storage/UpdateDelta")]
    public class StorageDelta
    {
        /// <summary>
        /// Application Id
        /// </summary>
        [Required]
        public string AppId { get; set; }

        /// <summary>
        /// Unique object Id
        /// </summary>
        [Required]
        public string ObjectId { get; set; }

        /// <summary>
        /// Unique property Name
        /// </summary>
        [Required]
        public string PropertyName { get; set; }

        /// <summary>
        /// Is the object not an int
        /// </summary>
        public bool IsFloat { get; set; }

        /// <summary>
        /// Change
        /// </summary>
        [Required]
        public float Delta { get; set; }

        #region ==
        public override int GetHashCode()
        {
            return (ObjectId != null ? ObjectId.GetHashCode() : 0);
        }
        #endregion
    }


    /// <summary>
    /// Updates a single storage object property
    /// </summary>
    [UnityRoute("api/Storage/UpdateProperty")]
    public class StorageProperty
    {
        /// <summary>
        /// Application Id
        /// </summary>
        [Required]
        public string AppId { get; set; }

        /// <summary>
        /// Unique object Id
        /// </summary>
        [Required]
        public string ObjectId { get; set; }

        /// <summary>
        /// Unique property Name
        /// </summary>
        [Required]
        public string PropertyName { get; set; }

        /// <summary>
        /// Is the object not an int
        /// </summary>
        public string PropertyValue { get; set; }

        #region ==
        public override int GetHashCode()
        {
            return (ObjectId != null ? ObjectId.GetHashCode() : 0);
        }
        #endregion
    }

    //

    /// <summary>
    /// Describes the user Account
    /// </summary>
    public class AccountDetails
    {
        /// <summary>
        /// Unique Id (GUID)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// User's Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Authentication Token. Used for Realtime Messenger
        /// </summary>
        public string AuthenticationToken { get; set; }
        /// <summary>
        /// Key used to acquire the session serverside. Add to header.
        /// </summary>
        public string Session { get; set; }
    }

    /// <summary>
    /// Creates a new account
    /// </summary>
    [UnityRoute("api/Account/SignUp", typeof(AccountDetails))]
    public class AccountSignUpRequest
    {
        /// <summary>
        /// Unique Id for the user.
        /// This can be App:UserName or just a Guid.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }

    /// <summary>
    /// Sign in Request
    /// </summary>
    [UnityRoute("api/Account/SignIn", typeof(AccountDetails))]
    public class AccountSignInRequest
    {
        /// <summary>
        /// User's email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }

    /// <summary>
    /// Authorizes the Messenger
    /// </summary>
    /// <returns></returns>
    [UnityRoute("api/Account/Authorize", typeof(AccountDetails))]
    public class AccountAuthorizeRequest
    {
        public Dictionary<string, string[]> Channels { get; set; }
    }

    /// <summary>
    /// Issues a Token as needed for Update
    /// </summary>
    [UnityRoute("api/Account/Token", typeof(AccountTokenResult))]
    public class AccountTokenRequest
    {
        /// <summary>
        /// User's email
        /// </summary>
        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// If no email is set, returns the token via HTTP. If Email is set, token is Emailed
    /// </summary>
    public class AccountTokenResult
    {
        /// <summary>
        /// Found User
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Was the Token sent via Email
        /// </summary>
        public bool TokenInEmail { get; set; }
        /// <summary>
        /// Null if sent via Email
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Updates an account's details
    /// </summary>
    [UnityRoute("api/Account/Update", typeof(AccountDetails))]
    public class AccountUpdateRequest
    {
        /// <summary>
        /// User's unique id
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Personal Details (Updated)
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Personal Details (Updated)
        /// </summary>
        public string NewEmail { get; set; }
        /// <summary>
        /// Personal Details (Updated)
        /// </summary>
        public string NewPassword { get; set; }
    }

}