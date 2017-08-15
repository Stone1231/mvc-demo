using Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : baseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var model = new MyTestModel();

            bind(model);

            model.Text4 = DateTime.Today;

            return View(model);
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public ActionResult Test(MyTestModel model)
        {
            if (!ModelState.IsValid)
            {
                //return Index();
                bind(model);
                return View(model);
            }

            bind(model);

            if (model.File != null)
            {
                var httpPath2 = HttpContext.Server.MapPath("~/Files");
                if (!Directory.Exists(httpPath2))
                {
                    Directory.CreateDirectory(httpPath2);
                }

                //var fileName = Path.GetFileName(file.FileName);
                var pathFileName = Path.Combine(httpPath2, model.File.FileName);
                if (System.IO.File.Exists(pathFileName))
                {
                    System.IO.File.Delete(pathFileName);
                }
                model.File.SaveAs(pathFileName);
            }


            if (model.File != null)
            {
                byte[] uploadedFile = new byte[model.File.InputStream.Length];
                model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
            }

            var httpPath = HttpContext.Server.MapPath("~/Files");
            if (!Directory.Exists(httpPath))
            {
                Directory.CreateDirectory(httpPath); //路徑不存在自已建立
            }

            foreach (HttpPostedFileBase file in model.Files)
            {
                if (file != null)
                {
                    //var fileName = Path.GetFileName(file.FileName);
                    var pathFileName = Path.Combine(httpPath, file.FileName);
                    file.SaveAs(pathFileName);
                }
            }

            //return Content("Thanks for uploading the file");
            return View(model);
        }

        void bind(MyTestModel model)
        {
            model.Select1 = StudentBiz.GetAll().Select(s => new SelectListItem
            {
                Value = s.Sn.ToString(),
                Text = s.Name
            });

            model.Select2 = StudentBiz.GetAll().Select(s => new SelectListItem
            {
                Value = s.Sn.ToString(),
                Text = s.Name
            });

            //班級
            model.ClassSelect = ClassMBiz.GetAll().Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            });

            if (!string.IsNullOrWhiteSpace(model.ClassId))
            {
                var list = StudentBiz.ClassData(model.ClassId).Select(s => new SelectListItem
                {
                    Value = s.Sn.ToString(),
                    Text = s.Name
                });

                model.StudentSelect = list;
            }
            else
            {
                model.StudentSelect = new List<SelectListItem>();
            }
        }

        public ActionResult ClassStud(string id)
        {
            //var defItem = new SelectListItem { Text = "請選擇", Value = "" };

            var list = StudentBiz.ClassData(id).Select(s => new SelectListItem
            {
                Value = s.Sn.ToString(),
                Text = s.Name
            }).ToList();

            //list.Insert(0, defItem);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            //Logout();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModels User, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                //SysUserMInfo info = SysUserMBiz.GetInfo(User.UserId);
                if (!string.IsNullOrWhiteSpace(User.UserId) && User.Pwd == User.Pwd)
                {
                    SessionModels sessionModel = new SessionModels();
                    sessionModel.UserId = User.UserId;

                    HttpContext.Session[AuthorizeUserAttribute.SessionId] = sessionModel;

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }
    }
}