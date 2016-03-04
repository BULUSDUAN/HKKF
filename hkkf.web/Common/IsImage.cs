using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace hkkf.Web.Common
{
    public static class IsImage
    {
        public static bool IsImage1(string virturePath)
        {
            string PhysicalPath = HttpContext.Current.Server.MapPath(virturePath);
            FileStream fileStream=new FileStream(PhysicalPath,FileMode.Open,FileAccess.Read);
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(fileStream, true, true);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}