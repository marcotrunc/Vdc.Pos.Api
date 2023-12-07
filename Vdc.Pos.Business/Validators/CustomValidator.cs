using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Business.Validators
{
    public static class CustomValidator
    {
        public static bool IsEmail(string email) 
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsImageExtensionValid(string imageExtension)
        {
            string[] supportedTypes = new[] { "jpeg", "png", "jpg" };
            if (supportedTypes.Contains(imageExtension))
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
    }
}
