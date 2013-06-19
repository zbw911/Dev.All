using System;

namespace NCAS.jasig.web.MOCK2JAVA
{
    public class ServletRequestDataBinder
    {
        private Dev.CasServer.jasig.validation.ValidationSpecification validationSpecification;
        private string p;

        public ServletRequestDataBinder(Dev.CasServer.jasig.validation.ValidationSpecification validationSpecification, string p)
        {
            // TODO: Complete member initialization
            this.validationSpecification = validationSpecification;
            this.p = p;
        }
        public void setRequiredFields(string renew)
        {
            throw new NotImplementedException();
        }

        internal void bind(System.Web.HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
