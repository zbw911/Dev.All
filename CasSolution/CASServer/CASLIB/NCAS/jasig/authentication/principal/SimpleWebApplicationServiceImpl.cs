/*
 * Licensed to Jasig under one or more contributor license
 * agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * Jasig licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License.  You may obtain a
 * copy of the License at the following location:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System.Collections.Generic;
using System;
using System.Web;
using Dev.CasServer.jasig.util;

////package org.jasig.cas.authentication.principal;

////import java.util.HashMap;
////import java.util.Map;

////import javax.servlet.http.HttpRequest;

////import org.jasig.cas.authentication.principal.Response.ResponseType;
////import org.jasig.cas.util.HttpClient;
////import org.springframework.util.StringUtils;

/**
 * Represents a service which wishes to use the CAS protocol.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.3 $ $Date: 2007/04/24 18:19:22 $
 * @since 3.1
 */
namespace Dev.CasServer.principal
{
    public class SimpleWebApplicationServiceImpl : AbstractWebApplicationService
    {

        private static String CONST_PARAM_SERVICE = "service";

        private static String CONST_PARAM_TARGET_SERVICE = "targetService";

        private static String CONST_PARAM_TICKET = "ticket";

        private static String CONST_PARAM_METHOD = "method";

        private Dev.CasServer.principal.Response.ResponseType responseType;

        /**
     * Unique Id for Serialization
     */
        private static long serialVersionUID = 8334068957483758042L;

        public SimpleWebApplicationServiceImpl(String id)
            : this(id, id, null, Response.ResponseType.REDIRECT, null)
        {
            ;
        }

        public SimpleWebApplicationServiceImpl(String id, HttpClient httpClient)
            : this(id, id, null, Response.ResponseType.REDIRECT, httpClient)
        {
            ;
        }

        private SimpleWebApplicationServiceImpl(String id,
                                                String originalUrl, String artifactId,
                                                Response.ResponseType responseType, HttpClient httpClient)
            : base(id, originalUrl, artifactId, httpClient)
        {
            ;
            this.responseType = responseType;
        }

        public static SimpleWebApplicationServiceImpl createServiceFrom(HttpRequest request)
        {
            return createServiceFrom(request, null);
        }

        public static SimpleWebApplicationServiceImpl createServiceFrom(
            HttpRequest request, HttpClient httpClient)
        {
            String targetService = request.Params[CONST_PARAM_TARGET_SERVICE];
            String method = request.Params[CONST_PARAM_METHOD];
            String serviceToUse = !string.IsNullOrEmpty(targetService)
                                      ? targetService : request.Params[CONST_PARAM_SERVICE];

            if (!string.IsNullOrEmpty(serviceToUse))
            {
                return null;
            }

            String id = cleanupUrl(serviceToUse);
            String artifactId = request.Params[CONST_PARAM_TICKET];

            return new SimpleWebApplicationServiceImpl(id, serviceToUse,
                                                       artifactId, "POST".Equals(method)
                                                       ? Response.ResponseType.POST
                                                                       : Response.ResponseType.REDIRECT, httpClient);
        }

        public override Response getResponse(String ticketId)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            if (!string.IsNullOrEmpty(ticketId))
            {
                parameters.Add(CONST_PARAM_TICKET, ticketId);
            }

            if (Response.ResponseType.POST == this.responseType)
            {
                return Response.getPostResponse(this.getOriginalUrl(), parameters);
            }
            return Response.getRedirectResponse(this.getOriginalUrl(), parameters);
        }
    }
}
