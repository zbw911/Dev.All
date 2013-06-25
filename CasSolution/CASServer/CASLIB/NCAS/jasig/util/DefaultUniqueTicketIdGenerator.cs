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
////package org.jasig.cas.util;

/**
 * Default implementation of {@link UniqueTicketIdGenerator}. Implementation
 * utilizes a DefaultLongNumericGeneraor and a DefaultRandomStringGenerator to
 * construct the ticket id.
 * <p>
 * Tickets are of the form [PREFIX]-[SEQUENCE NUMBER]-[RANDOM STRING]-[SUFFIX]
 * </p>
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.0
 */

using System.Text;

namespace NCAS.jasig.util
{
    public sealed class DefaultUniqueTicketIdGenerator :
        UniqueTicketIdGenerator
    {

        /** The numeric generator to generate the static part of the id. */
        private readonly NumericGenerator numericGenerator;

        /** The RandomStringGenerator to generate the secure random part of the id. */
        private readonly RandomStringGenerator randomStringGenerator;

        /**
     * Optional suffix to ensure uniqueness across JVMs by specifying unique
     * values.
     */
        private readonly string suffix;

        /**
     * Creates an instance of DefaultUniqueTicketIdGenerator with default values
     * including a {@link DefaultLongNumericGenerator} with a starting value of
     * 1.
     */
        public DefaultUniqueTicketIdGenerator()
            : this(null)
        {

        }

        /**
     * Creates an instance of DefaultUniqueTicketIdGenerator with a specified
     * maximum length for the random portion.
     * 
     * @param maxLength the maximum length of the random string used to generate
     * the id.
     */
        public DefaultUniqueTicketIdGenerator(int maxLength)
            : this(maxLength, null)
        {

        }

        /**
     * Creates an instance of DefaultUniqueTicketIdGenerator with default values
     * including a {@link DefaultLongNumericGenerator} with a starting value of
     * 1.
     * 
     * @param suffix the value to append at the end of the unique id to ensure
     * uniqueness across JVMs.
     */
        public DefaultUniqueTicketIdGenerator(string suffix)
        {
            this.numericGenerator = new DefaultLongNumericGenerator(1);
            this.randomStringGenerator = new DefaultRandomStringGenerator();

            if (suffix != null)
            {
                this.suffix = "-" + suffix;
            }
            else
            {
                this.suffix = null;
            }
        }

        /**
     * Creates an instance of DefaultUniqueTicketIdGenerator with a specified
     * maximum length for the random portion.
     * 
     * @param maxLength the maximum length of the random string used to generate
     * the id.
     * @param suffix the value to append at the end of the unique id to ensure
     * uniqueness across JVMs.
     */
        public DefaultUniqueTicketIdGenerator(int maxLength, string suffix)
        {
            this.numericGenerator = new DefaultLongNumericGenerator(1);
            this.randomStringGenerator = new DefaultRandomStringGenerator(maxLength);

            if (suffix != null)
            {
                this.suffix = "-" + suffix;
            }
            else
            {
                this.suffix = null;
            }
        }

        public string getNewTicketId(string prefix)
        {
            string number = this.numericGenerator.getNextNumberAsString();
            StringBuilder buffer = new StringBuilder(prefix.Length + 2
                                                     + (this.suffix != null ? this.suffix.Length : 0) + this.randomStringGenerator.getMaxLength()
                                                     + number.Length);

            buffer.Append(prefix);
            buffer.Append("-");
            buffer.Append(number);
            buffer.Append("-");
            buffer.Append(this.randomStringGenerator.getNewString());

            if (this.suffix != null)
            {
                buffer.Append(this.suffix);
            }

            return buffer.ToString();
        }
    }
}
