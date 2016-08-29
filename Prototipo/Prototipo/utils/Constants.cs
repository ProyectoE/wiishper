﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo
{
    public static class Constants
    {
        public static string RestURL = "http://mill.com.co/ws/v1{0}{1}";

        //Resources
        public static string UsersResource = "/Users";
        public static string FriendsResource = "/Friends";

        //Actions
        public static string SignUp = "/signup";
        public static string Login = "/login";
        public static string Add = "/add";
    }
}