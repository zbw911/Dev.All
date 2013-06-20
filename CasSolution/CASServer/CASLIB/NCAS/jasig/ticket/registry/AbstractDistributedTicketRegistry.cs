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
//package org.jasig.cas.ticket.registry;

//import java.util.List;

//import org.jasig.cas.authentication.Authentication;
//import org.jasig.cas.authentication.principal.Service;
//import org.jasig.cas.ticket.ExpirationPolicy;
//import org.jasig.cas.ticket.ServiceTicket;
//import org.jasig.cas.ticket.Ticket;
//import org.jasig.cas.ticket.TicketGrantingTicket;

/**
 * Abstract Implementation that handles some of the commonalities between
 * distributed ticket registries.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */

using System;
using System.Collections.Generic;
using Dev.CasServer.jasig.authentication;
using NCAS.jasig.ticket;
public abstract class AbstractDistributedTicketRegistry : AbstractTicketRegistry
{

    protected abstract void updateTicket(Ticket ticket);

    protected abstract bool needsCallback();

    protected Ticket getProxiedTicketInstance(Ticket ticket)
    {
        if (ticket == null)
        {
            return null;
        }

        if (ticket is TicketGrantingTicket)
        {
            return new TicketGrantingTicketDelegator(this, (TicketGrantingTicket)ticket, needsCallback());
        }

        return new ServiceTicketDelegator(this, (ServiceTicket)ticket, needsCallback());
    }

    private class TicketDelegator<T> : Ticket where T : Ticket
    {

        private static long serialVersionUID = 1780193477774123440L;

        private AbstractDistributedTicketRegistry ticketRegistry;

        private T ticket;

        private bool callback;

        protected TicketDelegator(AbstractDistributedTicketRegistry ticketRegistry, T ticket, bool callback)
        {
            this.ticketRegistry = ticketRegistry;
            this.ticket = ticket;
            this.callback = callback;
        }

        protected void updateTicket()
        {
            this.ticketRegistry.updateTicket(this.ticket);
        }

        protected T getTicket()
        {
            return this.ticket;
        }

        public string getId()
        {
            return this.ticket.getId();
        }

        public bool isExpired()
        {
            if (!callback)
            {
                return this.ticket.isExpired();
            }

            TicketGrantingTicket t = getGrantingTicket();

            return this.ticket.isExpired() || (t != null && t.isExpired());
        }

        public TicketGrantingTicket getGrantingTicket()
        {
            TicketGrantingTicket old = this.ticket.getGrantingTicket();

            if (old == null || !callback)
            {
                return old;
            }

            return (TicketGrantingTicket)this.ticketRegistry.getTicket(old.getId(), typeof(Ticket));
        }

        public long getCreationTime()
        {
            return this.ticket.getCreationTime();
        }

        public int getCountOfUses()
        {
            return this.ticket.getCountOfUses();
        }

        //@Override
        public int hashCode()
        {
            return this.ticket.GetHashCode();
        }

        //@Override
        public bool equals(Object o)
        {
            return this.ticket.Equals(o);
        }
    }

    private class ServiceTicketDelegator : TicketDelegator<ServiceTicket>, ServiceTicket
    {

        private static long serialVersionUID = 8160636219307822967L;

        public ServiceTicketDelegator(AbstractDistributedTicketRegistry ticketRegistry, ServiceTicket serviceTicket, bool callback) :
            base(ticketRegistry, serviceTicket, callback)
        {
            ;
        }


        public Service getService()
        {
            return getTicket().getService();
        }

        public bool isFromNewLogin()
        {
            return getTicket().isFromNewLogin();
        }

        public bool isValidFor(Service service)
        {
            bool b = this.getTicket().isValidFor(service);
            updateTicket();
            return b;
        }

        public TicketGrantingTicket grantTicketGrantingTicket(string id, Authentication authentication, ExpirationPolicy expirationPolicy)
        {
            TicketGrantingTicket t = this.getTicket().grantTicketGrantingTicket(id, authentication, expirationPolicy);
            updateTicket();
            return t;
        }
    }

    private class TicketGrantingTicketDelegator : TicketDelegator<TicketGrantingTicket>, TicketGrantingTicket
    {

        private static long serialVersionUID = 3946038899057626741L;

        public TicketGrantingTicketDelegator(AbstractDistributedTicketRegistry ticketRegistry, TicketGrantingTicket ticketGrantingTicket, bool callback)
            : base(ticketRegistry, ticketGrantingTicket, callback)
        {
            ;
        }

        public Authentication getAuthentication()
        {
            return getTicket().getAuthentication();
        }

        public ServiceTicket grantServiceTicket(string id, Service service, ExpirationPolicy expirationPolicy, bool credentialsProvided)
        {
            ServiceTicket t = this.getTicket().grantServiceTicket(id, service, expirationPolicy, credentialsProvided);
            updateTicket();
            return t;
        }

        public void expire()
        {
            this.getTicket().expire();
            updateTicket();
        }

        public bool isRoot()
        {
            return getTicket().isRoot();
        }

        public List<Authentication> getChainedAuthentications()
        {
            return getTicket().getChainedAuthentications();
        }
    }
}
