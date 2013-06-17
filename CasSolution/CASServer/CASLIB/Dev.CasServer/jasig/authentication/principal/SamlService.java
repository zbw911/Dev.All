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
package org.jasig.cas.authentication.principal;

import java.io.BufferedReader;
import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.jasig.cas.util.HttpClient;
import org.springframework.util.StringUtils;

/**
 * Class to represent that this service wants to use SAML. We use this in
 * combination with the CentralAuthenticationServiceImpl to choose the right
 * UniqueTicketIdGenerator.
 * 
 * @author Scott Battaglia
 * @version $Revision: 1.6 $ $Date: 2007/02/27 19:31:58 $
 * @since 3.1
 */
public  class SamlService extends AbstractWebApplicationService {

    private static  Log log = LogFactory.getLog(SamlService.class);

    /** Constant representing service. */
    private static  String CONST_PARAM_SERVICE = "TARGET";

    /** Constant representing artifact. */
    private static  String CONST_PARAM_TICKET = "SAMLart";

    private static  String CONST_START_ARTIFACT_XML_TAG_NO_NAMESPACE = "<AssertionArtifact>";

    private static  String CONST_END_ARTIFACT_XML_TAG_NO_NAMESPACE = "</AssertionArtifact>";
    
    private static  String CONST_START_ARTIFACT_XML_TAG = "<samlp:AssertionArtifact>";
    
    private static  String CONST_END_ARTIFACT_XML_TAG = "</samlp:AssertionArtifact>";

    private String requestId;

    /**
     * Unique Id for serialization.
     */
    private static  long serialVersionUID = -6867572626767140223L;

    protected SamlService( String id) {
        super(id, id, null, new HttpClient());
    }

    protected SamlService( String id,  String originalUrl,  String artifactId,  HttpClient httpClient,  String requestId) {
        super(id, originalUrl, artifactId, httpClient);
        this.requestId = requestId;
    }

    /**
     * This always returns true because a SAML Service does not receive the TARGET value on validation.
     */
    public boolean matches( Service service) {
        return true;
    }

    public String getRequestID() {
        return this.requestId;
    }

    public static SamlService createServiceFrom(
         HttpServletRequest request,  HttpClient httpClient) {
         String service = request.getParameter(CONST_PARAM_SERVICE);
         String artifactId;
         String requestBody = getRequestBody(request);
         String requestId;
        
        if (!StringUtils.hasText(service) && !StringUtils.hasText(requestBody)) {
            return null;
        }

         String id = cleanupUrl(service);
        
        if (StringUtils.hasText(requestBody)) {

             String tagStart;
             String tagEnd;
            if (requestBody.contains(CONST_START_ARTIFACT_XML_TAG)) {
                tagStart = CONST_START_ARTIFACT_XML_TAG;
                tagEnd = CONST_END_ARTIFACT_XML_TAG;
            } else {
                tagStart = CONST_START_ARTIFACT_XML_TAG_NO_NAMESPACE;
                tagEnd = CONST_END_ARTIFACT_XML_TAG_NO_NAMESPACE;
            }
             int startTagLocation = requestBody.indexOf(tagStart);
             int artifactStartLocation = startTagLocation + tagStart.length();
             int endTagLocation = requestBody.indexOf(tagEnd);

            artifactId = requestBody.substring(artifactStartLocation, endTagLocation).trim();

            // is there a request id?
            requestId = extractRequestId(requestBody);
        } else {
            artifactId = null;
            requestId = null;
        }

        if (log.isDebugEnabled()) {
            log.debug("Attempted to extract Request from HttpServletRequest.  Results:");
            log.debug(String.format("Request Body: %s", requestBody));
            log.debug(String.format("Extracted ArtifactId: %s", artifactId));
            log.debug(String.format("Extracted Request Id: %s", requestId));
        }

        return new SamlService(id, service, artifactId, httpClient, requestId);
    }

    public Response getResponse( String ticketId) {
         Map<String, String> parameters = new HashMap<String, String>();

        parameters.put(CONST_PARAM_TICKET, ticketId);
        parameters.put(CONST_PARAM_SERVICE, getOriginalUrl());

        return Response.getRedirectResponse(getOriginalUrl(), parameters);
    }

    protected static String extractRequestId( String requestBody) {
        if (!requestBody.contains("RequestID")) {
            return null;
        }

        try {
             int position = requestBody.indexOf("RequestID=\"") + 11;
             int nextPosition = requestBody.indexOf("\"", position);

            return requestBody.substring(position,  nextPosition);
        } catch ( Exception e) {
            log.debug("Exception parsing RequestID from request." ,e);
            return null;
        }
    }
    
    protected static String getRequestBody( HttpServletRequest request) {
         StringBuilder builder = new StringBuilder();
        try {
             BufferedReader reader = request.getReader();
            
            String line;
            while ((line = reader.readLine()) != null) {
                builder.append(line);
            }
            return builder.toString();
        } catch ( Exception e) {
           return null;
        }
    }
}
