/*
 * This file is intended to be used as the include file for the
 * Futronic Software Development Kit consumers.
 * All necessary declarations and constants definitions are placed here.
 *
 * Copyright (c) 2003-2008 Futronic Technology Company Ltd.
 * All rights reserved.
 */

#ifndef _FUTRONIC_API_H_
#define _FUTRONIC_API_H_

#include "defs.h"


/* API function return value type definition. */
typedef DGT32 FTRAPI_RESULT;

// Return code values.
#define FTR_RETCODE_OK             0 // Successful function completion.

#define FTR_RETCODE_ERROR_BASE     1 // Base value for the error codes.
#define FTR_RETCODE_DEVICE_BASE  200 // Base value for the device error codes.

#define FTR_RETCODE_NO_MEMORY             (FTR_RETCODE_ERROR_BASE + 1)
#define FTR_RETCODE_INVALID_ARG           (FTR_RETCODE_ERROR_BASE + 2)
#define FTR_RETCODE_ALREADY_IN_USE        (FTR_RETCODE_ERROR_BASE + 3)
#define FTR_RETCODE_INVALID_PURPOSE       (FTR_RETCODE_ERROR_BASE + 4)
#define FTR_RETCODE_INTERNAL_ERROR        (FTR_RETCODE_ERROR_BASE + 5)

#define FTR_RETCODE_UNABLE_TO_CAPTURE     (FTR_RETCODE_ERROR_BASE + 6)
#define FTR_RETCODE_CANCELED_BY_USER      (FTR_RETCODE_ERROR_BASE + 7)
#define FTR_RETCODE_NO_MORE_RETRIES       (FTR_RETCODE_ERROR_BASE + 8)
#define FTR_RETCODE_INCONSISTENT_SAMPLING (FTR_RETCODE_ERROR_BASE + 10)
#define FTR_RETCODE_TRIAL_EXPIRED         (FTR_RETCODE_ERROR_BASE + 11)

#define FTR_RETCODE_FRAME_SOURCE_NOT_SET  (FTR_RETCODE_DEVICE_BASE + 1)
#define FTR_RETCODE_DEVICE_NOT_CONNECTED  (FTR_RETCODE_DEVICE_BASE + 2)
#define FTR_RETCODE_DEVICE_FAILURE        (FTR_RETCODE_DEVICE_BASE + 3)
#define FTR_RETCODE_EMPTY_FRAME           (FTR_RETCODE_DEVICE_BASE + 4)
#define FTR_RETCODE_FAKE_SOURCE           (FTR_RETCODE_DEVICE_BASE + 5)
#define FTR_RETCODE_INCOMPATIBLE_HARDWARE (FTR_RETCODE_DEVICE_BASE + 6)
#define FTR_RETCODE_INCOMPATIBLE_FIRMWARE (FTR_RETCODE_DEVICE_BASE + 7)
#define FTR_RETCODE_FRAME_SOURCE_CHANGED  (FTR_RETCODE_DEVICE_BASE + 8)
#define FTR_RETCODE_INCOMPATIBLE_SOFTWARE  (FTR_RETCODE_DEVICE_BASE + 9)


/*
 * Constants and types definitions section.
 */
typedef UDGTPOINTER FTR_HANDLE, *FTR_HANDLE_PTR;

// Values used for the parameter definition.
typedef UDGT32 FTR_PARAM;

#define FTR_PARAM_IMAGE_WIDTH        (1)
#define FTR_PARAM_IMAGE_HEIGHT       (2) 
#define FTR_PARAM_IMAGE_SIZE         (3)

#define FTR_PARAM_CB_FRAME_SOURCE    (4)
#define FTR_PARAM_CB_CONTROL         (5)

#define FTR_PARAM_MAX_MODELS         (10)
#define FTR_PARAM_MAX_TEMPLATE_SIZE  (6)
#define FTR_PARAM_MAX_FAR_REQUESTED  (7)
#define FTR_PARAM_MAX_FARN_REQUESTED (13)

#define FTR_PARAM_SYS_ERROR_CODE     (8)

#define FTR_PARAM_FAKE_DETECT        (9)
#define FTR_PARAM_FFD_CONTROL        (11)

#define FTR_PARAM_MIOT_CONTROL       (12)

#define FTR_PARAM_VERSION            (14)

#define FTR_PARAM_CHECK_TRIAL        (15)

