

#ifndef __BYTEORDERING_H__
#define __BYTEORDERING_H__


#if !defined( FTR_LITTLE_ENDIAN ) && !defined( FTR_BIG_ENDIAN )
#    if defined( _WIN32 ) //Under Windows we have little endian
#        define FTR_LITTLE_ENDIAN
#    elif defined __GNUC__ //Under Linux lets gcc determinate byte ordering 
#        ifdef BYTES_BIG_ENDIAN
#            define FTR_BIG_ENDIAN
#        else
#            define FTR_LITTLE_ENDIAN
#        endif
#    endif
#endif


#if defined( __sparc ) || defined( sparc )
#   define FTR_BIG_ENDIAN
#endif


//
// Use outer configuration for determinate byte ordering
//
#if !defined( FTR_LITTLE_ENDIAN )  && !defined( FTR_BIG_ENDIAN )

#    if defined( BIG_ENDIAN ) && defined( LITTLE_ENDIAN ) && defined( BYTE_ORDER )
#        if( BYTE_ORDER == BIG_ENDIAN ) && !defined( DATA_BIGENDIAN )
#            define FTR_BIG_ENDIAN
#        elif !defined( CTLIB_LITTLE_ENDIAN )
#            define FTR_LITTLE_ENDIAN
#        endif
#    endif

#endif

#if !defined( FTR_LITTLE_ENDIAN )  && !defined( FTR_BIG_ENDIAN )
#    if defined( __BIG_ENDIAN) && defined( __LITTLE_ENDIAN ) && defined( __BYTE_ORDER )
#        if( __BYTE_ORDER == __BIG_ENDIAN ) && !defined( DATA_BIGENDIAN )
#            define FTR_BIG_ENDIAN
#        elif !defined( CTLIB_LITTLE_ENDIAN )
#            define FTR_LITTLE_ENDIAN
#        endif
#    endif
#endif

//
// Used shift operation to determinate byte ordering
//
#if !defined( FTR_LITTLE_ENDIAN ) && !defined( FTR_BIG_ENDIAN )
#    if( ( (unsigned short)('AB') >> 8) == 'B' )
#        define FTR_LITTLE_ENDIAN
#    elif( ( (unsigned short)('AB') >> 8) == 'A' )
#        define FTR_BIG_ENDIAN
#    else
#        error Cannot determine processor endianness - edit ByteOrdering.h and recompile
#    endif
#endif


#ifdef FTR_BIG_ENDIAN
#error Big endian byte ordering not supported now
#endif


#endif //__BYTEORDERING_H__
