using Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;  //Compare受影響

namespace WebApplication1
{
    public class MyTestModel
    {
        [DisplayName("檔案上傳")]
        public HttpPostedFileBase File { get; set; }

        [DisplayName("多檔案上傳")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        //public SelectList DoctorSelect { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Select1 { get; set; }

        [DisplayName("單選-1")]
        [Required(ErrorMessage = "一定要填啦!")]
        public string Id1 { get; set; }

        [DisplayName("單選-2")]
        [Required(ErrorMessage = "一定要填啦!")]
        public string Id2 { get; set; }

        [MustHaveOne(ErrorMessage = "ListBox一定要選一筆以上")]
        [DisplayName("複選-1")]
        public IList<int>List1 { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Select2 { get; set; }
        [DisplayName("複選-2")]
        public string[] List2
        {
            get
            {
                if (Select2 == null) {
                    return null;
                }
                var data = Select2.Where(w => w.Selected);
                if (data.Count() > 0)
                {
                    return data.Select(s => s.Value).ToArray();
                }
                return null;
            }
        }

        [DisplayName("數字(2~5位)")]
        //[Required(ErrorMessage = "要填啦!")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "Foo1")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Foo3")]
        public string Text1 { get; set; }

        [DisplayName("信用卡")]
        [CreditCard(ErrorMessage = "要填信用卡啦!")]
        public string Text2 { get; set; }

        [DisplayName("電子郵件")]
        [EmailAddress(ErrorMessage = "要填電子郵件啦!")]
        public string Text3 { get; set; }

        [DisplayName("電子郵件2")]
        [Compare("Text32", ErrorMessage = "電子郵件2與電子郵件不同")]
        public string Text32 { get; set; }

        [DisplayName("日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Text4 { get; set; }

        [DisplayName("數字")]
        [Required(ErrorMessage = "Please enter how many Stream Entries are displayed per page.")]
        [Range(0, 250, ErrorMessage = "Please enter a number between 0 and 250.")]
        public int Text5 { get; set; }

        [DisplayName("會員密碼")]
        //[Required(ErrorMessage = "您必須輸入密碼！")]
        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [StringLength(30, MinimumLength = 6, ErrorMessage = "會員帳號的長度需再6~30個字元內！")]
        [RegularExpression(@"[a-zA-Z]+[a-zA-Z0-9]*$", ErrorMessage = "密碼僅能有英文或數字，且開頭需為英文字母！")]
        public string Text6 { get; set; }

        [DisplayName("勾選")]
        public bool Check1 { get; set; }

        [DisplayName("班級")]
        public string ClassId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ClassSelect { get; set; }

        [DisplayName("學生")]
        [Required(ErrorMessage = "您必須輸入學生！")]
        public string StudentId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> StudentSelect { get; set; }
    }
}