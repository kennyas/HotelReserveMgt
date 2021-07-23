﻿using HotelReserveMgt.Core.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReserveMgt.Core.Domain.Entities
{
    public class Customer : AuditableBaseEntity
    {
        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Address { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonElement("PhoneNumber")]
        public string Contact { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}