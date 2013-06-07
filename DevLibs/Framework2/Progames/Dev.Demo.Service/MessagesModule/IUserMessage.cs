using Dev.Demo.ViewModels.MessagesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Application.MainBoundedContext.MessagesModule
{
    interface IUserMessage
    {
        bool SendMessage(string from,string to,MessageBody messagebody);
        List<MessageDetail> GetMessageByUser(string UserName);

    }
}
