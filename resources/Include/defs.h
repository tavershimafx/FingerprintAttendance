
#ifndef __Defs_h__
#define __Defs_h__

#include <stddef.h>
#include <stdlib.h>

#if !defined( _WIN32 ) && !defined( _WIN32_WCE )
#include <stdint.h> // required for "uintptr_t" defifnition in some Linux
#endif

/* Это включение нужно чтобы получить правильный calling conversion т.к. на данной платформе  
   __stdcall переопределяется в __cdecl*/
#if defined(_WIN32_WCE) && defined(_X86_)
#include <windef.h>  
#endif

#include "byteordering.h"

#ifdef __cplusplus
extern "C" { /* assume C declarations for C++ */
#endif

typedef signed char    DGT8;
typedef unsigned char  UDGT8;
typedef signed short   DGT16;
typedef unsigned short UDGT16;
typedef signed long    DGT32;
typedef unsigned long  UDGT32;
typedef unsigned long long UDGT64;
typedef double         DGTFLOAT;
typedef void           DGTVOID;
typedef DGT32          DGTBOOL;

#if defined( ARMLIN_DEBUG ) || defined( _WIN32_WCE )
typedef ptrdiff_t      DGTPOINTER; 
typedef ptrdiff_t      UDGTPOINTER; 
#else
typedef uintptr_t      DGTPOINTER; 
typedef uintptr_t      UDGTPOINTER; 
#endif
 
#ifndef FALSE
#define FALSE               0
#endif

#ifndef TRUE
#define TRUE                1
#endif

#ifndef NULL
#ifdef __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif

/* Futronic API function calling convention definition. */
#if defined _WIN32
	#if defined STATIC_FTRAPI_LIB
	#  define FTRAPIPREFIX
	#else
#  define FTRAPIPREFIX __declspec(dllimport)
	#endif
#  define FTRAPI __stdcall
#  define FTR_CBAPI __stdcall
#else
#  define FTRAPIPREFIX
#  define FTRAPI 
#  define FTR_CBAPI 
#endif

//
//Declare pack macros. For non Win32 it is gcc specific definition
//
#if defined _WIN32
#  define FTRPACK 
#else
#  define FTRPACK __attribute__ ((aligned(1),packed)) 
#endif

#ifdef __cplusplus
};
#endif

#endif
