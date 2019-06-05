using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManagerApp.Models
{
    public class TaskFactory
    {
        private static readonly TaskManagerDBEntities db = new TaskManagerDBEntities();

        public static List<SelectListItem> GetStatusFilter()
        {

            List<SelectListItem> filter = new List<SelectListItem>();
            filter.Add(new SelectListItem { Text = "Choose a Status", Value = "" });
            filter.Add(new SelectListItem { Text = "Completed", Value = "true" });
            filter.Add(new SelectListItem { Text = "Incompleted", Value = "false" });
            return filter;
        }
    }
}