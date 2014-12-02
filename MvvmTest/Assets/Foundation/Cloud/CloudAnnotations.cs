using System;

namespace Foundation.Cloud
{
    /// <summary>
    /// Defines the protection level for a Storage Object
    /// </summary>
    public enum StorageACL
    {
        Public,
        User,
        Admin,
        // Group,
    }

    /// <summary>
    /// Decorate a class to make it usable by the storage system
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StorageTable : Attribute
    {
        public string TableName { get; protected set; }

        public StorageTable(string tableName)
        {
            TableName = tableName;
        }
    }

    /// <summary>
    /// Decorate your unique id. This should be a GUID string
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StorageIdentity : Attribute
    {

    }

    /// <summary>
    /// Optional, Default Order by Property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StorageScore : Attribute
    {

    }

}