using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManagerApp.Models
{
    

    public class MemberFactory
    {
        private static readonly TaskManagerDBEntities db = new TaskManagerDBEntities();

        public static IEnumerable<SelectListItem> GetMemberSelectedListItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach(Member member in db.Members.ToList())
            {
                SelectListItem item = new SelectListItem();
                item.Text = member.firstName + " " + member.lastName;
                item.Value = member.Id.ToString();

                list.Add(item);
            }
            return list;
        }


        public static Member GetLoggedInMember(string username)
        {
            return db.Members.FirstOrDefault(m => m.Username == username);
        }

    }
}