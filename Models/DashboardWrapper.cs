using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjaxCsharp.Models
{
    public class DashboardWrapper
    {

        public TodoList TodoList { get; set; }


        public List<TodoList> TodoListItems { get; set; }





    }
}