#define FTR_PARAM_FAST_MODE          (16)

#define FTR_PARAM_SCANAPI_ANDROID_CONTEXT (100)

typedef DGTVOID *FTR_PARAM_VALUE;

// Available frame sources. These device identifiers are intended to be used
// with the FTR_PARAM_CB_FRAME_SOURCE parameter.
#define FSD_UNDEFINED    (0)  // No device attached.
#define FSD_FUTRONIC_USB (1)  // Futronic USB Fingerprint Scanner Device.

// Values used for the purpose definition.
typedef UDGT32 FTR_PURPOSE;

//#define FTR_PURPOSE_VERIFY   (1)
#define FTR_PURPOSE_IDENTIFY (2)
#define FTR_PURPOSE_ENROLL   (3)
#define FTR_PURPOSE_COMPATIBILITY (4)

// Values used for the version definition.
typedef UDGT32 FTR_VERSION;

#define FTR_VERSION_PREVIOUS   (1)
#define FTR_VERSION_COMPATIBLE (2)
#define FTR_VERSION_CURRENT    (3)

// User callback context.
typedef DGTVOID *FTR_USER_CTX;

// State bit mask values.
typedef UDGT32 FTR_STATE;

#define FTR_STATE_FRAME_PROVIDED   (0x01)
#define FTR_STATE_SIGNAL_PROVIDED  (0x02)

// Signal values.
typedef UDGT32 FTR_SIGNAL, *FTR_SIGNAL_PTR;

#define FTR_SIGNAL_UNDEFINED    (0)
#define FTR_SIGNAL_TOUCH_SENSOR (1)
#define FTR_SIGNAL_TAKE_OFF     (2)
#define FTR_SIGNAL_FAKE_SOURCE  (3)

// Response values.
typedef UDGT32 FTR_RESPONSE;

#define FTR_CANCEL   (1)
#define FTR_CONTINUE (2)

// False acception ratio (FAR) types.
typedef UDGT32 FTR_FAR;
typedef DGT32  FTR_FARN; // Numerical form.

// External key type.
typedef DGT8 FTR_DATA_KEY[16];

/*
 * Structured types definitions section.
 * Note, that all structures and unions are aligned on a byte boundary.
 */
#if defined _WIN32
#pragma pack( push, 1 )
#endif

// Generic byte data.
typedef struct FTRPACK
{
  UDGT32 dwSize; // Length of data in bytes.
  DGTVOID *pData; // Data pointer.
} FTR_DATA, *FTR_DATA_PTR;

typedef struct FTRPACK
{
  UDGT32    Width;
  UDGT32    Height;
  FTR_DATA Bitmap;
} FTR_BITMAP, *FTR_BITMAP_PTR;

typedef struct FTRPACK
{ // Data capture progress information.
  UDGT32 dwSize; // The size of the structure in bytes.
  UDGT32 dwCount; // Currently requested frame number.
  DGTBOOL  bIsRepeated; // Flag indicating whether the frame is requested not the first time.
  UDGT32 dwTotal; // Total number of frames to be captured.
} FTR_PROGRESS, *FTR_PROGRESS_PTR;

// Data types used for operation of identification.
typedef struct FTRPACK
{ // Identify record description.
  FTR_DATA_KEY KeyValue;
  FTR_DATA_PTR pData;
} FTR_IDENTIFY_RECORD, *FTR_IDENTIFY_RECORD_PTR;

typedef struct FTRPACK
{ // Array of identify records.
  UDGT32                   TotalNumber;
  FTR_IDENTIFY_RECORD_PTR pMembers;
} FTR_IDENTIFY_ARRAY, *FTR_IDENTIFY_ARRAY_PTR;

typedef struct FTRPACK
{ // Match record description.
  FTR_DATA_KEY KeyValue;
  FTR_FAR      FarAttained;
} FTR_MATCHED_RECORD, *FTR_MATCHED_RECORD_PTR;

typedef struct FTRPACK
{ // Array of match records.
  UDGT32                  TotalNumber;
  FTR_MATCHED_RECORD_PTR pMembers;
} FTR_MATCHED_ARRAY, *FTR_MATCHED_ARRAY_PTR;

typedef union FTRPACK
{ // FAR presentation value.
  FTR_FAR  P; // Stochastic FAR.
  FTR_FARN N; // Numerical FAR value.
} FAR_ATTAINED, *FAR_ATTAINED_PTR;

