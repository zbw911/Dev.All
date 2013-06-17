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
package org.jasig.cas.web.support;

import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.jasig.cas.authentication.principal.WebApplicationService;
import org.springframework.util.Assert;
import org.springframework.webflow.context.servlet.ServletExternalContext;
import org.springframework.webflow.execution.RequestContext;

/**
 * Common utilities for the web tier.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */
public  class WebUtils {

    /** Request attribute that contains message key describing details of authorization failure.*/
    public static  String CAS_ACCESS_DENIED_REASON = "CAS_ACCESS_DENIED_REASON";

    public static HttpServletRequest getHttpServletRequest(
         RequestContext context) {
        Assert.isInstanceOf(ServletExternalContext.class, context
            .getExternalContext(),
            "Cannot obtain HttpServletRequest from event of type: "
                + context.getExternalContext().getClass().getName());

        return (HttpServletRequest) context.getExternalContext().getNativeRequest();
    }

    public static HttpServletResponse getHttpServletResponse(
         RequestContext context) {
        Assert.isInstanceOf(ServletExternalContext.class, context
            .getExternalContext(),
            "Cannot obtain HttpServletResponse from event of type: "
                + context.getExternalContext().getClass().getName());
        return (HttpServletResponse) context.getExternalContext()
            .getNativeResponse();
    }

    public static WebApplicationService getService(
         List<ArgumentExtractor> argumentExtractors,
         HttpServletRequest request) {
        for ( ArgumentExtractor argumentExtractor : argumentExtractors) {
             WebApplicationService service = argumentExtractor
                .extractService(request);

            if (service != null) {
                return service;
            }
        }

        return null;
    }
    
    public static WebApplicationService getService(
         List<ArgumentExtractor> argumentExtractors,
         RequestContext context) {
         HttpServletRequest request = WebUtils.getHttpServletRequest(context);
        return getService(argumentExtractors, request);
    }

    public static WebApplicationService getService(
         RequestContext context) {
        return (WebApplicationService) context.getFlowScope().get("service");
    }

    public static void putTicketGrantingTicketInRequestScope(
         RequestContext context,  String ticketValue) {
        context.getRequestScope().put("ticketGrantingTicketId", ticketValue);
    }

    public static String getTicketGrantingTicketId(
         RequestContext context) {
         String tgtFromRequest = (String) context.getRequestScope().get("ticketGrantingTicketId");
         String tgtFromFlow = (String) context.getFlowScope().get("ticketGrantingTicketId");
        
        return tgtFromRequest != null ? tgtFromRequest : tgtFromFlow;

    }

    public static void putServiceTicketInRequestScope(
         RequestContext context,  String ticketValue) {
        context.getRequestScope().put("serviceTicketId", ticketValue);
    }

    public static String getServiceTicketFromRequestScope(
         RequestContext context) {
        return context.getRequestScope().getString("serviceTicketId");
    }
    
    public static void putLoginTicket( RequestContext context,  String ticket) {
        context.getFlowScope().put("loginTicket", ticket);
    }
    
    public static String getLoginTicketFromFlowScope( RequestContext context) {
        // Getting the saved LT destroys it in support of one-time-use
        // See section 3.5.1 of http://www.jasig.org/cas/protocol
         String lt = (String) context.getFlowScope().remove("loginTicket");
        return lt != null ? lt : "";
    }
    
    public static String getLoginTicketFromRequest( RequestContext context) {
       return context.getRequestParameters().get("lt");
    }
}
