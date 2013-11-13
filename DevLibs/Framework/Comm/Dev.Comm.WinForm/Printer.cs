// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��11��13�� 16:47
//  
//  �޸��ڣ�2013��11��13�� 16:49
//  �ļ�����Dev.Libs/Dev.Comm.WinForm/Printer.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Dev.Comm.WinForm
{
    /// <summary>
    ///   from:http://www.cnblogs.com/waxdoll/archive/2008/10/20/342910.html
    ///   ��ӡ��
    /// </summary>
    public class Printer
    {
        private Printer()
        {
        }

        #region API����

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structPrinterDefaults
        {
            [MarshalAs(UnmanagedType.LPTStr)] public String pDatatype;
            public IntPtr pDevMode;
            [MarshalAs(UnmanagedType.I4)] public int DesiredAccess;
        };

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinter", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPTStr)] string printerName,
                                                out IntPtr phPrinter,
                                                ref structPrinterDefaults pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity()]
        internal static extern bool ClosePrinter(IntPtr phPrinter);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structSize
        {
            public Int32 width;
            public Int32 height;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structRect
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        internal struct FormInfo1
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.I4)] public uint Flags;
            [FieldOffset(4), MarshalAs(UnmanagedType.LPWStr)] public String pName;
            [FieldOffset(8)] public structSize Size;
            [FieldOffset(16)] public structRect ImageableArea;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct structDevMode
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public String
                dmDeviceName;

            [MarshalAs(UnmanagedType.U2)] public short dmSpecVersion;
            [MarshalAs(UnmanagedType.U2)] public short dmDriverVersion;
            [MarshalAs(UnmanagedType.U2)] public short dmSize;
            [MarshalAs(UnmanagedType.U2)] public short dmDriverExtra;
            [MarshalAs(UnmanagedType.U4)] public int dmFields;
            [MarshalAs(UnmanagedType.I2)] public short dmOrientation;
            [MarshalAs(UnmanagedType.I2)] public short dmPaperSize;
            [MarshalAs(UnmanagedType.I2)] public short dmPaperLength;
            [MarshalAs(UnmanagedType.I2)] public short dmPaperWidth;
            [MarshalAs(UnmanagedType.I2)] public short dmScale;
            [MarshalAs(UnmanagedType.I2)] public short dmCopies;
            [MarshalAs(UnmanagedType.I2)] public short dmDefaultSource;
            [MarshalAs(UnmanagedType.I2)] public short dmPrintQuality;
            [MarshalAs(UnmanagedType.I2)] public short dmColor;
            [MarshalAs(UnmanagedType.I2)] public short dmDuplex;
            [MarshalAs(UnmanagedType.I2)] public short dmYResolution;
            [MarshalAs(UnmanagedType.I2)] public short dmTTOption;
            [MarshalAs(UnmanagedType.I2)] public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public String dmFormName;
            [MarshalAs(UnmanagedType.U2)] public short dmLogPixels;
            [MarshalAs(UnmanagedType.U4)] public int dmBitsPerPel;
            [MarshalAs(UnmanagedType.U4)] public int dmPelsWidth;
            [MarshalAs(UnmanagedType.U4)] public int dmPelsHeight;
            [MarshalAs(UnmanagedType.U4)] public int dmNup;
            [MarshalAs(UnmanagedType.U4)] public int dmDisplayFrequency;
            [MarshalAs(UnmanagedType.U4)] public int dmICMMethod;
            [MarshalAs(UnmanagedType.U4)] public int dmICMIntent;
            [MarshalAs(UnmanagedType.U4)] public int dmMediaType;
            [MarshalAs(UnmanagedType.U4)] public int dmDitherType;
            [MarshalAs(UnmanagedType.U4)] public int dmReserved1;
            [MarshalAs(UnmanagedType.U4)] public int dmReserved2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct PRINTER_INFO_9
        {
            public IntPtr pDevMode;
        }

        [DllImport("winspool.Drv", EntryPoint = "AddFormW", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity()]
        internal static extern bool AddForm(
            IntPtr phPrinter,
            [MarshalAs(UnmanagedType.I4)] int level,
            ref FormInfo1 form);

        [DllImport("winspool.Drv", EntryPoint = "DeleteForm", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern bool DeleteForm(
            IntPtr phPrinter,
            [MarshalAs(UnmanagedType.LPTStr)] string pName);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = false,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern Int32 GetLastError();

        [DllImport("GDI32.dll", EntryPoint = "CreateDC", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern IntPtr CreateDC([MarshalAs(UnmanagedType.LPTStr)] string pDrive,
                                               [MarshalAs(UnmanagedType.LPTStr)] string pName,
                                               [MarshalAs(UnmanagedType.LPTStr)] string pOutput,
                                               ref structDevMode pDevMode);

        [DllImport("GDI32.dll", EntryPoint = "ResetDC", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern IntPtr ResetDC(
            IntPtr hDC,
            ref structDevMode
                pDevMode);

        [DllImport("GDI32.dll", EntryPoint = "DeleteDC", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity()]
        internal static extern bool DeleteDC(IntPtr hDC);

        [DllImport("winspool.Drv", EntryPoint = "SetPrinterA", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurity()]
        internal static extern bool SetPrinter(
            IntPtr hPrinter,
            [MarshalAs(UnmanagedType.I4)] int level,
            IntPtr pPrinter,
            [MarshalAs(UnmanagedType.I4)] int command);

        /* 
         LONG DocumentProperties( 
           HWND hWnd,               // handle to parent window  
           HANDLE hPrinter,         // handle to printer object 
           LPTSTR pDeviceName,      // device name 
           PDEVMODE pDevModeOutput, // modified device mode 
           PDEVMODE pDevModeInput,  // original device mode 
           DWORD fMode              // mode options 
           ); 
         */

        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesA", SetLastError = true,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DocumentProperties(
            IntPtr hwnd,
            IntPtr hPrinter,
            [MarshalAs(UnmanagedType.LPStr)] string pDeviceName,
            IntPtr pDevModeOutput,
            IntPtr pDevModeInput,
            int fMode
            );

        [DllImport("winspool.Drv", EntryPoint = "GetPrinterA", SetLastError = true,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool GetPrinter(
            IntPtr hPrinter,
            int dwLevel,
            IntPtr pPrinter,
            int dwBuf,
            out int dwNeeded
            );

        [Flags]
        internal enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        private const int WM_SETTINGCHANGE = 0x001A;
        private const int HWND_BROADCAST = 0xffff;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result
            );

        //EnumPrinters�õ��ĺ����ͻ����� 
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumPrinters(PrinterEnumFlags Flags, string Name, uint Level,
                                                IntPtr pPrinterEnum, uint cbBuf,
                                                ref uint pcbNeeded, ref uint pcReturned);

        [StructLayout(LayoutKind.Sequential)]
        internal struct PRINTER_INFO_2
        {
            public string pServerName;
            public string pPrinterName;
            public string pShareName;
            public string pPortName;
            public string pDriverName;
            public string pComment;
            public string pLocation;
            public IntPtr pDevMode;
            public string pSepFile;
            public string pPrintProcessor;
            public string pDatatype;
            public string pParameters;
            public IntPtr pSecurityDescriptor;
            public uint Attributes;
            public uint Priority;
            public uint DefaultPriority;
            public uint StartTime;
            public uint UntilTime;
            public uint Status;
            public uint cJobs;
            public uint AveragePPM;
        }

        [Flags]
        internal enum PrinterEnumFlags
        {
            PRINTER_ENUM_DEFAULT = 0x00000001,
            PRINTER_ENUM_LOCAL = 0x00000002,
            PRINTER_ENUM_CONNECTIONS = 0x00000004,
            PRINTER_ENUM_FAVORITE = 0x00000004,
            PRINTER_ENUM_NAME = 0x00000008,
            PRINTER_ENUM_REMOTE = 0x00000010,
            PRINTER_ENUM_SHARED = 0x00000020,
            PRINTER_ENUM_NETWORK = 0x00000040,
            PRINTER_ENUM_EXPAND = 0x00004000,
            PRINTER_ENUM_CONTAINER = 0x00008000,
            PRINTER_ENUM_ICONMASK = 0x00ff0000,
            PRINTER_ENUM_ICON1 = 0x00010000,
            PRINTER_ENUM_ICON2 = 0x00020000,
            PRINTER_ENUM_ICON3 = 0x00040000,
            PRINTER_ENUM_ICON4 = 0x00080000,
            PRINTER_ENUM_ICON5 = 0x00100000,
            PRINTER_ENUM_ICON6 = 0x00200000,
            PRINTER_ENUM_ICON7 = 0x00400000,
            PRINTER_ENUM_ICON8 = 0x00800000,
            PRINTER_ENUM_HIDE = 0x01000000
        }

        //��ӡ��״̬ 
        [Flags]
        internal enum PrinterStatus
        {
            PRINTER_STATUS_BUSY = 0x00000200,
            PRINTER_STATUS_DOOR_OPEN = 0x00400000,
            PRINTER_STATUS_ERROR = 0x00000002,
            PRINTER_STATUS_INITIALIZING = 0x00008000,
            PRINTER_STATUS_IO_ACTIVE = 0x00000100,
            PRINTER_STATUS_MANUAL_FEED = 0x00000020,
            PRINTER_STATUS_NO_TONER = 0x00040000,
            PRINTER_STATUS_NOT_AVAILABLE = 0x00001000,
            PRINTER_STATUS_OFFLINE = 0x00000080,
            PRINTER_STATUS_OUT_OF_MEMORY = 0x00200000,
            PRINTER_STATUS_OUTPUT_BIN_FULL = 0x00000800,
            PRINTER_STATUS_PAGE_PUNT = 0x00080000,
            PRINTER_STATUS_PAPER_JAM = 0x00000008,
            PRINTER_STATUS_PAPER_OUT = 0x00000010,
            PRINTER_STATUS_PAPER_PROBLEM = 0x00000040,
            PRINTER_STATUS_PAUSED = 0x00000001,
            PRINTER_STATUS_PENDING_DELETION = 0x00000004,
            PRINTER_STATUS_PRINTING = 0x00000400,
            PRINTER_STATUS_PROCESSING = 0x00004000,
            PRINTER_STATUS_TONER_LOW = 0x00020000,
            PRINTER_STATUS_USER_INTERVENTION = 0x00100000,
            PRINTER_STATUS_WAITING = 0x20000000,
            PRINTER_STATUS_WARMING_UP = 0x00010000
        }

        //GetDefaultPrinter�õ���API����˵�� 
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int size);

        //SetDefaultPrinter�õ���API�������� 
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string Name);

        //EnumFormsA�õ��ĺ���������Ӧ�ú�EnumPrinters���� 
        [DllImport("winspool.drv", EntryPoint = "EnumForms")]
        internal static extern int EnumFormsA(IntPtr hPrinter, int Level, ref byte pForm, int cbBuf, ref int pcbNeeded,
                                              ref int pcReturned);

        #endregion

        public static int GetPrinterStatusInt(string PrinterName)
        {
            int intRet = 0;
            IntPtr hPrinter;
            structPrinterDefaults defaults = new structPrinterDefaults();

            if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
            {
                int cbNeeded = 0;
                bool bolRet = GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out cbNeeded);
                if (cbNeeded > 0)
                {
                    IntPtr pAddr = Marshal.AllocHGlobal((int) cbNeeded);
                    bolRet = GetPrinter(hPrinter, 2, pAddr, cbNeeded, out cbNeeded);
                    if (bolRet)
                    {
                        PRINTER_INFO_2 Info2 = new PRINTER_INFO_2();

                        Info2 = (PRINTER_INFO_2) Marshal.PtrToStructure(pAddr, typeof (PRINTER_INFO_2));

                        intRet = System.Convert.ToInt32(Info2.Status);
                    }
                    Marshal.FreeHGlobal(pAddr);
                }
                ClosePrinter(hPrinter);
            }

            return intRet;
        }

        internal static PRINTER_INFO_2[] EnumPrintersByFlag(PrinterEnumFlags Flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            bool ret = EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL, null, 2, IntPtr.Zero, 0, ref cbNeeded,
                                    ref cReturned);

            IntPtr pAddr = Marshal.AllocHGlobal((int) cbNeeded);
            ret = EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL, null, 2, pAddr, cbNeeded, ref cbNeeded,
                               ref cReturned);

            if (ret)
            {
                PRINTER_INFO_2[] Info2 = new PRINTER_INFO_2[cReturned];

                int offset = pAddr.ToInt32();

                for (int i = 0; i < cReturned; i++)
                {
                    Info2[i].pServerName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPrinterName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pShareName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPortName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDriverName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pComment = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pLocation = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDevMode = Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].pSepFile = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPrintProcessor = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDatatype = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pParameters = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pSecurityDescriptor = Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Attributes = (uint) Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Priority = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].DefaultPriority = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].StartTime = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].UntilTime = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Status = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].cJobs = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].AveragePPM = (uint) Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                }

                Marshal.FreeHGlobal(pAddr);

                return Info2;
            }
            else
            {
                return new PRINTER_INFO_2[0];
            }
        }

        /// <summary>
        ///   ��ȡ��ǰָ����ӡ����״̬
        /// </summary>
        /// <param name="PrinterName"> ��ӡ������ </param>
        /// <returns> ��ӡ��״̬���� </returns>
        public static string GetPrinterStatus(string PrinterName)
        {
            int intValue = GetPrinterStatusInt(PrinterName);
            string strRet = string.Empty;
            switch (intValue)
            {
                case 0:
                    strRet = "׼��������Ready��";
                    break;
                case 0x00000200:
                    strRet = "æ(Busy��";
                    break;
                case 0x00400000:
                    strRet = "�ű��򿪣�Printer Door Open��";
                    break;
                case 0x00000002:
                    strRet = "����(Printer Error��";
                    break;
                case 0x0008000:
                    strRet = "���ڳ�ʼ��(Initializing��";
                    break;
                case 0x00000100:
                    strRet = "��������������I/O Active��";
                    break;
                case 0x00000020:
                    strRet = "�ֹ���ֽ��Manual Feed��";
                    break;
                case 0x00040000:
                    strRet = "��ī�ۣ�No Toner��";
                    break;
                case 0x00001000:
                    strRet = "�����ã�Not Available��";
                    break;
                case 0x00000080:
                    strRet = "�ѻ���Off Line��";
                    break;
                case 0x00200000:
                    strRet = "�ڴ������Out of Memory��";
                    break;
                case 0x00000800:
                    strRet = "�����������Output Bin Full��";
                    break;
                case 0x00080000:
                    strRet = "��ǰҳ�޷���ӡ��Page Punt��";
                    break;
                case 0x00000008:
                    strRet = "��ֽ��Paper Jam��";
                    break;
                case 0x00000010:
                    strRet = "��ӡֽ���꣨Paper Out��";
                    break;
                case 0x00000040:
                    strRet = "ֽ�����⣨Page Problem��";
                    break;
                case 0x00000001:
                    strRet = "��ͣ��Paused��";
                    break;
                case 0x00000004:
                    strRet = "����ɾ����Pending Deletion��";
                    break;
                case 0x00000400:
                    strRet = "���ڴ�ӡ��Printing��";
                    break;
                case 0x00004000:
                    strRet = "���ڴ���Processing��";
                    break;
                case 0x00020000:
                    strRet = "ī�۲��㣨Toner Low��";
                    break;
                case 0x00100000:
                    strRet = "��Ҫ�û���Ԥ��User Intervention��";
                    break;
                case 0x20000000:
                    strRet = "�ȴ���Waiting��";
                    break;
                case 0x00010000:
                    strRet = "����׼����Warming Up��";
                    break;
                default:
                    strRet = "δ֪״̬��Unknown Status��";
                    break;
            }
            return strRet;
        }

        /// <summary>
        ///   ɾ���Ѿ����ڵ��Զ���ֽ��
        /// </summary>
        /// <param name="PrinterName"> ��ӡ������ </param>
        /// <param name="PaperName"> ֽ������ </param>
        public static void DeleteCustomPaperSize(string PrinterName, string PaperName)
        {
            const int PRINTER_ACCESS_USE = 0x00000008;
            const int PRINTER_ACCESS_ADMINISTER = 0x00000004;

            structPrinterDefaults defaults = new structPrinterDefaults();
            defaults.pDatatype = null;
            defaults.pDevMode = IntPtr.Zero;
            defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;

            IntPtr hPrinter = IntPtr.Zero;

            //�򿪴�ӡ�� 
            if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
            {
                try
                {
                    DeleteForm(hPrinter, PaperName);
                    ClosePrinter(hPrinter);
                }
                catch
                {
                    throw new Exception("ɾ���Զ���ֽ��ʱ��������");
                    //Pub.WinForm.Msg.Warning("ɾ���Զ���ֽ��ʱ��������");
                }
            }
        }

        /// <summary>
        ///   ָ���Ĵ�ӡ��������mmΪ��λ���Զ���ֽ��(Form)
        /// </summary>
        /// <param name="PrinterName"> ��ӡ������ </param>
        /// <param name="PaperName"> Form���� </param>
        /// <param name="WidthInMm"> ��mmΪ��λ�Ŀ�� </param>
        /// <param name="HeightInMm"> ��mmΪ��λ�ĸ߶� </param>
        public static void AddCustomPaperSize(string PrinterName, string PaperName, float WidthInMm, float HeightInMm)
        {
            if (PlatformID.Win32NT == Environment.OSVersion.Platform)
            {
                const int PRINTER_ACCESS_USE = 0x00000008;
                const int PRINTER_ACCESS_ADMINISTER = 0x00000004;
                const int FORM_PRINTER = 0x00000002;

                structPrinterDefaults defaults = new structPrinterDefaults();
                defaults.pDatatype = null;
                defaults.pDevMode = IntPtr.Zero;
                defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;

                IntPtr hPrinter = IntPtr.Zero;

                //�򿪴�ӡ�� 
                if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
                {
                    try
                    {
                        //���Form����ɾ��֮ 
                        DeleteForm(hPrinter, PaperName);
                        //��������ʼ��FORM_INFO_1 
                        FormInfo1 formInfo = new FormInfo1();
                        formInfo.Flags = 0;
                        formInfo.pName = PaperName;
                        formInfo.Size.width = (int) (WidthInMm*1000.0);
                        formInfo.Size.height = (int) (HeightInMm*1000.0);
                        formInfo.ImageableArea.left = 0;
                        formInfo.ImageableArea.right = formInfo.Size.width;
                        formInfo.ImageableArea.top = 0;
                        formInfo.ImageableArea.bottom = formInfo.Size.height;
                        if (!AddForm(hPrinter, 1, ref formInfo))
                        {
                            StringBuilder strBuilder = new StringBuilder();
                            strBuilder.AppendFormat("���ӡ�� {1} ����Զ���ֽ�� {0} ʧ�ܣ�������ţ�{2}",
                                                    PaperName, PrinterName, GetLastError());
                            throw new ApplicationException(strBuilder.ToString());
                        }

                        //��ʼ�� 
                        const int DM_OUT_BUFFER = 2;
                        const int DM_IN_BUFFER = 8;
                        structDevMode devMode = new structDevMode();
                        IntPtr hPrinterInfo, hDummy;
                        PRINTER_INFO_9 printerInfo;
                        printerInfo.pDevMode = IntPtr.Zero;
                        int iPrinterInfoSize, iDummyInt;


                        int iDevModeSize = DocumentProperties(IntPtr.Zero, hPrinter, PrinterName, IntPtr.Zero,
                                                              IntPtr.Zero, 0);

                        if (iDevModeSize < 0)
                            throw new ApplicationException("�޷�ȡ��DEVMODE�ṹ�Ĵ�С��");

                        //���仺�� 
                        IntPtr hDevMode = Marshal.AllocCoTaskMem(iDevModeSize + 100);

                        //��ȡDEV_MODEָ�� 
                        int iRet = DocumentProperties(IntPtr.Zero, hPrinter, PrinterName, hDevMode, IntPtr.Zero,
                                                      DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("�޷����DEVMODE�ṹ��");

                        //���DEV_MODE 
                        devMode = (structDevMode) Marshal.PtrToStructure(hDevMode, devMode.GetType());


                        devMode.dmFields = 0x10000;

                        //FORM���� 
                        devMode.dmFormName = PaperName;

                        Marshal.StructureToPtr(devMode, hDevMode, true);

                        iRet = DocumentProperties(IntPtr.Zero, hPrinter, PrinterName,
                                                  printerInfo.pDevMode, printerInfo.pDevMode,
                                                  DM_IN_BUFFER | DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("�޷�Ϊ��ӡ���趨��ӡ����");

                        GetPrinter(hPrinter, 9, IntPtr.Zero, 0, out iPrinterInfoSize);
                        if (iPrinterInfoSize == 0)
                            throw new ApplicationException("����GetPrinter����ʧ�ܣ�");

                        hPrinterInfo = Marshal.AllocCoTaskMem(iPrinterInfoSize + 100);

                        bool bSuccess = GetPrinter(hPrinter, 9, hPrinterInfo, iPrinterInfoSize, out iDummyInt);

                        if (!bSuccess)
                            throw new ApplicationException("����GetPrinter����ʧ�ܣ�");

                        printerInfo = (PRINTER_INFO_9) Marshal.PtrToStructure(hPrinterInfo, printerInfo.GetType());
                        printerInfo.pDevMode = hDevMode;

                        Marshal.StructureToPtr(printerInfo, hPrinterInfo, true);

                        bSuccess = SetPrinter(hPrinter, 9, hPrinterInfo, 0);

                        if (!bSuccess)
                            throw new Win32Exception(Marshal.GetLastWin32Error(), "����SetPrinter����ʧ�ܣ��޷����д�ӡ�����ã�");

                        SendMessageTimeout(
                            new IntPtr(HWND_BROADCAST),
                            WM_SETTINGCHANGE,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            Printer.SendMessageTimeoutFlags.SMTO_NORMAL,
                            1000,
                            out hDummy);
                    }
                    finally
                    {
                        ClosePrinter(hPrinter);
                    }
                }
                else
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat("�޷��򿪴�ӡ��{0}, �������: {1}",
                                            PrinterName, GetLastError());
                    throw new ApplicationException(strBuilder.ToString());
                }
            }
            else
            {
                structDevMode pDevMode = new structDevMode();
                IntPtr hDC = CreateDC(null, PrinterName, null, ref pDevMode);
                if (hDC != IntPtr.Zero)
                {
                    const long DM_PAPERSIZE = 0x00000002L;
                    const long DM_PAPERLENGTH = 0x00000004L;
                    const long DM_PAPERWIDTH = 0x00000008L;
                    pDevMode.dmFields = (int) (DM_PAPERSIZE | DM_PAPERWIDTH | DM_PAPERLENGTH);
                    pDevMode.dmPaperSize = 256;
                    pDevMode.dmPaperWidth = (short) (WidthInMm*1000.0);
                    pDevMode.dmPaperLength = (short) (HeightInMm*1000.0);
                    ResetDC(hDC, ref pDevMode);
                    DeleteDC(hDC);
                }
            }
        }

        /// <summary>
        ///   ��ȡ���ش�ӡ���б� 
        ///   ����ͨ���ƶ�������ȡ�����ӡ��
        /// </summary>
        /// <returns> ��ӡ���б� </returns>
        public static System.Collections.ArrayList GetPrinterList()
        {
            System.Collections.ArrayList alRet = new System.Collections.ArrayList();
            PRINTER_INFO_2[] Info2 = EnumPrintersByFlag(PrinterEnumFlags.PRINTER_ENUM_LOCAL);
            for (int i = 0; i < Info2.Length; i++)
            {
                alRet.Add(Info2[i].pPrinterName);
            }
            return alRet;
        }

        /// <summary>
        ///   ��ȡ������Ĭ�ϴ�ӡ������
        /// </summary>
        /// <returns> Ĭ�ϴ�ӡ������ </returns>
        public static string GetDeaultPrinterName()
        {
            StringBuilder dp = new StringBuilder(256);
            int size = dp.Capacity;
            if (GetDefaultPrinter(dp, ref size))
            {
                return dp.ToString();
            }
            else
            {
                int rc = GetLastError();
                //Pub.WinForm.Msg.Warning("��ȡĬ�ϴ�ӡ��ʧ�ܣ�������ţ�" + rc.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        ///   ����Ĭ�ϴ�ӡ��
        /// </summary>
        /// <param name="PrinterName"> ���õĴ�ӡ������ </param>
        public static void SetPrinterToDefault(string PrinterName)
        {
            SetDefaultPrinter(PrinterName);
        }

        ///// <summary> 
        ///// �жϴ�ӡ���Ƿ���ϵͳ���õĴ�ӡ���б��� 
        ///// </summary> 
        ///// <param name="PrinterName">��ӡ������</param> 
        ///// <returns>�ǣ��ڣ��񣺲���</returns> 
        public static bool PrinterInList(string PrinterName)
        {
            bool bolRet = false;

            System.Collections.ArrayList alPrinters = GetPrinterList();

            for (int i = 0; i < alPrinters.Count; i++)
            {
                if (PrinterName == alPrinters[i].ToString())
                {
                    bolRet = true;
                    break;
                }
            }

            alPrinters.Clear();
            alPrinters = null;

            return bolRet;
        }

        ///// <summary> 
        ///// �жϱ��Ƿ���ָ���Ĵ�ӡ����֧�ֵ�ֽ���б��� 
        ///// </summary> 
        ///// <param name="PrinterName">��ӡ������</param> 
        ///// <param name="PaperName">ֽ������</param> 
        ///// <returns>�ǣ��ڣ��񣺲���</returns> 
        public static bool FormInPrinter(string PrinterName, string PaperName)
        {
            bool bolRet = false;

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

            pd.PrinterSettings.PrinterName = PrinterName;

            foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName == PaperName)
                {
                    bolRet = true;
                    break;
                }
            }

            pd.Dispose();

            return bolRet;
        }

        /// <summary>
        ///   �ж�ָ��ֽ�ŵĿ�Ⱥ͸߶Ⱥ����ı�����ָ���Ŀ�Ⱥ͸߶��Ƿ�ƥ��
        /// </summary>
        /// <param name="PrinterName"> ��ӡ������ </param>
        /// <param name="FormName"> ������ </param>
        /// <param name="Width"> ��� </param>
        /// <param name="Height"> �߶� </param>
        /// <returns> </returns>
        public static bool FormSameSize(string PrinterName, string FormName, decimal Width, decimal Height)
        {
            //bool bolRet = false;

            //System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

            //pd.PrinterSettings.PrinterName = PrinterName;

            //foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
            //{
            //    if (ps.PaperName == FormName)
            //    {
            //        decimal decWidth = FromInchToCM(System.Convert.ToDecimal(ps.Width));
            //        decimal decHeight = FromInchToCM(System.Convert.ToDecimal(ps.Height));
            //        //ֻҪ����λ��ͬ����Ϊ��ͬһֽ�ţ��Ͼ�inch��cm��ת������������ 
            //        if (Pub.MathEx.Round(decWidth, 0) == Pub.MathEx.Round(Width, 0) && Pub.MathEx.Round(decHeight, 0) == Pub.MathEx.Round(Height, 0))
            //            bolRet = true;
            //        break;
            //    }
            //}

            //pd.Dispose();

            //return bolRet;

            throw new NotImplementedException();
        }

        /// <summary>
        ///   Ӣ�ߵ����׵�ת�� 
        ///   �׹����õ���Ӣ�ߣ��й����õ������� 
        ///   1 inch = 2.5400 cm
        /// </summary>
        /// <param name="inch"> Ӣ���� </param>
        /// <returns> ����������λС�� </returns>
        public static decimal FromInchToCM(decimal inch)
        {
            return Math.Round((System.Convert.ToDecimal((inch/100))*System.Convert.ToDecimal(2.5400)), 2);
        }
    }
}