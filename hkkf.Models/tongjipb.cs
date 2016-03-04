using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace hkkf.Models
{
    [DisplayName("排班统计")]
   public class tongjipb:PresentationModel
    {
        virtual public int id { get; set; }
        virtual public DateTime _date { get; set; }
        virtual public string Week { get; set; }

        public virtual List<string> DayPersons { get; set; }
        public virtual List<string> NightPersons { get; set; }

        

        virtual public string Day { get; set; }
     
        //virtual public string DayPerson { get; set; }
        //virtual public string NightPerson { get; set; }
        //virtual public string DayShopName { get; set; }
        //virtual public string NightShopName { get; set; }

    }
}
