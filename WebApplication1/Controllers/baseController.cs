using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Information;
namespace WebApplication1.Controllers
{
    public class baseController : Controller
    {
        public JsonResult ReturnJson(bool issuccess = true, string msg = "")
        {
            //var errorModel = 
            //from x in ModelState.Keys
            //where ModelState[x].Errors.Count > 0
            //select new
            //{
            //    key = x,
            //    errors = ModelState[x].Errors.
            //                           Select(y => y.ErrorMessage).
            //                           ToArray()
            //};
            //var result = new { success = true, error = errorModel };

            return new JsonResult()
            {
                Data = new { success = issuccess, message = msg }
            };
        }
    }
}
