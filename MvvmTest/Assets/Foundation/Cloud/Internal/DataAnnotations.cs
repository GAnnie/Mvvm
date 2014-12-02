
namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequiredAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinLength : Attribute
    {
        public int Length { get; set; }

        public MinLength(int length)
        {
            Length = length;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EmailAddressAttribute : Attribute
    {

    }
}