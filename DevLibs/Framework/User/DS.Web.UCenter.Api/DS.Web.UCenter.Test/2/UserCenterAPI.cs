using System;
using System.Text;
using Commons;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
//using UserCenter.DBUtility;
using System.Configuration;


namespace UserCenter.UserCenter
{
    public class UserCenterAPI
    {
        private static string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        #region ��׼�ӿں���

        /// <summary>
        /// ͷ���޸Ľӿ�
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public string uc_avatar(int UID)
        {
            string uc_input = uc_api_input("uid=" + UID.ToString());
            string uc_avatarflash = SqlHelper.UC_API + "/images/camera.swf?inajax=1&appid=" + SqlHelper.UC_APPID + "&input=" + uc_input + "&agent=" + uc_Authcode.MD5(UserAgent) + "&ucapi=" + urlencode(SqlHelper.UC_API);
            return "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9\" width=447 height=477 id=\"mycamera\"><param name=\"movie\" value=\"" + uc_avatarflash + "\"><param name=\"quality\" value=\"high\"><param name=\"menu\" value=\"false\"><embed src=\"" + uc_avatarflash + "\" quality=\"high\" menu=\"false\"  width=\"447\" height=\"477\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\" name=\"mycamera\" swLiveConnect=\"true\"></embed></object>";
        }
        /// <summary>
        /// �û�ע��ӿ�
        /// </summary>
        public int uc_user_register(string username, string password, string email)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", username);
            ht.Add("password", password);
            ht.Add("email", email);
            //Hashtable hb = XmlCompent.GetTable(call_user_func("user", "register", ht));
            return int.Parse(call_user_func("user", "register", ht));
        }
        public int uc_user_register(string username, string password, string email, string questionid, string answer)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", username);
            ht.Add("password", password);
            ht.Add("email", email);
            ht.Add("questionid", questionid);
            ht.Add("answer", answer);
            return int.Parse(call_user_func("user", "register", ht));
        }
        /// <summary>
        /// �û���½�ӿ�
        /// </summary>
        public string[] uc_user_login(string uname, string pword)
        {
            return uc_user_login(uname, pword, 0, 0, 0, "");
        }
        public string[] uc_user_login(string uname, string pword, int id)
        {
            return uc_user_login(uname, pword, id, 0, 0, "");
        }
        public string[] uc_user_login(string uname, string pword, int id, int checkques, int questionid, string answer)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", uname);
            ht.Add("password", pword);
            ht.Add("isuid", id);
            ht.Add("checkques", checkques);
            ht.Add("questionid", questionid);
            ht.Add("answer", answer);
            Hashtable hb = XmlCompent.GetTable(call_user_func("user", "login", ht));
            string[] s = new string[5];
            s[0] = hb["item_0"].ToString();//�����û� ID����ʾ�û���¼�ɹ�
            s[1] = hb["item_1"].ToString();//�û���
            s[2] = hb["item_2"].ToString();//����
            s[3] = hb["item_3"].ToString();//Email
            s[4] = hb["item_4"].ToString();//�û����Ƿ�����,���Ӧ�ó��������������ģ����ҵ�ǰ��¼�û��������û���������ô���ص������� [4] ��ֵ������ 1
            return s;
        }
        /// <summary>
        /// ��ȡ�û����ݽӿ�
        /// </summary>
        public string[] uc_get_user(string username)
        {
            return uc_get_user(username, 0);
        }
        public string[] uc_get_user(string username, int isuid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", username);
            ht.Add("isuid", isuid);
            string ss = call_user_func("user", "get_user", ht);
            string[] s = new string[3];
            if (ss.Length < 20)
            {
                s[0] = ss;
                s[1] = "";
                s[2] = "";
            }
            else
            {
                Hashtable hb = XmlCompent.GetTable(ss);
                s[0] = hb["item_0"].ToString();//�û� ID
                s[1] = hb["item_1"].ToString();//�û���
                s[2] = hb["item_2"].ToString();//Email
            }
            return s;
        }
        /// <summary>
        /// �����û�����
        /// </summary>
        public decimal uc_user_edit(string username, string oldpw, string newpw, string email)
        {
            return uc_user_edit(username, oldpw, newpw, email, 0, 0, "");//�����ԣ�����������Ҫ��֤����
        }
        public decimal uc_user_edit(string username, string oldpw, string newpw, string email, int ignoreoldpw)
        {
            return uc_user_edit(username, oldpw, newpw, email, ignoreoldpw, 0, "");
        }
        public decimal uc_user_edit(string username, string oldpw, string newpw, string email, int ignoreoldpw, int questionid, string answer)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", username);
            ht.Add("oldpw", oldpw);
            ht.Add("newpw", newpw);
            ht.Add("email", email);
            ht.Add("ignoreoldpw", ignoreoldpw);
            ht.Add("questionid", questionid);
            ht.Add("answer", answer);

            string temp = call_user_func("user", "edit", ht);
            //ygj 2011-6-14 �Զ���һ������
            if (string.IsNullOrEmpty(temp))
            {
                temp = "-9";
            }

            //return decimal.Parse(temp);
            decimal _temp = 0;
            decimal.TryParse(temp, out _temp);
            return _temp;

        }
        /// <summary>
        /// ����û�����
        /// </summary>
        public int uc_user_checkname(string username)
        {
            Hashtable ht = new Hashtable();
            ht.Add("username", username);
            return int.Parse(call_user_func("user", "check_username", ht)); //Utils.StrToInt(call_user_func("user", "check_username", ht), -10);//-10��ʾϵͳ��æ
        }
        public int uc_user_checkemail(string email)
        {
            Hashtable ht = new Hashtable();
            ht.Add("email", email);
            return int.Parse(call_user_func("user", "check_email", ht));//-10��ʾϵͳ��æ
        }
        /// <summary>
        /// ɾ���û���Ϣ�ӿ�
        /// </summary>
        public int uc_user_delete(int uid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            Hashtable hb = XmlCompent.GetTable(call_user_func("user", "delete", ht));
            return int.Parse(hb["item_0"].ToString());
        }
        /// <summary>
        /// ͬ����½
        /// </summary>
        public string uc_user_synlogin(int uid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            return call_user_func("user", "synlogin", ht);//js�ű�����  - ͬ����¼�� HTML ����
        }
        /// <summary>
        /// ͬ���˳�����
        /// </summary>
        public string uc_user_synlogout()
        {
            return call_user_func("user", "synlogout", new Hashtable());//js�ű�����  - ͬ����¼�� HTML ����
        }




        /// <summary>
        /// �������Ϣ����
        /// </summary>
        /// <param name="uid">�û� ID</param>
        public void uc_pm_location(int uid)
        {
            uc_pm_location(uid, 0);
        }
        public void uc_pm_location(int uid, int newpm)
        {
            string apiurl = uc_api_url("pm_client", "ls", "uid=" + uid.ToString(), newpm == 1 ? "&folder=newbox" : "");
            //HttpContext.Current.Response.AddHeader("Expires", "0");
            //HttpContext.Current.Response.AddHeader("Cache-Control", "private, post-check=0, pre-check=0, max-age=0");
            //HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //HttpContext.Current.Response.Redirect(apiurl, true);
        }

        /// <summary>
        /// ����µĶ���Ϣ
        /// </summary>
        public string uc_pm_checknew(int uid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("more", "0");
            return call_user_func("pm", "check_newpm", ht);
        }
        public Hashtable uc_pm_checknew(int uid, int more)
        {
            if (more == 0) more = 1;//����Ϊ1
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("more", more);
            return XmlCompent.GetTable(call_user_func("pm", "check_newpm", ht));
        }
        /// <summary>
        /// ���Ͷ���Ϣ
        /// </summary>
        /// <param name="fromuid">�������û� ID��0 Ϊϵͳ��Ϣ</param>
        /// <param name="msgto">�ռ����û��� / �û� ID������ö��ŷָ�</param>
        /// <param name="subject">��Ϣ����</param>
        /// <param name="message">��Ϣ����</param>
        /// <param name="instantly">�Ƿ�ֱ�ӷ���</param>
        /// <param name="replypmid">�ظ�����Ϣ ID</param>
        /// <param name="isusername">0 = msgto Ϊ uid��1 = msgto Ϊ username</param>
        /// <returns></returns>
        public int uc_pm_send(int fromuid, string msgto, string subject, string message)
        {
            return uc_pm_send(fromuid, msgto, subject, message, 0, 1, true);
        }
        public int uc_pm_send(int fromuid, string msgto, string subject, string message, int replypmid)
        {
            return uc_pm_send(fromuid, msgto, subject, message, replypmid, 1, true);
        }
        public int uc_pm_send(int fromuid, string msgto, string subject, string message, int replypmid, int isusername)
        {
            return uc_pm_send(fromuid, msgto, subject, message, replypmid, isusername, true);
        }
        public int uc_pm_send(int fromuid, string msgto, string subject, string message, int replypmid, int isusername, bool instantly)
        {
            if (!instantly)
            {
                uc_pm_send_instantly(fromuid, msgto, subject, message, replypmid, isusername);
            }
            Hashtable ht = new Hashtable();
            ht.Add("fromuid", fromuid);
            ht.Add("msgto", msgto);
            ht.Add("subject", subject);
            ht.Add("message", message);
            ht.Add("replypmid", replypmid < 0 ? 0 : replypmid);
            ht.Add("isusername", isusername);
            return int.Parse(call_user_func("pm", "sendpm", ht));//Ĭ��Ϊ����ʧ��
        }
        /// <summary>
        /// ���뷢�Ͷ���Ϣ�Ľ���
        /// </summary>
        public void uc_pm_send_instantly(int fromuid, string msgto, string subject, string message, int replypmid, int isusername)
        {
            subject = urlencode(subject);
            msgto = urlencode(msgto);
            message = urlencode(message);
            string replyadd = replypmid > 0 ? "&pmid=" + replypmid.ToString() + "&do=reply" : "";
            string apiurl = uc_api_url("pm_client", "send", "uid=" + fromuid.ToString(), "&msgto=" + msgto + "&subject=" + subject + "&message=" + message + replyadd);
            //HttpContext.Current.Response.AddHeader("Expires", "0");
            //HttpContext.Current.Response.AddHeader("Cache-Control", "private, post-check=0, pre-check=0, max-age=0");
            //HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //HttpContext.Current.Response.Redirect(apiurl, true);
        }

        /// <summary>
        /// ɾ������Ϣ
        /// </summary>
        /// <param name="uid">�û� ID</param>
        /// <param name="folder">����Ϣ���ڵ��ļ���[inbox=�ռ��䣬outbox=������]</param>
        /// <param name="pmids">Ҫɾ������ϢID����</param>
        /// <returns>>0 �ɹ�  xiaoyu dengyu0 ʧ��</returns>
        public int uc_pm_delete(int uid, string folder, Hashtable pmids)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("folder", folder);
            ht.Add("pmids", pmids);
            return int.Parse(call_user_func("pm", "delete", ht));//Ĭ��Ϊɾ��ʧ��
        }

        /// <summary>
        /// ɾ���� uid �Ի��� touids �е����ж���Ϣ��
        /// </summary>
        public int uc_pm_deleteuser(int uid, Hashtable touids)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("touids", touids);
            return int.Parse(call_user_func("pm", "deleteuser", ht));//Ĭ��Ϊɾ��ʧ��
        }

        /// <summary>
        /// ��Ƕ���Ϣ�Ѷ�/δ��״̬
        /// </summary>
        public void uc_pm_readstatus(int uid, Hashtable uids)
        {
            uc_pm_readstatus(uid, uids, new Hashtable(), 0);
        }
        public void uc_pm_readstatus(int uid, Hashtable uids, Hashtable pmids)
        {
            uc_pm_readstatus(uid, uids, pmids, 0);
        }
        public void uc_pm_readstatus(int uid, Hashtable uids, Hashtable pmids, int status)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("uids", uids);
            ht.Add("pmids", pmids);
            ht.Add("status", status);
            call_user_func("pm", "readstatus", ht);
        }

        /// <summary>
        /// ��ȡ����Ϣ�б�
        /// </summary>
        /// <param name="uid">�û� ID</param>
        /// <param name="page">��ǰҳ��ţ�Ĭ��ֵ 1</param>
        /// <param name="pagesize">ÿҳ�����Ŀ����Ĭ��ֵ 10</param>
        /// <param name="folder">�򿪵�Ŀ¼ newbox=δ����Ϣ��inbox=�ռ��䣬outbox=������</param>
        /// <param name="filter">���˷�ʽ newpm=δ����Ϣ��systempm=ϵͳ��Ϣ��announcepm=������Ϣ</param>
        /// <param name="msglen">��ȡ����Ϣ���ֳ���</param>
        /// <returns>array('count' => ��Ϣ����, 'data' => ����Ϣ����)</returns>

        public Hashtable uc_pm_list(int uid, int page, int pagesize, string folder, string filter, int msglen)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("page", page);
            ht.Add("pagesize", pagesize);
            ht.Add("folder", folder);
            ht.Add("filter", filter);
            ht.Add("msglen", msglen);
            return XmlCompent.GetTable(call_user_func("pm", "ls", ht));
        }
        /// <summary>
        /// ����δ����Ϣ��ʾ
        /// </summary>
        public void uc_pm_ignore(int uid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            call_user_func("pm", "ignore", ht);
        }
        /// <summary>
        /// ��ȡ����Ϣ����
        /// </summary>
        /// <param name="uid">�û� ID</param>
        /// <param name="pmid">��Ϣ ID</param>
        /// <param name="touid">��Ϣ�Է��û� ID</param>
        /// <param name="daterange">���ڷ�Χ 1=����,2=����,3=ǰ��,4=����,5=����</param>
        /// <returns>����Ϣ��������</returns>
        public Hashtable uc_pm_view(int uid, int pmid)
        {
            return uc_pm_view(uid, pmid, 0, 1);
        }
        public Hashtable uc_pm_view(int uid, int pmid, int touid)
        {
            return uc_pm_view(uid, pmid, touid, 1);
        }
        public Hashtable uc_pm_view(int uid, int pmid, int touid, int daterange)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("pmid", pmid);
            ht.Add("touid", touid);
            ht.Add("daterange", daterange);
            return XmlCompent.GetTable(call_user_func("pm", "view", ht));
        }
        /// <summary>
        /// ��ȡ��������Ϣ����
        /// </summary>
        public Hashtable uc_pm_viewnode(int uid)
        {
            return uc_pm_viewnode(uid, 0, 0);
        }
        public Hashtable uc_pm_viewnode(int uid, int type, int pmid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("type", type);
            ht.Add("pmid", pmid);
            return XmlCompent.GetTable(call_user_func("pm", "viewnode", ht));
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        public string uc_pm_blackls_get(int uid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            return call_user_func("pm", "blackls_get", ht);
        }
        /// <summary>
        /// ���º�����
        /// </summary>
        public bool uc_pm_blackls_set(int uid, string blackls)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("blackls", blackls);
            string t = call_user_func("pm", "blackls_set", ht);
            if (t == "1")
                return true;
            else
                return false;
        }
        /// <summary>
        /// ��Ӻ�������Ŀ
        /// </summary>
        public bool uc_pm_blackls_add(int uid, Hashtable username)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("username", username);
            string t = call_user_func("pm", "blackls_add", ht);
            if (t == "1")
                return true;
            else
                return false;
        }
        /// <summary>
        /// ɾ����������Ŀ
        /// </summary>
        public void uc_pm_blackls_delete(int uid, Hashtable username)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("username", username);
            call_user_func("pm", "blackls_delete", ht);
        }
        #region ���ֲ���

        /// <summary>
        /// ����û�����
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="uid"></param>
        /// <param name="credit"></param>
        /// <returns></returns>
        public int uc_user_getcredit(int appid, int uid, int credit)
        {
            Hashtable ht = new Hashtable();
            ht.Add("appid", appid);
            ht.Add("uid", uid);
            ht.Add("credit", credit);
            //string s = call_user_func("user", "getcredit", ht);
            return int.Parse(call_user_func("user", "getcredit", ht));//Ĭ��Ϊɾ��ʧ��
        }
        /// <summary>
        /// ���ָ��Ӧ�õĻ���
        /// </summary>
        public int uc_credit_updatecredit(int uid, int to, int toappid, int amount)
        {
            Hashtable ht = new Hashtable();
            ht.Add("uid", uid);
            ht.Add("to", to);
            ht.Add("toappid", toappid);
            ht.Add("amount", amount);
            //string s = call_user_func("credit", "update", ht);
            return int.Parse(call_user_func("credit", "update", ht));//Ĭ��Ϊɾ��ʧ��
        }

        #endregion

        #endregion

        public string geturl(string url, string postdata)
        {
            return uc_fopen2(url, 500000, postdata, string.Empty, true, "", 20);
        }

        #region API�ӿڲ���
        public string uc_api_url(string module, string action)
        {
            return uc_api_url(module, action, "", "");
        }
        public string uc_api_url(string module, string action, string arg, string extra)
        {
            return SqlHelper.UC_API + "/index.php?" + uc_api_requestdata(module, action, arg, extra);
        }


        public string call_user_func(string module, string action, Hashtable hb)
        {
            string s = "";
            string sep = "";
            foreach (DictionaryEntry de in hb)
            {
                if (de.Value.GetType().ToString() == "System.Collections.Hashtable")
                {
                    string s2 = "";
                    string sep2 = "";
                    Hashtable ht = (Hashtable)de.Value;
                    foreach (DictionaryEntry de1 in ht)
                    {
                        s2 = sep2 + de.Key.ToString() + "[" + de1.Key.ToString() + "]=" + urlencode(de1.Value.ToString()); // System.Text.Encoding.GetEncoding(de1.Value.ToString());
                        sep2 = "&";
                    }
                    s += sep2 + s2;
                }
                else
                {


                    s += sep + de.Key.ToString() + "=" + Dev.Comm.Utils.MockUrlCode.UrlEncode(de.Value.ToString());
                    //s += sep + de.Key.ToString() + "=" + urlencode(de.Value.ToString());
                }
                sep = "&";
            }
            string postdata = uc_api_requestdata(module, action, s, "");
            // XZ.Common.WebClient client = new XZ.Common.WebClient();
            //client.Encoding = System.Text.Encoding.Default;
            // client.OpenRead(SqlHelper.UC_API + "/index.php?", postdata);
            return uc_fopen2(SqlHelper.UC_API + "/index.php", 500000, postdata, string.Empty, true, SqlHelper.UC_IP, 20);
            //return client.RespHtml;
        }
        public string uc_api_requestdata(string module, string action, string arg, string extra)
        {
            string input = uc_api_input(arg);
            string post = "m=" + module + "&a=" + action + "&inajax=2&input=" + input + "&appid=" + SqlHelper.UC_APPID + extra;
            return post;
        }
        public string uc_api_input(string data)
        {
            string s = urlencode(uc_Authcode.DiscuzAuthcodeEncode(data + "&agent=" + uc_Authcode.MD5(UserAgent) + "&time=" + gettimestamp(), SqlHelper.UC_KEY));
            return s;
        }
        public string gettimestamp()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //�õ�1970���ʱ���
            long s = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
            return s.ToString();

            //ע��������ʱ�����⣬��now��Ҫ����8��Сʱ
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            //TimeSpan toNow = dtNow.Subtract(dtStart);
            //string timeStamp = toNow.Ticks.ToString();
            //return timeStamp.Substring(0, timeStamp.Length - 7);
        }
        /// <summary>
        /// php��urlencode����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string urlencode(string str)
        {
            //�˴������⣬��Ϊphp��urlencode��ͬ��HttpUtility.UrlEncode
            //return HttpUtility.UrlEncode(str);
            string tmp = string.Empty;
            string strSpecial = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string crt = str.Substring(i, 1);
                if (strSpecial.Contains(crt))
                    tmp += crt;
                else
                {
                    byte[] bts = System.Text.Encoding.Default.GetBytes(crt);
                    foreach (byte bt in bts)
                    {
                        tmp += "%" + bt.ToString("X");
                    }
                }
            }
            return tmp;
        }
        #endregion
        #region uc_fopen
        private static string uc_fopen(string url)
        {
            return uc_fopen(url, 0, string.Empty, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen(string url, int limit)
        {
            return uc_fopen(url, limit, string.Empty, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen(string url, int limit, string post)
        {
            return uc_fopen(url, limit, post, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen(string url, int limit, string post, string cookie)
        {
            return uc_fopen(url, limit, post, cookie, false, string.Empty, 15, true);
        }

        private static string uc_fopen(string url, int limit, string post, string cookie, bool bysocket)
        {
            return uc_fopen(url, limit, post, cookie, bysocket, string.Empty, 15, true);
        }

        private static string uc_fopen(string url, int limit, string post, string cookie, bool bysocket, string ip)
        {
            return uc_fopen(url, limit, post, cookie, bysocket, ip, 15, true);
        }

        private static string uc_fopen(string url, int limit, string post, string cookie, bool bysocket, string ip, int timeout)
        {
            return uc_fopen(url, limit, post, cookie, bysocket, ip, timeout, true);
        }

        private static string uc_fopen(string url, int limit, string post, string cookie, bool bysocket, string ip, int timeout, bool block)
        {
            //ʱ������ ����ʱ��Ϊ
            //DateTime ndt = Convert.ToDateTime("2009-2-10");
            //TimeSpan tss = ndt - DateTime.Now;
            //if (tss.TotalDays < 0)
            //{
            //    HttpContext.Current.Response.Write("ʹ�ù�����");
            //    HttpContext.Current.Response.End();
            //    return "";
            //}
            bool bRetVal = false;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            //myRequest.AllowAutoRedirect = true;
            //if (string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent))
            //    myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //else                

            //myRequest.UserAgent = UserAgent;
            //myRequest.UserAgent = HttpContext.Current.Request.UserAgent;
            //myRequest.KeepAlive = true;
            //if (HttpContext.Current.Request.Url != null)
            //    myRequest.Referer = HttpContext.Current.Request.Url.ToString();

            //myRequest.CookieContainer = ;

            if (string.IsNullOrEmpty(post))
            {
                myRequest.Method = "GET";
            }
            else
            {
                //myRequest.Method = "POST";
                //Stream myStream = new MemoryStream();//�������Stream��ֻ��Ϊ�˵õ������ִ� ��������֮��õ���byte�ĳ��ȡ�                    
                //StreamWriter myStreamWriter = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(SqlHelper.UC_CHARSET));//��Ĭ�ϱ��� �õ�Stream
                //myStreamWriter.Write(post);
                //myStreamWriter.Flush();
                //long len = myStream.Length;//Ŀ�����
                //myStreamWriter.Close();
                //myStream.Close();

                //myRequest.ContentType = "application/x-www-form-urlencoded";
                //myRequest.ContentLength = len;//����ַ����д������� ʹ��loginWebView.postContent.Length�õ����Ⱥͱ���֮��ĳ����ǲ�һ����:(

                //Stream newStream = myRequest.GetRequestStream();
                //myStreamWriter = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(SqlHelper.UC_CHARSET));//����ʹ��Encoding.Default ������ȥ���뷽�� ���������õ������Ĳ����������

                //myStreamWriter.Write(post);
                //myStreamWriter.Close();
                //myStream.Close();

                Encoding encoding = Encoding.GetEncoding(SqlHelper.UC_CHARSET);
                byte[] data = encoding.GetBytes(post);

                myRequest.UserAgent = UserAgent;
                myRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;

                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

            }

            //�������ջ������ֽ�����
            string responseHtml = string.Empty;
            //myRequest.Timeout = timeout;
            HttpWebResponse webResponse = null;
            try
            {
                webResponse = (HttpWebResponse)myRequest.GetResponse();
                bRetVal = true;
                if (webResponse.StatusCode != HttpStatusCode.OK)
                    bRetVal = false;

                if (bRetVal)
                {
                    //�������ջ������ֽ�����
                    Stream receiveStream = webResponse.GetResponseStream();//�õ���д���ֽ���
                    StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding(SqlHelper.UC_CHARSET));
                    responseHtml = readStream.ReadToEnd();
                    readStream.Close();
                }
                if (webResponse != null)
                    webResponse.Close();
            }
            catch (Exception exp)
            {
                throw;
            }

            return responseHtml;
        }
        #endregion

        #region uc_fopen2
        private static string uc_fopen2(string url)
        {
            return uc_fopen2(url, 0, string.Empty, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen2(string url, int limit)
        {
            return uc_fopen2(url, limit, string.Empty, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen2(string url, int limit, string post)
        {
            return uc_fopen2(url, limit, post, string.Empty, false, string.Empty, 15, true);
        }

        private static string uc_fopen2(string url, int limit, string post, string cookie)
        {
            return uc_fopen2(url, limit, post, cookie, false, string.Empty, 15, true);
        }

        private static string uc_fopen2(string url, int limit, string post, string cookie, bool bysocket)
        {
            return uc_fopen2(url, limit, post, cookie, bysocket, string.Empty, 15, true);
        }

        private static string uc_fopen2(string url, int limit, string post, string cookie, bool bysocket, string ip)
        {
            return uc_fopen2(url, limit, post, cookie, bysocket, ip, 15, true);
        }

        private static string uc_fopen2(string url, int limit, string post, string cookie, bool bysocket, string ip, int timeout)
        {
            return uc_fopen2(url, limit, post, cookie, bysocket, ip, timeout, true);
        }

        private static string uc_fopen2(string url, int limit, string post, string cookie, bool bysocket, string ip, int timeout, bool block)
        {
            int times = 1;
            //if (HttpContext.Current.Request["__times__"] != null)
            //{
            //    times = int.Parse(HttpContext.Current.Request["__times__"].ToString()) + 1;
            //}
            //if (times > 2)
            //    return string.Empty;

            //url += url.Contains("?") ? "&" : "?" + "__times__=" + times;

            return uc_fopen(url, limit, post, cookie, bysocket, ip, timeout, block);
        }
        #endregion
        #region ��UNIXʱ���ת����ϵͳʱ��

        public DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion
    }

    /// <summary>
    /// SqlHelper����ר���ṩ������û����ڸ����ܡ��������������ϰ��sql���ݲ���
    /// </summary>
    public abstract class SqlHelper
    {

        //        define('UC_CHARSET', 'utf-8');
        //define('UC_KEY', 'l9e5f387Y5IcW9I6U3q6Abibfccag4Qc86pfQ53ai4L9KdJfFfIci7I6Oeyb472c');
        //define('UC_API', 'http://localhost:34382/ucServer');
        //define('UC_APPID', '1');
        //���ݿ������ַ���
        //public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString;
        //�ؼ��ӿ��ַ���
        public static readonly string UC_KEY = "l9e5f387Y5IcW9I6U3q6Abibfccag4Qc86pfQ53ai4L9KdJfFfIci7I6Oeyb472c";//Dx.Common.EncryptFunc.JieMi(ConfigurationManager.AppSettings["UC_KEY"].ToString());
        public static readonly string UC_APPID = "1";// ConfigurationManager.AppSettings["UC_APPID"];
        public static readonly string UC_IP = "";// ConfigurationManager.AppSettings["UC_IP"];
        public static readonly string UC_API = "http://localhost:34382/ucServer";// ConfigurationManager.AppSettings["UC_API"];
        public static readonly string UC_CHARSET = "utf-8";// ConfigurationManager.AppSettings["UC_CHARSET"];
        //public static readonly string UserCookie1 = ConfigurationManager.AppSettings["CookieField1"];
        //public static readonly string UserCookie2 = ConfigurationManager.AppSettings["CookieField2"];
        //public static readonly string CodeFlag = ConfigurationManager.AppSettings["CodeFlag"];
        //public static readonly string ManageCode = ConfigurationManager.AppSettings["ManageCode"];
        //public static readonly string ServerNum = ConfigurationManager.AppSettings["ServerNum"];
        //public static readonly string GameType = ConfigurationManager.AppSettings["GameType"];
        //public static readonly string AreaNo = ConfigurationManager.AppSettings["AreaNo"];
        //public static readonly string GameURL = ConfigurationManager.AppSettings["GameURL"];
        //public static readonly string PayURL = ConfigurationManager.AppSettings["PayURL"];
    }
}
