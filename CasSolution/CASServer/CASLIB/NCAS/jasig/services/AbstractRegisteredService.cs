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

////package org.jasig.cas.services;

////import org.apache.commons.lang.StringUtils;
////import org.apache.commons.lang.builder.CompareToBuilder;
////import org.apache.commons.lang.builder.EqualsBuilder;
////import org.apache.commons.lang.builder.HashCodeBuilder;
////import org.apache.commons.lang.builder.ToStringBuilder;
////import org.apache.commons.lang.builder.ToStringStyle;
////import org.hibernate.annotations.IndexColumn;

////import java.io.Serializable;
////import java.util.ArrayList;
////import java.util.List;

////import javax.persistence.DiscriminatorColumn;
////import javax.persistence.ElementCollection;
////import javax.persistence.Entity;
////import javax.persistence.GenerationType;
////import javax.persistence.Id;
////import javax.persistence.Inheritance;
////import javax.persistence.Table;
////import javax.persistence.DiscriminatorType;
////import javax.persistence.GeneratedValue;
////import javax.persistence.JoinTable;
////import javax.persistence.JoinColumn;
////import javax.persistence.Column;
////import javax.persistence.FetchType;

/**
 * Base class for mutable, persistable registered services.
 *
 * @author Marvin S. Addison
 * @author Scott Battaglia
 */
//@Entity
//@Inheritance
//@DiscriminatorColumn(
//        name = "expression_type",
//        length = 15,
//        discriminatorType = DiscriminatorType.STRING,
//        columnDefinition = "VARCHAR(15) DEFAULT 'ant'")
//@Table(name = "RegisteredServiceImpl")

using System.Collections.Generic;
using Dev.CasServer.Juntil;
using Dev.CasServer.jasig.services;
using System;
using System.Runtime.Serialization;
using NCAS.jasig.services;

