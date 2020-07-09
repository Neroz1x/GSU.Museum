﻿using GSU.Museum.Shared.Data.Enums;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace GSU.Museum.Shared.Data.Models
{

    [DataContract(IsReference = false)]
    public class Error : Exception
    {
        public Error(Errors errorCode, string message)
        {
            ErrorCode = errorCode;
            Info = message;
        }

        public Error()
        { }

        [DataMember]
        public Errors ErrorCode { get; set; }

        [DataMember]
        public string Info { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}