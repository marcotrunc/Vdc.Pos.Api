using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Vdc.Pos.Domain.DTOs.Requests;
using Microsoft.AspNetCore.Http;

namespace Vdc.Pos.Business.Utilities
{
    public static class Utility
    {
        public static bool IsConnectedToInternet()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("www.google.com");
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        public static void HashString(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public static  bool UploadDocument(IFormFile file, string completePath, string fileNameWithoutExtension, out string fileNameWithPath)
        {
            fileNameWithPath = string.Empty;
            try
            {
                if (!Directory.Exists(completePath))
                {
                    Directory.CreateDirectory(completePath);
                }

                FileInfo fileInfo = new FileInfo(file.FileName);
                string fileName = fileNameWithoutExtension + fileInfo.Extension;
                fileNameWithPath = Path.Combine(completePath, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                     file.CopyTo(stream);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                // Add the logs
                Console.Write(ex.ToString());
                return false;   
            }
        }
        public static bool DeleteDocument(string completePath)
        {

            try
            {
                if (!File.Exists(completePath))
                {
                    return false;
                }
                
                File.Delete(completePath));
                return true;
                
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                return false;
            }
        }
    }

}

