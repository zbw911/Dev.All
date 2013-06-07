using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Application.MainBoundedContext.MessagesModule
{

    
    class UserMessage : IUserMessage
    {
        
        public bool SendMessage(string from, string to, ViewModels.MessagesDto.MessageBody messagebody)
        {

            throw new NotImplementedException();
        }

        public List<ViewModels.MessagesDto.MessageDetail> GetMessageByUser(string UserName)
        {
            throw new NotImplementedException();
        }
    }
}
