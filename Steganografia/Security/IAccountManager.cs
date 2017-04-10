﻿using System.Web;

namespace Steganografia.Security
{
    public interface IAccountManager
    {
        bool UserExists(string userName, string password);
        void SignIn(string userName, HttpContextBase response);
    }
}