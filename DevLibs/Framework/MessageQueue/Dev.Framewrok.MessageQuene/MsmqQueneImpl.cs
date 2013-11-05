using System;
using System.Collections.Generic;
using System.Messaging;

namespace Dev.Framewrok.MessageQuene
{
    /// <summary>
    /// ΢����Ϣ���е�Ĭ��ʵ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsmqQueneImpl<T> : IMsgQuene<T>
    {
        private readonly string _queuePath;
        private MessageQueue _myQueue;

        public MsmqQueneImpl(string queuePath, bool isLocalMachine)
        {
            _queuePath = queuePath;

            if (isLocalMachine)
                CreateIfNotExist();

            this._myQueue = new MessageQueue(_queuePath);
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public void Createqueue()
        {
            this._myQueue = MessageQueue.Create(_queuePath);

            //throw new NotImplementedException();
        }

        public void CreateIfNotExist()
        {
            if (!Exists())
                Createqueue();
        }

        /// <summary>
        /// �鿴ָ����Ϣ�����Ƿ���� 
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return MessageQueue.Exists(_queuePath);
        }

        /// <summary>
        /// ɾ�����е���Ϣ����
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            MessageQueue.Delete(_queuePath);
        }

        /// <summary>
        /// �õ������е�������Ϣ
        /// </summary>
        /// <returns></returns>
        public List<T> GetAllMessages()
        {


            Message[] message = this._myQueue.GetAllMessages();
            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(T) });

            List<T> list = new List<T>();


            foreach (Message msg in message)
            {
                msg.Formatter = formatter;

                //_myQueue.ReceiveById(msg.Id);
                list.Add((T)msg.Body);
            }



            return list;
        }

        /// <summary>
        /// �鿴ĳ���ض������е���Ϣ���У������Ӹö������Ƴ���Ϣ��
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            this._myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
            //try
            //{
            //�Ӷ����н�����Ϣ
            Message myMessage = this._myQueue.Peek();
            T context = (T)myMessage.Body; //��ȡ��Ϣ������
            return context;
            //}
            //catch (MessageQueueException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //catch (InvalidCastException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        /// <summary>
        /// ����ID Peek ��������Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T PeekById(string id)
        {
            var myMessage = this._myQueue.PeekById(id);
            T context = (T)myMessage.Body; //��ȡ��Ϣ������
            return context;
        }

        /// <summary>
        /// ����ָ����Ϣ��������ǰ�����Ϣ������Ӹö������Ƴ���
        /// </summary>
        /// <returns></returns>
        public T Receive()
        {
            this._myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
            //try
            //{
            //�Ӷ����н�����Ϣ
            Message myMessage = this._myQueue.Receive();
            T context = (T)myMessage.Body; //��ȡ��Ϣ������
            return context;
        }

        /// <summary>
        /// ����ID ����ָ����Ϣ��������ǰ�����Ϣ������Ӹö������Ƴ���
        /// </summary>
        /// <returns></returns>
        public T ReceiveById(string id)
        {
            Message myMessage = this._myQueue.ReceiveById(id);
            T context = (T)myMessage.Body; //��ȡ��Ϣ������
            return context;
        }

        /// <summary>
        /// ������Ϣ��ָ������Ϣ����
        /// </summary>
        /// <param name="msg"></param>
        public void Send(T msg)
        {
            //try
            //{
            //���ӵ����صĶ���
            Message myMessage = new Message();
            myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
            myMessage.Body = msg;

            //������Ϣ��������
            this._myQueue.Send(myMessage);
            //}
            //catch (ArgumentException e)
            //{
            //    Console.WriteLine(e.Message);
            //}

        }

        /// <summary>
        /// ���ָ�����е���Ϣ
        /// </summary>
        public void Clear()
        {
            this._myQueue.Purge();
        }
    }
}