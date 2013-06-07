using System.Collections.Generic;
using System.Web.Http;

namespace Dev.Demo.Web.Controllers
{
    using Dev.Demo.Application.MainBoundedContext.TestModule;
    using Dev.Demo.ViewModels;

    public class AController : ApiController
    {

        private ITestService TestService;



        public AController(ITestService TestService)
        {

            this.TestService = TestService;
        }


        // GET api/default1
        public IEnumerable<string> Get()
        {
            string[] s = TestService.GetData();

            //TestService.GetMostViews(2);
            return s;
        }

        // GET api/default1/5
        public PurviewGroup Get(int id)
        {
            PurviewGroup v = new PurviewGroup
            {
                GroupName = "gm",
                PurviewKeyValues =
                    new List<KeyValuePair<string, string>>
                                             {
                                                 new KeyValuePair
                                                     <string, string>(
                                                     "1,", "2"),
                                                 new KeyValuePair
                                                     <string, string>(
                                                     "1,", "2")
                                             }
            };

            return v;

        }

        // POST api/default1
        public void Post([FromBody]string value)
        {
        }

        // PUT api/default1/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/default1/5
        public void Delete(int id)
        {
        }
    }
}
