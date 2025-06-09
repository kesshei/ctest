using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace cload
{
    internal class Program
    {
        const string dll = @"E:\临时\ctest\x64\Debug\Dll2.dll";
        [DllImport(dll, EntryPoint = "add", CallingConvention = CallingConvention.Cdecl)]
        public static extern int add(int q, int a);


        /************************************************************************/
        /*                          1. 调用约定                                   */
        /************************************************************************/
        //1.1 标准调用约定
        [DllImport(dll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern void CallingCvt_Stdcall();

        //1.2 C调用约定
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void CallingCvt_Cdecl();


        /************************************************************************/
        /*                          2. 函数指针                                   */
        /************************************************************************/
        //2.1 获取回调函数的函数地址
        public delegate int DelegateGetFunPtrType(int i);
        [DllImport(dll, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.FunctionPtr)]
        public static extern DelegateGetFunPtrType CallBack_GetFunPtr();

        /************************************************************************/
        /*                          3. 字符串                                   */
        /************************************************************************/

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern int Str_Output([MarshalAs(UnmanagedType.LPWStr)] string pStr);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern int Str_Change([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pStr, int len);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern IntPtr Str_Return();
        //3.4 字符串数组作为参数,每个元素长度为10
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern int Str_ChangeArr([In, Out] string[] ppStr, int len);

        //3.5 释放非托管的内存,128
        //使用CoTaskMemAlloc方法申请的内存,则会自动调用CoTaskMemFree来释放非托管内存
        //这就意味了托管代码无需处理内存问题，减轻了托管代码的的复杂度
        //但.NET只能释放由CoTaskMemAlloc分配的内存，所以如果底层不是使用CoTaskMemAlloc申请的内存，必须定义对应的释放函数
        [DllImport(dll, CharSet = CharSet.Unicode, EntryPoint = "Str_ParameterOut")]
        public static extern void Str_ParameterOutString(ref string ppStr);
        //使用IntPtr接受时，需要手动释放
        [DllImport(dll, CharSet = CharSet.Unicode, EntryPoint = "Str_ParameterOut")]
        public static extern void Str_ParameterOuttPtr(ref IntPtr ppStr);


        /************************************************************************/
        /*                          4. 错误码                                    */
        /************************************************************************/
        //5.1 获取错误码
        [DllImport(dll, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void Err_Get();
        /************************************************************************/
        /*                          5. 结构体                                   */
        /************************************************************************/
        /*   1.以StructLayout来标记结构体,指定结构体内存布局
          *   2.字段定义的顺序 
          *   3.字段类型 
          *   4.字段在内存中的大小 
          *   5.非托管与托管结构名称可以不同 
        */
        //4.1 结构体作为输入输出参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru1
        {
            public int iVal;
            public sbyte cVal;
            public long llVal;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_Change(ref testStru1 pStru);

        //4.2 结构体边界对齐
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        public struct testStru2
        {
            public int iVal;
            public sbyte cVal;
            public long llVal;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_PackN(ref testStru2 pStru);


        //4.3 结构体中含有内置数据类型的数组
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru3
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public int[] iValArrp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string szChArr;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_ChangeArr(ref testStru3 pStru);


        //4.4 union类型中含有结构体
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct testStru4
        {
            [FieldOffset(0)]
            int iValLower;
            [FieldOffset(4)]
            int iValUpper;
            [FieldOffset(0)]
            long llLocation;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_Union(ref testStru4 pStru);


        //4.5 结构体作为返回值
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru5
        {
            public int iVal;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern IntPtr Struct_Return();

        //4.6 结构体数组作为参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru6
        {
            public int iVal;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_StruArr([In, Out] testStru6[] pStru, int len);


        //4.7 结构体中含有内置数据类型的二维数组
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru7Pre
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] m;
        };
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru7
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public testStru7Pre[] m;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_Change2DArr(ref testStru7 pStru);

        //4.8 结构体作为返出参数，释放非托管的内存
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru8
        {
            public int m;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_ParameterOut(ref IntPtr ppStru);


        //4.9 结构体中含有字符串--指针
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct testStru9
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pWChArr;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pChArr;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsCbool;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsBOOL;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_ChangePtr(ref testStru9 pStru);


        //4.10 结构体中嵌套结构体指针
        public struct testStru10Pre
        {
            public int iVal;
        };
        public struct testStru10
        {
            public IntPtr pPre;
            public int lVal;
        };
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void Struct_NestStruct(ref testStru10 pStru);
        static void Main(string[] args)
        {
            CallingCvt_Stdcall();
            CallingCvt_Cdecl();


            var b = CallBack_GetFunPtr();
            b(10);


            Console.WriteLine(Str_Output("Hello, World!"));
            Console.WriteLine(Str_Change(new StringBuilder("Hello, World!"), 5));

            IntPtr strPtr = Str_Return();
            string strIntPtr = Marshal.PtrToStringUni(strPtr);
            //3.4 字符串数组作为参数,每个元素长度为10
            string[] strArr = new string[4] {new string('\0', 10),
                                             new string('\0', 10),
                                             new string('\0', 10),
                                             new string('\0', 10) };
            Str_ChangeArr(strArr, 4);
            Console.WriteLine(string.Join(",", strArr));


            //3.5 释放非托管的内存,128
            string strOut = "";
            Str_ParameterOutString(ref strOut);
            //手动释放
            IntPtr strOutIntPtr = IntPtr.Zero;
            Str_ParameterOuttPtr(ref strOutIntPtr);
            string strOut2 = Marshal.PtrToStringUni(strOutIntPtr);
            Marshal.FreeCoTaskMem(strOutIntPtr);

            Err_Get();
            Win32Exception win32Exp = new Win32Exception();
            Console.WriteLine(win32Exp.Message);



            testStru1 stru1 = new testStru1();
            Struct_Change(ref stru1);

            //4.2 结构体边界对齐
            testStru2 stru2 = new testStru2();
            Struct_PackN(ref stru2);



            //4.3 结构体中含有内置数据类型的数组
            testStru3 stru3 = new testStru3();
            Struct_ChangeArr(ref stru3);

            //4.4 union类型中含有结构体
            testStru4 stru4 = new testStru4();
            Struct_Union(ref stru4);

            //4.5 结构体作为返回值
            IntPtr struIntPtr = Struct_Return();
            testStru5 stru5 = (testStru5)(Marshal.PtrToStructure(struIntPtr, typeof(testStru5)));


            //4.6 结构体数组作为参数
            testStru6[] stru6 = new testStru6[5];
            Struct_StruArr(stru6, 5);

            //4.7 结构体中含有内置数据类型的二维数组
            testStru7 stru7 = new testStru7();
            Struct_Change2DArr(ref stru7);


            //4.8 结构体作为返出参数，释放非托管的内存
            IntPtr outPtr = IntPtr.Zero;
            Struct_ParameterOut(ref outPtr);
            testStru8 stru8 = (testStru8)(Marshal.PtrToStructure(outPtr, typeof(testStru8)));
            Marshal.FreeCoTaskMem(outPtr);

            //4.9 结构体中含有字符串--指针
            testStru9 stru9 = new testStru9();
            Struct_ChangePtr(ref stru9);

            //4.10 结构体中嵌套结构体指针
            testStru10Pre str10Pre = new testStru10Pre();
            IntPtr intPtrStru10Pre = Marshal.AllocCoTaskMem(Marshal.SizeOf(str10Pre));
            Marshal.StructureToPtr(str10Pre, intPtrStru10Pre, false);

            testStru10 stru10 = new testStru10();
            stru10.pPre = intPtrStru10Pre;
            Struct_NestStruct(ref stru10);
            testStru10Pre str10Pre2 = (testStru10Pre)Marshal.PtrToStructure(stru10.pPre, typeof(testStru10Pre));

            Marshal.DestroyStructure(intPtrStru10Pre, typeof(testStru10Pre));

            Console.ReadLine();
        }
    }
}
