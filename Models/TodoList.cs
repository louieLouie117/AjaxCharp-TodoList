using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AjaxCsharp.Models
{
    public class TodoList
    {
        [Key]
        public int TodoListId { get; set; }


        public string Item { get; set; }



        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        // Relashinship-----------------------------------------
        // Foregin Key for O2M
        public int UserId { get; set; }
        // Nav Property for O2M
        public User User { get; set; }






    }
}