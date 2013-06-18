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
//package org.jasig.cas.services;

//import java.util.*;
//import java.util.concurrent.ConcurrentHashMap;

//import com.github.inspektr.audit.annotation.Audit;
//import org.jasig.cas.authentication.principal.Service;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.transaction.annotation.Transactional;

//import javax.validation.constraints.NotNull;

/**
 * Default implementation of the {@link ServicesManager} interface. If there are
 * no services registered with the server, it considers the ServicecsManager
 * disabled and will not prevent any service from using CAS.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */
public  class DefaultServicesManagerImpl : ReloadableServicesManager {

    private  Logger log = LoggerFactory.getLogger(getClass());

    /** Instance of ServiceRegistryDao. */
    @NotNull
    private ServiceRegistryDao serviceRegistryDao;

    /** Map to store all services. */
    private ConcurrentHashMap<Long, RegisteredService> services = new ConcurrentHashMap<Long, RegisteredService>();

    /** Default service to return if none have been registered. */
    private RegisteredService disabledRegisteredService;
    
    public DefaultServicesManagerImpl(
         ServiceRegistryDao serviceRegistryDao) {
        this(serviceRegistryDao, new ArrayList<string>());
    }
    
    /**
     * Constructs an instance of the {@link DefaultServicesManagerImpl} where the default RegisteredService
     * can include a set of default attributes to use if no services are defined in the registry.
     * 
     * @param serviceRegistryDao the Service Registry Dao.
     * @param defaultAttributes the list of default attributes to use.
     */
    public DefaultServicesManagerImpl( ServiceRegistryDao serviceRegistryDao,  List<string> defaultAttributes) {
        this.serviceRegistryDao = serviceRegistryDao;
        this.disabledRegisteredService = constructDefaultRegisteredService(defaultAttributes);
        
        load();
    }

    @Transactional(readOnly = false)
    @Audit(action = "DELETE_SERVICE", actionResolverName = "DELETE_SERVICE_ACTION_RESOLVER", resourceResolverName = "DELETE_SERVICE_RESOURCE_RESOLVER")
    public synchronized RegisteredService delete( long id) {
         RegisteredService r = findServiceBy(id);
        if (r == null) {
            return null;
        }
        
        this.serviceRegistryDao.delete(r);
        this.services.remove(id);
        
        return r;
    }

    /**
     * Note, if the repository is empty, this implementation will return a default service to grant all access.
     * <p>
     * This preserves default CAS behavior.
     */
    public RegisteredService findServiceBy( Service service) {
         Collection<RegisteredService> c = convertToTreeSet();
        
        if (c.isEmpty()) {
            return this.disabledRegisteredService;
        }

        for ( RegisteredService r : c) {
            if (r.matches(service)) {
                return r;
            }
        }

        return null;
    }

    public RegisteredService findServiceBy( long id) {
         RegisteredService r = this.services.get(id);
        
        try {
            return r == null ? null : (RegisteredService) r.clone();
        } catch ( CloneNotSupportedException e) {
            return r;
        }
    }
    
    protected TreeSet<RegisteredService> convertToTreeSet() {
        return new TreeSet<RegisteredService>(this.services.values());
    }

    public Collection<RegisteredService> getAllServices() {
        return Collections.unmodifiableCollection(convertToTreeSet());
    }

    public bool matchesExistingService( Service service) {
        return findServiceBy(service) != null;
    }

    @Transactional(readOnly = false)
    @Audit(action = "SAVE_SERVICE", actionResolverName = "SAVE_SERVICE_ACTION_RESOLVER", resourceResolverName = "SAVE_SERVICE_RESOURCE_RESOLVER")
    public synchronized RegisteredService save( RegisteredService registeredService) {
         RegisteredService r = this.serviceRegistryDao.save(registeredService);
        this.services.put(r.getId(), r);
        return r;
    }
    
    public void reload() {
        log.info("Reloading registered services.");
        load();
    }
    
    private void load() {
         ConcurrentHashMap<Long, RegisteredService> localServices = new ConcurrentHashMap<Long, RegisteredService>();
                
        for ( RegisteredService r : this.serviceRegistryDao.load()) {
            log.debug("Adding registered service " + r.getServiceId());
            localServices.put(r.getId(), r);
        }
        
        this.services = localServices;
        log.info(string.format("Loaded %s services.", this.services.size()));
    }
    
    private RegisteredService constructDefaultRegisteredService( List<string> attributes) {
         RegisteredServiceImpl r = new RegisteredServiceImpl();
        r.setAllowedToProxy(true);
        r.setAnonymousAccess(false);
        r.setEnabled(true);
        r.setSsoEnabled(true);
        r.setAllowedAttributes(attributes);
        
        if (attributes == null || attributes.isEmpty()) {
            r.setIgnoreAttributes(true);
        }

        return r;
    }
}
