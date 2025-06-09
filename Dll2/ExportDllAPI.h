#ifndef ExportDllAPI_111
#define ExportDllAPI_111

#if _MSC_VER >= 1200
	#pragma once
#endif //_MSC_VER >= 1200  如果是高级语法直接用#pragma once


#ifndef EXPORTDLL_EXPORTS
	#define EXPORTDLL_API extern "C" _declspec(dllimport)
#else
	#define EXPORTDLL_API extern "C" _declspec(dllexport)
#endif//!EXPORTDLL_EXPORTS  函数是导出还是导入


/************************************************************************/
/*                          1. 调用约定                                 */
/************************************************************************/
//1.1 标准调用约定
EXPORTDLL_API void _stdcall CallingCvt_Stdcall();

//1.2 C调用约定
EXPORTDLL_API void _cdecl   CallingCvt_Cdecl();


/************************************************************************/
/*                          2. 函数指针                                 */
/************************************************************************/

typedef int (*PCALLBACKFUN)(int i);

EXPORTDLL_API PCALLBACKFUN CallBack_GetFunPtr();


/************************************************************************/
/*                          3. 字符串                                   */
/************************************************************************/

EXPORTDLL_API int Str_Output(WCHAR* pStr);

EXPORTDLL_API int Str_Change(WCHAR* pStr, int len);

//3.3 字符串作为返回值
static const WCHAR* g_StrReturn = L"Str_Return";
EXPORTDLL_API const WCHAR* Str_Return();


EXPORTDLL_API int Str_ChangeArr(WCHAR** ppStr, int len);


EXPORTDLL_API void Str_ParameterOut(WCHAR** ppStr);




/************************************************************************/
/*                          4. 错误码                                   */
/************************************************************************/
EXPORTDLL_API void Err_Get();



/************************************************************************/
/*                          5. 结构体                                   */
/************************************************************************/
//4.1 结构体作为输入输出参数
typedef struct _testStru1
{
	int		iVal;
	char	cVal;
	__int64 llVal;
}testStru1;
EXPORTDLL_API  void Struct_Change(testStru1* pStru);


//4.2 结构体边界对齐
#pragma pack(push)
#pragma pack(1)
typedef struct _testStru2
{
	int		iVal;
	char	cVal;
	__int64 llVal;
}testStru2;
#pragma pack(pop)
EXPORTDLL_API  void Struct_PackN(testStru2* pStru);

//4.3 结构体中含有内置数据类型的数组
typedef struct _testStru3
{
	int		iValArrp[30];
	WCHAR	szChArr[30];
}testStru3;
EXPORTDLL_API  void Struct_ChangeArr(testStru3* pStru);


//4.4 union类型中含有结构体
typedef union _testStru4
{
	int		iValLower;
	int		iValUpper;
	struct
	{
		__int64 llLocation;
	};
}testStru4;
EXPORTDLL_API  void Struct_Union(testStru4* pStru);

//4.5 结构体作为返回值
typedef struct _testStru5
{
	int		iVal;
}testStru5;
testStru5	g_stru5;
EXPORTDLL_API  testStru5* Struct_Return();


//4.6 结构体数组作为参数
typedef struct _testStru6
{
	int		iVal;
}testStru6;
EXPORTDLL_API  void Struct_StruArr(testStru6* pStru, int len);


//4.7 结构体中含有内置数据类型的二维数组
typedef struct _testStru7
{
	int		m[5][5];
}testStru7;
EXPORTDLL_API  void Struct_Change2DArr(testStru7* pStru);


//4.8 结构体作为返出参数，释放非托管的内存
typedef struct _testStru8
{
	int		m;
}testStru8;
EXPORTDLL_API  void Struct_ParameterOut(testStru8** ppStru);


//4.9 结构体中含有字符串--指针
typedef struct _testStru9
{
	WCHAR* pWChArr;
	CHAR* pChArr;
	bool	IsCbool;
	BOOL	IsBOOL;
}testStru9;
EXPORTDLL_API  void Struct_ChangePtr(testStru9* pStru);

//4.10 结构体中嵌套结构体指针
typedef struct _testStru10Pre
{
	int  iVal;
}testStru10Pre;
typedef struct _testStru10
{
	testStru10Pre* pPre;
	long		  lVal;
	_testStru10()
	{
		pPre = NULL;
	}
}testStru10;
EXPORTDLL_API  void Struct_NestStruct(testStru10* pStru);






#endif //!ExportDllAPI_111 保证文件只包含一次