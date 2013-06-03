using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Application.Dto
{
    /// <summary>
    /// 基础通讯模型 ，
    /// ErrorCode 成功为0
    /// </summary>
    [DataContract]
    public class BaseState
    {
        public BaseState() { }
        public BaseState(int ErrorCode, string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
            this.ErrorCode = ErrorCode;
        }
        public BaseState(int ErrorCode) : this(ErrorCode, "") { }
        public BaseState(string ErrorMessage) : this(0 - 1 - System.DateTime.Today.Hour, ErrorMessage) { }
        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