typedef struct FTRPACK
{ // Extended match record description.
  FTR_DATA_KEY KeyValue;
  FAR_ATTAINED FarAttained;
} FTR_MATCHED_X_RECORD, *FTR_MATCHED_X_RECORD_PTR;

typedef struct FTRPACK
{ // Array of match records with numerical FAR value.
  UDGT32                    TotalNumber;
  FTR_MATCHED_X_RECORD_PTR pMembers;
} FTR_MATCHED_X_ARRAY, *FTR_MATCHED_X_ARRAY_PTR;

// Data types used for enrollment.
typedef struct FTRPACK 
{ // Results of the enrollment process.
  UDGT32 dwSize; // The size of the structure in bytes.
  UDGT32 dwQuality; // Estimation of a template quality in terms of recognition:
                   // 1 corresponds to the worst quality, 10 denotes the best.
} FTR_ENROLL_DATA, *FTR_ENROLL_DATA_PTR;

#if defined _WIN32
#pragma pack( pop )
#endif
/* End of structured types definitions. */

// A callback function that is supplied by an application and used
// to control capture, enrollment or verification execution flow.
typedef void (FTR_CBAPI *FTR_CB_STATE_CONTROL)(
  FTR_USER_CTX   Context, /* (input) - user-defined context information. */
  FTR_STATE      StateMask, /* (input) - a bit mask indicating what arguments are provided. */
  FTR_RESPONSE  *pResponse, /* (output) - API function execution control is achieved through this value. */
  FTR_SIGNAL     Signal, /* (input) - this signal should be used to interact with a user. */
  FTR_BITMAP_PTR pBitmap /* (input) - a pointer to the bitmap to be displayed. */
);