public abstract class AbstractRegisteredService
        : RegisteredService/*, IComparable<RegisteredService>, ISerializable*/
{

    /** Serialization version marker  */
    private static long serialVersionUID = 7645279151115635245L;

    //@Id
    //@GeneratedValue(strategy = GenerationType.AUTO)
    private long id = -1;

    //@ElementCollection(targetClass = String.class, fetch = FetchType.EAGER)
    //@JoinTable(name = "rs_attributes", joinColumns = @JoinColumn(name = "RegisteredServiceImpl_id"))
    //@Column(name = "a_name", nullable = false)
    //@IndexColumn(name = "a_id")
    private List<String> allowedAttributes = new List<String>();

    private String description;

    protected String serviceId;

    private String name;

    private String theme;

    private bool allowedToProxy = true;

    private bool enabled = true;

    private bool ssoEnabled = true;

    private bool anonymousAccess = false;

    private bool ignoreAttributes = false;

    //@Column(name = "evaluation_order", nullable = false)
    private int evaluationOrder;

    /**
     * Name of the user attribute that this service expects as the value of the username payload in the
     * validate responses.
     */
    //@Column(name = "username_attr", nullable = true, length = 256)
    private String usernameAttribute = null;

    public bool isAnonymousAccess()
    {
        return this.anonymousAccess;
    }

    public void setAnonymousAccess(bool anonymousAccess)
    {
        this.anonymousAccess = anonymousAccess;
    }

    public List<String> getAllowedAttributes()
    {
        return this.allowedAttributes;
    }

    public long getId()
    {
        return this.id;
    }

    public String getDescription()
    {
        return this.description;
    }

    public String getServiceId()
    {
        return this.serviceId;
    }

    public String getName()
    {
        return this.name;
    }

    public String getTheme()
    {
        return this.theme;
    }

    public bool isAllowedToProxy()
    {
        return this.allowedToProxy;
    }

    public bool isEnabled()
    {
        return this.enabled;
    }

    public bool isSsoEnabled()
    {
        return this.ssoEnabled;
    }

    public bool equals(Object o)
    {
        if (o == null)
        {
            return false;
        }

        if (this == o)
        {
            return true;
        }

        if (!(o is AbstractRegisteredService))
        {
            return false;
        }

        AbstractRegisteredService that = (AbstractRegisteredService)o;

        //return new EqualsBuilder()
        //          .Append(this.allowedToProxy, that.allowedToProxy)
        //          .Append(this.anonymousAccess, that.anonymousAccess)
        //          .Append(this.enabled, that.enabled)
        //          .Append(this.evaluationOrder, that.evaluationOrder)
        //          .Append(this.ignoreAttributes, that.ignoreAttributes)
        //          .Append(this.ssoEnabled, that.ssoEnabled)
        //          .Append(this.allowedAttributes, that.allowedAttributes)
        //          .Append(this.description, that.description)
        //          .Append(this.name, that.name)
        //          .Append(this.serviceId, that.serviceId)
        //          .Append(this.theme, that.theme)
        //          .Append(this.usernameAttribute, that.usernameAttribute)
        //          .isEquals();
        throw new NotImplementedException();
    }

    public int hashCode()
    {
        //return new HashCodeBuilder(7, 31)
        //          .Append(this.allowedAttributes)
        //          .Append(this.description)
        //          .Append(this.serviceId)
        //          .Append(this.name)
        //          .Append(this.theme)
        //          .Append(this.enabled)
        //          .Append(this.ssoEnabled)
        //          .Append(this.anonymousAccess)
        //          .Append(this.ignoreAttributes)
        //          .Append(this.evaluationOrder)
        //          .Append(this.usernameAttribute)
        //          .toHashCode();

        throw new NotImplementedException();
    }

    public void setAllowedAttributes(List<String> allowedAttributes)
    {
        if (allowedAttributes == null)
        {
            this.allowedAttributes = new List<String>();
        }
        else
        {
            this.allowedAttributes = allowedAttributes;
        }
    }

    public void setAllowedToProxy(bool allowedToProxy)
    {
        this.allowedToProxy = allowedToProxy;
    }

    public void setDescription(String description)
    {
        this.description = description;
    }

    public void setEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public abstract void setServiceId(String id);

    public void setId(long id)
    {
        this.id = id;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public void setSsoEnabled(bool ssoEnabled)
    {
        this.ssoEnabled = ssoEnabled;
    }

    public void setTheme(String theme)
    {
        this.theme = theme;
    }

    public bool isIgnoreAttributes()
    {
        return this.ignoreAttributes;
    }

    public void setIgnoreAttributes(bool ignoreAttributes)
    {
        this.ignoreAttributes = ignoreAttributes;
    }

    public void setEvaluationOrder(int evaluationOrder)
    {
        this.evaluationOrder = evaluationOrder;
    }

    public int getEvaluationOrder()
    {
        return this.evaluationOrder;
    }

    public String getUsernameAttribute()
    {
        return this.usernameAttribute;
    }

    public abstract bool matches(Service service);


    /**
     * Sets the name of the user attribute to use as the username when providing usernames to this registered service.
     * 
     * <p>Note: The username attribute will have no affect on services that are marked for anonymous access.
     * 
     * @param username attribute to release for this service that may be one of the following values: 
     * <ul>
     *  <li>name of the attribute this service prefers to consume as username</li>. 
     *  <li><code>null</code> to enforce default CAS behavior</li>
     * </ul>
     * @see #isAnonymousAccess()
     */
    public void setUsernameAttribute(String username)
    {
        if (string.IsNullOrEmpty(username))
        {
            this.usernameAttribute = null;
        }
        else
        {
            this.usernameAttribute = username;
        }
    }

    //public object clone()
    //{
    //    AbstractRegisteredService clone = newInstance();
    //    clone.copyFrom(this);
    //    return clone;
    //}

    /**
     * Copies the properties of the source service into this instance.
     *
     * @param source Source service from which to copy properties.
     */
    public void copyFrom(RegisteredService source)
    {
        this.setId(source.getId());
        this.setAllowedAttributes(new List<String>(source.getAllowedAttributes()));
        this.setAllowedToProxy(source.isAllowedToProxy());
        this.setDescription(source.getDescription());
        this.setEnabled(source.isEnabled());
        this.setName(source.getName());
        this.setServiceId(source.getServiceId());
        this.setSsoEnabled(source.isSsoEnabled());
        this.setTheme(source.getTheme());
        this.setAnonymousAccess(source.isAnonymousAccess());
        this.setIgnoreAttributes(source.isIgnoreAttributes());
        this.setEvaluationOrder(source.getEvaluationOrder());
        this.setUsernameAttribute(source.getUsernameAttribute());
    }

    /**
     * Compares this instance with the <code>other</code> registered service based on 
     * evaluation order, name. The name comparison is case insensitive.
     * 
     * @see #getEvaluationOrder()
     */
    public int compareTo(RegisteredService other)
    {
        //return new CompareToBuilder()
        //          .Append(this.getEvaluationOrder(), other.getEvaluationOrder())
        //          .Append(this.getName().toLowerCase(), other.getName().toLowerCase())
        //          .toComparison();

        throw new NotImplementedException();
    }

    public String toString()
    {
        ToStringBuilder<AbstractRegisteredService> toStringBuilder = new ToStringBuilder<AbstractRegisteredService>(this);
        toStringBuilder.Append(x => x.id);
        toStringBuilder.Append(x => x.name);
        toStringBuilder.Append(x => x.description);
        toStringBuilder.Append(x => x.serviceId);
        toStringBuilder.Append(x => x.usernameAttribute);
        toStringBuilder.Append(x => x.allowedAttributes.ToArray());

        return toStringBuilder.ToString();
    }

    protected abstract AbstractRegisteredService newInstance();
    //public abstract object Clone();
    //public abstract int CompareTo(RegisteredService other);
    //public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    //public abstract object Clone();
    public object Clone()
    {
        //return newInstance();

        AbstractRegisteredService clone = newInstance();
        clone.copyFrom(this);
        return clone;
    }
}
