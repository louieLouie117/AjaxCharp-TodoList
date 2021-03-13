using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjaxCsharp.Models
{
    public class indexWrapper
    {

        public LoginUser LogInAUser { get; set; }
        public User regUser { get; set; }


    }
}