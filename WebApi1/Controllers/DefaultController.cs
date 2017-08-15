using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Business;
using Information;
namespace WebApplication1.Controllers
{
    public class DefaultController : ApiController
    {
        // GET api/default1
        public IEnumerable<StudentInfo> Get()
        {
            return StudentBiz.GetAll();
        }

        // GET api/default1/5
        public StudentInfo Get(int id)
        {
            var info = StudentBiz.GetInfo(id);
            if (info == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return info;
        }

        // POST api/default1
        public HttpResponseMessage Post(StudentInfo info)
        {
            if (ModelState.IsValid)
            {
                var result = StudentBiz.AddNew(info);

                if (result)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, info);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = info.Sn }));
                    return response;
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/default1/5
        public HttpResponseMessage Put(int id, StudentInfo info)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var _info = StudentBiz.GetInfo(id);

            if (id != _info.Sn)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            _info.Name = info.Name;
            _info.Hight = info.Hight;
            _info.Weight = info.Weight;
            _info.Memo = info.Memo;

            var result = StudentBiz.Update(_info);
            if (!result) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/default1/5
        public HttpResponseMessage Delete(int id)
        {
            var info = StudentBiz.GetInfo(id);

            if (info.Sn == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            bool result = StudentBiz.Del(id);

            if (!result)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,"delete error");
            }

            return Request.CreateResponse(HttpStatusCode.OK, info);  
        }
    }
}
