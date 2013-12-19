// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��05��09�� 10:30
//  
//  �޸��ڣ�2013��05��13�� 18:20
//  �ļ�����FrameworkOnly/Dev.Comm.Web.Mvc/CnStringLengthAttribute.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dev.Comm.Utils;

namespace Dev.Comm.Web.Mvc.Validate
{
    /// <summary>
    /// </summary>
    public class CnStringLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string StringLengthAttributeValidationErrorIncludingMinimum = "�ֶ�{0}����Ӧ��{2}-{1}֮��";

        /// <summary>
        /// 
        /// </summary>
        public int MaximumLength { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int MinimumLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maximumLength"></param>
        public CnStringLengthAttribute(int maximumLength)
            : base(() => "�ֶ�{0}���Ȳ��ܳ���{1}")
        {
            this.MaximumLength = maximumLength;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override bool IsValid(object value)
        {
            this.EnsureLegalLengths();

            //if (value == null) return false;
            //if (value.GetType() != typeof(string)) throw new InvalidOperationException("can only be used on boolean properties.");

            //var len = Dev.Comm.StringUtil.GbStrLength(value as string);

            int num = (value == null) ? 0 : StringUtil.GbStrLength((string)value);
            return value == null || (num >= this.MinimumLength && num <= this.MaximumLength);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public override string FormatErrorMessage(string name)
        {
            this.EnsureLegalLengths();


            string format = (this.MinimumLength != 0)
                                ? StringLengthAttributeValidationErrorIncludingMinimum
                                : base.ErrorMessageString;
            return string.Format(format, new object[]
                                             {
                                                 name,
                                                 this.MaximumLength,
                                                 this.MinimumLength
                                             });
        }

        /// <summary>
        ///   When implemented in a class, returns client validation rules for that class.
        /// </summary>
        /// <returns> The client validation rules for this validator. </returns>
        /// <param name="metadata"> The model metadata. </param>
        /// <param name="context"> The controller context. </param>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(System.Web.Mvc.ModelMetadata metadata,
                                                                               ControllerContext context)
        {
            var errormessage = String.IsNullOrEmpty(this.ErrorMessage)
                                   ? this.FormatErrorMessage(metadata.DisplayName)
                                   : string.Format(this.ErrorMessage, new object[]
                                                                          {
                                                                              metadata.DisplayName,
                                                                              this.MaximumLength,
                                                                              this.MinimumLength
                                                                          });
            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = errormessage,
                               ValidationType = "cnlength"
                           };
            rule.ValidationParameters["min"] = this.MinimumLength;
            rule.ValidationParameters["max"] = this.MaximumLength;

            yield return rule;
        }

        private void EnsureLegalLengths()
        {
            if (this.MaximumLength < 0)
            {
                throw new InvalidOperationException("��Ч����󳤶�");
            }
            if (this.MaximumLength < this.MinimumLength)
            {
                throw new InvalidOperationException(string.Format("��󳤶�{0}С����С����{1}", new object[]
                                                                                          {
                                                                                              this.MaximumLength,
                                                                                              this.MinimumLength
                                                                                          }));
            }
        }
    }
}