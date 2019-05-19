﻿using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace AWS.SignatureVersion4.Integration.Contents
{
    public static class Extensions
    {
        public static StringContent ToContent(this Type self) =>
            new StringContent(
                JsonConvert.SerializeObject(Activator.CreateInstance(self)),
                Encoding.UTF8,
                "application/json");
    }
}
