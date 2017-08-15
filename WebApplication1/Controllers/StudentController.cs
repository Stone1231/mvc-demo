using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Information;
using System.IO;
using Commons;

namespace WebApplication1.Controllers
{
    public class StudentController : baseController
    {
        public ActionResult Index()
        {
            var model = new StudentListModel();
            var list = StudentBiz.QueryData(" ", " ");

            int currentPageIndex = 0;
            model.List = new MvcPaging.PagedList<StudentInfo>(list, currentPageIndex, 2);

            bindList(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(StudentListModel model, string sortCol, bool? sortDesc, int? page)
        {
            model.Name = string.IsNullOrWhiteSpace(model.Name) ? "" : model.Name;
            model.ClassId = string.IsNullOrWhiteSpace(model.ClassId) ? "" : model.ClassId;
            var list = StudentBiz.QueryData(model.ClassId, model.Name);

            if (!string.IsNullOrWhiteSpace(sortCol))
            {
                ListSort<StudentInfo>.GetList(list, sortCol, sortDesc.Value);
            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int pageSize = 2;
            while (currentPageIndex > 0 &&
                list.Count <= currentPageIndex * pageSize)
            {
                currentPageIndex--;
            }

            model.List = new MvcPaging.PagedList<StudentInfo>(list, currentPageIndex, pageSize);

            return PartialView("List", model.List);
        }

        public ActionResult Create()
        {
            var model = new StudentModel();
            bindEdit(model);
            return PartialView("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(StudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return ReturnJson(false);
            }

            if (model.PhotoFile != null)
            {
                var PhotoPath = HttpContext.Server.MapPath("~/Files/Stud");
                if (!Directory.Exists(PhotoPath))
                {
                    Directory.CreateDirectory(PhotoPath);
                }
                var PhotoFileName = Path.Combine(PhotoPath, model.PhotoFile.FileName);
                if (System.IO.File.Exists(PhotoFileName))
                {
                    System.IO.File.Delete(PhotoFileName);
                }
                model.PhotoFile.SaveAs(PhotoFileName);
                model.Info.Photo = model.PhotoFile.FileName;
            }

            return ReturnJson(StudentBiz.AddNew(model.Info));
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var model = new StudentModel();
                model.Info = StudentBiz.GetInfo((int)id);
                bindEdit(model);
                return PartialView(model);
            }
            return Content("error");
        }

        [HttpPost]
        public ActionResult Edit(StudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return ReturnJson(false);
            }
            var info = StudentBiz.GetInfo(model.Info.Sn);
            info.Sn = model.Info.Sn;
            info.Name = model.Info.Name;
            info.ClassId = model.Info.ClassId;
            info.Hight = model.Info.Hight;
            info.Weight = model.Info.Weight;
            info.Birthday = model.Info.Birthday;
            if (model.PhotoFile != null)
            {
                var PhotoPath = HttpContext.Server.MapPath("~/Files/Stud");
                if (!Directory.Exists(PhotoPath))
                {
                    Directory.CreateDirectory(PhotoPath);
                }
                var PhotoFileName = Path.Combine(PhotoPath, model.PhotoFile.FileName);
                if (System.IO.File.Exists(PhotoFileName))
                {
                    System.IO.File.Delete(PhotoFileName);
                }
                model.PhotoFile.SaveAs(PhotoFileName);
                info.Photo = model.PhotoFile.FileName;
            }
            info.Memo = model.Info.Memo;
            return ReturnJson(StudentBiz.Update(info));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if (StudentBiz.Del(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        void bindEdit(StudentModel model)
        {
            model.ClassIdSelect = ClassMBiz.GetAll().Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name
            });
        }

        void bindList(StudentListModel model)
        {
            model.ClassIdSelect = ClassMBiz.GetAll().Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name
            });
        }
    }
}