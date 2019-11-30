using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inzynierka.Models
{
    public enum status
    {
        send,
        invite,
        approved,
        canceled
    }
    public class Invite
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Expo Expo { get; set; }
        public status Status { get; set; }
    }
}