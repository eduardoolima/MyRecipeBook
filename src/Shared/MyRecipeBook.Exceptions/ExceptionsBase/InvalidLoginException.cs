using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : MyRecipeBookException
    {
        public InvalidLoginException() : base(ResourceMessagesException.Email_or_Password_Invalid)
        {
        }
    }
}