#ifdef __cplusplus
extern "C" { /* assume C declarations for C++ */
#endif
/*
 * API function declarations section.
 */

/*
 * FTRInitialize - activates the Futronic API interface. This function must be called
 *   before any other API call.
 *
 * Parameters:
 *   This function has no parameters.
 *
 *  Returns result code.
 *   FTR_RETCODE_OK - success, to finish the API usage call the FTRTerminate function;
 *   FTR_RETCODE_ALREADY_IN_USE - the API has been already initialized;
 *   FTR_RETCODE_NO_MEMORY - not enough memory to perform the operation.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRInitialize( void );

/*
 * FTRTerminate - releases all previously allocated resources and completes the API usage.
 *   This call must be the last API call in the case of SUCCESSFULL FTRInitialize return.
 *
 * Parameters:
 *   This function has no parameters.
 *
 * Returns void.
 */
FTRAPIPREFIX void FTRAPI FTRTerminate( void );

/*
 * FTRSetParam - sets the indicated parameter value.
 *
 * Parameters:
 *   FTR_PARAM Param - this argument indicates the parameter which value is passed through
 *                     the Value argument;
 *
 *   FTR_PARAM_VALUE Value - the value to be set.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success,
 *   FTR_RETCODE_INVALID_ARG - the value of the required parameter could not be set,
 *   FTR_RETCODE_NO_MEMORY - not enough memory to perform the operation.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRSetParam( FTR_PARAM Param, FTR_PARAM_VALUE Value );

/*
 * FTRGetParam - gets the value of the specified parameter.
 *
 * Parameters:
 *   FTR_PARAM Param - this argument indicates the parameter which value must be placed
 *                     at the address passed through the pValue argument;
 *
 *   FTR_PARAM_VALUE *pValue - a pointer to the value.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success, the required value is written at the pValue address.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRGetParam( FTR_PARAM Param, FTR_PARAM_VALUE *pValue );

/*
 * FTRCaptureFrame - gets an image from the current frame source.
 *
 * Parameters:
 *   FTR_USER_CTX UserContext - optional caller-supplied value that is passed to callback
 *                              functions. This value is provided for convenience in
 *                              application design.
 *
 *   void *pFrameBuf - points to a buffer large enough to hold the frame data.
 *                     The size of a frame can be determined through the FTRGetParam call
 *                     with the FTR_PARAM_IMAGE_SIZE value of the first argument.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRCaptureFrame( FTR_USER_CTX UserContext, DGTVOID *pFrameBuf );

/*
 * FTREnrollX - creates the fingerprint template for the desired purpose.
 *
 * Parameters:
 *   FTR_USER_CTX UserContext - optional caller-supplied value that is passed to callback
 *                              functions. This value is provided for convenience in
 *                              application design.
 *
 *   FTR_PURPOSE Purpose - the purpose of template building. This value designates the
 *                         intended way of further template usage and can be one of the
 *                         following:
 *
 *                         FTR_PURPOSE_ENROLL - the created template is suitable for both
 *                         identification and verification purpose.
 *
 *                         FTR_PURPOSE_IDENTIFY - corresponding template can be used only
 *                         for identification as an input for the FTRSetBaseTemplate function.
 *
 *   FTR_DATA_PTR pTemplate - pointer to a result memory buffer. The space for this buffer
 *                            must be reservered by a caller. Maximum space amount can be
 *                            determined through the FTRGetParam call with the
 *                            FTR_PARAM_MAX_TEMPLATE_SIZE value of the first argument.
 *
 *   FTR_ENROLL_DATA_PTR pEData - optional pointer to the FTR_ENROLL_DATA structure that
 *                                receives on output additional information on the results of
 *                                the enrollment process. The caller must set the dwSize member
 *                                of this structure to sizeof(FTR_ENROLL_DATA) in order to
 *                                identify the version of the structure being passed.
 *                                If a caller does not initialize dwSize, the function fails.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTREnrollX( FTR_USER_CTX UserContext, FTR_PURPOSE Purpose, FTR_DATA_PTR pTemplate, FTR_ENROLL_DATA_PTR pEData );

/*
 * FTREnroll - creates the fingerprint template for the desired purpose.
 *
 * Parameters:
 *   FTR_USER_CTX UserContext - optional caller-supplied value that is passed to callback
 *                              functions. This value is provided for convenience in
 *                              application design.
 *
 *   FTR_PURPOSE Purpose - the purpose of template building. This value designates the
 *                         intended way of further template usage and can be one of the
 *                         following:
 *
 *                         FTR_PURPOSE_ENROLL - the created template is suitable for both
 *                         identification and verification purpose.
 *
 *                         FTR_PURPOSE_IDENTIFY - corresponding template can be used only
 *                         for identification as an input for the FTRSetBaseTemplate function.
 *
 *   FTR_DATA_PTR pTemplate - pointer to a result memory buffer. The space for this buffer
 *                            must be reservered by a caller. Maximum space amount can be
 *                            determined through the FTRGetParam call with the
 *                            FTR_PARAM_MAX_TEMPLATE_SIZE value of the first argument.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTREnroll( FTR_USER_CTX UserContext, FTR_PURPOSE Purpose, FTR_DATA_PTR pTemplate );

/*
 * FTRVerify - this function captures an image from the currently attached frame source,
 *   builds the corresponding template and compares it with the source template passed
 *   in the pTemplate parameter.
 *
 * Parameters:
 *   FTR_USER_CTX UserContext - optional caller-supplied value that is passed to callback
 *                              functions. This value is provided for convenience in
 *                              application design.
 *
 *   FTR_DATA_PTR pTemplate - pointer to a source template for verification.
 *
 *   BOOL *pResult - points to a value indicating whether the captured image matched to
 *                   the source template.
 *
 *   FTR_FAR *pFARVerify - points to the optional FAR level achieved.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - the input template was not built with the FTR_PURPOSE_ENROLL purpose;
 *   FTR_RETCODE_INVALID_ARG - the template is corrupted or has invalid data.                            
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRVerify( FTR_USER_CTX UserContext, FTR_DATA_PTR pTemplate, DGTBOOL *pResult, FTR_FAR *pFARVerify );

/*
 * FTRVerifyN - this function captures an image from the currently attached frame source,
 *   builds the corresponding template and compares it with the source template passed
 *   in the pTemplate parameter.
 *
 * Parameters:
 *   FTR_USER_CTX UserContext - optional caller-supplied value that is passed to callback
 *                              functions. This value is provided for convenience in
 *                              application design.
 *
 *   FTR_DATA_PTR pTemplate - pointer to a source template for verification.
 *
 *   BOOL *pResult - points to a value indicating whether the captured image matched to
 *                   the source template.
 *
 *   FTR_FARN *pFARVerify - points to the optional FAR Numerical level achieved.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - the input template was not built with the FTR_PURPOSE_ENROLL purpose;
 *   FTR_RETCODE_INVALID_ARG - the template is corrupted or has invalid data.                            
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRVerifyN( FTR_USER_CTX UserContext, FTR_DATA_PTR pTemplate, DGTBOOL *pResult, FTR_FARN *pFARVerify );

/*
 * FTRSetBaseTemplate - installs a template as a base for identification process.
 *   The passed template must have been enrolled for identification purpose, i.e.
 *   the FTR_PURPOSE_IDENTIFY purpose value must be used for its enrollment.
 *   Identification process is organized in one or more FTRIdentify calls.
 *
 * Parameters:
 *   FTR_DATA_PTR pTemplate - pointer to a previously enrolled template.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *   FTR_RETCODE_INVALID_PURPOSE - the template was not built with FTR_PURPOSE_IDENTIFY value.
 *                               
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRSetBaseTemplate( FTR_DATA_PTR pTemplate );

/*
 * FTRIdentify - compares the base template against a set of source templates. The
 *   matching is performed in terms of FAR (False Accepting Ratio), which designates
 *   the probability of falsely matching of the base template to the source template.
 *
 * Parameters:
 *   FTR_IDENTIFY_ARRAY_PTR pAIdent - points to a set of the source templates.
 *
 *   DWORD *pdwMatchCnt - number of matched records in the array pointed to by the pAMatch
 *                        argument. 
 *
 *   FTR_MATCHED_ARRAY_PTR pAMatch - pointer to the array of matched records. A caller is
 *                                   responsible for reserving appropriate memory space and
 *                                   proper initialization of this structure.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - there is a template built with the purpose other than 
 *                                 FTR_PURPOSE_ENROLL value in the pAIdent array.
 *                               
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRIdentify( FTR_IDENTIFY_ARRAY_PTR pAIdent, UDGT32 *pdwMatchCnt, FTR_MATCHED_ARRAY_PTR pAMatch );

/*
 * FTRIdentifyN - compares the base template against a set of source templates. The
 *   matching is performed in terms of FAR (False Accepting Ratio), which designates
 *   the probability of falsely matching of the base template to the source template.
 *
 * Parameters:
 *   FTR_IDENTIFY_ARRAY_PTR pAIdent - points to a set of the source templates.
 *
 *   DWORD *pdwMatchCnt - number of matched records in the array pointed to by the pAMatch
 *                        argument. 
 *
 *   FTR_MATCHED_X_ARRAY_PTR pAMatch - pointer to the array of matched records. A caller is
 *                                     responsible for reserving appropriate memory space and
 *                                     proper initialization of this structure.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success;
 *   FTR_RETCODE_INVALID_PURPOSE - there is a template built with the purpose other than 
 *                                 FTR_PURPOSE_ENROLL value in the pAIdent array.
 *                               
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRIdentifyN( FTR_IDENTIFY_ARRAY_PTR pAIdent, UDGT32 *pdwMatchCnt, FTR_MATCHED_X_ARRAY_PTR pAMatch );

/*
 * FTRGetOptConvParam - gets the optimal conversion parameters produced by the
 *   last comparison operation.
 *
 * Parameters:
 *   DGT32 *pFdx - offset along the X axis measured in the pixels of image.
 *
 *   DGT32 *pFdx - offset along the Y axis measured in the pixels of image.
 *
 *   DGT32 *pRtd - rotation measured in degrees.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *   FTR_RETCODE_INVALID_ARG - template has corrupted or invalid data.                            
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRGetOptConvParam( DGT32 *pFdx, DGT32 *pFdy, DGT32 *pRtd );

/*
 * FTRGetTopLeftImage - gets the top left corner of the image in algorithm
 *   format.
 *
 * Parameters:
 *   DGT32 *pFdx - offset along the X axis measured in the pixels of image.
 *
 *   DGT32 *pFdx - offset along the Y axis measured in the pixels of image.
 *
 * Returns result code.
 *   FTR_RETCODE_OK - success.
 *   FTR_RETCODE_INVALID_ARG - template has corrupted or invalid data.                            
 */
FTRAPIPREFIX FTRAPI_RESULT FTRAPI FTRGetTopLeftImage( DGT32 *pFdx, DGT32 *pFdy );

#ifdef __cplusplus
}; /* end of function prototypes */
#endif

#endif // _FUTRONIC_API_H_
